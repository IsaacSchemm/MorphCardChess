namespace Morph.GameState

open System.Drawing

type Card = { Suit: Suit; Rank: int }

type Stage =
| ReplaceCapturedPiece of Piece
| ChooseCard
| ChoosePiece of Card
| MovePiece of Card * Piece
| PromotePiece of Piece * Type list

type State = {
    Deck: Card list
    DarkHand: Card list
    LightHand: Card list
    Captured: Piece list
    Board: PiecePosition Set
    Team: Team
    Stage: Stage
}

module State =
    let private random = new System.Random()

    let CreateStartingState firstTeam = {
        Deck =
            seq {
                for suit in [Heart; Club; Diamond; Spade] do
                    for rank in [9..13] do
                        { Suit = suit; Rank = rank }
            }
            |> Seq.sortBy (fun _ -> random.Next())
            |> Seq.toList
        DarkHand = []
        LightHand = []
        Captured = []
        Board = Chess.initialBoard
        Team = firstTeam
        Stage = ChooseCard
    }

    let Draw team state =
        match  state.Deck, state.LightHand.Length with
        | top :: rest, len when len < 3 && team = state.Team ->
            {
                state with
                    Deck = rest
                    LightHand = [
                        if team = Light then top
                        yield! state.LightHand
                    ]
                    DarkHand = [
                        if team = Dark then top
                        yield! state.DarkHand
                    ]
            }
        | _ -> state

    let PlayCard card state = {
        state with
            LightHand = state.LightHand |> List.except [card]
            DarkHand = state.DarkHand |> List.except [card]
            Stage = ChoosePiece card
    }

    let SelectPiece piece state =
        match state.Stage with
        | ChoosePiece card ->
            {
                state with Stage = MovePiece (card, piece)
            }
        | _ -> state

    let MovePiece newPosition state =
        match state.Stage with
        | MovePiece (card, piece) ->
            let existingPP =
                state.Board
                |> Seq.where (fun pp -> pp.Piece = piece)
                |> Seq.head
            let legalMoves = Chess.getLegalMoves state.Board existingPP
            if Seq.contains newPosition legalMoves then
                {
                    state with
                        Board = Set.ofList [
                            for pp in state.Board do
                                if pp.Position <> newPosition && pp.Piece <> piece then
                                    yield pp
                            yield {
                                existingPP with
                                    Position = newPosition
                            }
                        ]
                        Captured = [
                            yield! state.Captured
                            for pp in state.Board do
                                if pp.Position = newPosition && pp.Piece <> piece then
                                    yield pp.Piece
                        ]
                        Stage =
                            let s =
                                match card.Rank with
                                | 13 -> Rook
                                | 12 -> Bishop
                                | 11 -> Knight
                                | _ -> Wazir
                            PromotePiece (piece, [existingPP.Type; s])
                }
            else
                state
        | _ -> state

    let PromotePiece t state =
        match state.Stage with
        | PromotePiece (piece, _) ->
            let captured, nextStage =
                match state.Captured with
                | c::rest -> rest, ReplaceCapturedPiece c
                | [] -> [], ChooseCard
            {
                state with
                    Board = Set.ofList [
                        for pp in state.Board do
                            if pp.Piece <> piece then
                                yield pp
                            else
                                yield { pp with Type = t }
                    ]
                    Captured = captured
                    Team = match state.Team with Light -> Dark | Dark -> Light
                    Stage = nextStage
            }
        | _ -> state

    let PlacePiece newPosition piece state = {
        state with
            Board = Set.ofList [
                for pp in state.Board do
                    if pp.Position <> newPosition && pp.Piece <> piece then
                        yield pp
                yield {
                    Piece = piece
                    Position = newPosition
                    Type = Wazir
                }
            ]
            Captured = state.Captured |> List.except [piece]
            Stage = ChooseCard
    }

type InteractiveButton = {
    Label: string
    Enabled: bool
    NextState: State Lazy
    Color: Color option
} with
    member this.ForeColor =
        match this.Color with
        | Some color -> color
        | _ -> SystemColors.ControlText

module Interactive =
    let DescribeSuit suit =
        match suit with
        | Heart -> "♥"
        | Club -> "♣"
        | Diamond -> "♦"
        | Spade -> "♠"

    let GetColor suit =
        match suit with
        | Heart -> Color.Red
        | Club -> Color.DarkGreen
        | Diamond -> Color.Blue
        | Spade -> Color.Black

    let DescribeCard card = String.concat " " [
        DescribeSuit card.Suit

        match card.Rank with
        | 13 -> "K"
        | 12 -> "Q"
        | 11 -> "J"
        | r -> string r
    ]

    let DescribeType t =
        match t with
        | Rook -> "♜ Rook"
        | Bishop -> "♝ Bishop"
        | Knight -> "♞ Knight"
        | Wazir -> "Wazir"

    let GetPromotionButtons team state =
        seq {
            if state.Team = team then
                match state.Stage with
                | PromotePiece (_, types) ->
                    for pc in types do
                        {
                            Label = DescribeType pc
                            Enabled = true
                            NextState = lazy (state |> State.PromotePiece pc)
                            Color = None
                        }
                | _ -> ()
            while true do
                {
                    Label = ""
                    Enabled = false
                    NextState = lazy state
                    Color = None
                }
        }
        |> Seq.truncate 2
        |> Seq.toList

    let GetHandButtons team state =
        seq {
            let step =
                state.Team = team
                && state.Stage = ChooseCard
            let hand =
                match team with
                | Light -> state.LightHand
                | Dark -> state.DarkHand
            for card in hand do
                {
                    Label = DescribeCard card
                    Enabled = step && (hand.Length >= 3 || state.Deck = [])
                    NextState = lazy (state |> State.PlayCard card)
                    Color = Some (GetColor card.Suit)
                }
            for _ in state.Deck do
                {
                    Label = "Draw"
                    Enabled = step
                    NextState = lazy (state |> State.Draw team)
                    Color = None
                }
            while true do
                {
                    Label = ""
                    Enabled = false
                    NextState = lazy state
                    Color = None
                }
        }
        |> Seq.truncate 3
        |> Seq.toList

    let GetHandAndPromotionButtons team state =
        GetPromotionButtons team state @ GetHandButtons team state

    let DescribePiece (piece: Piece) = String.concat " " [
        match piece.Team with
        | Light -> "▼"
        | Dark -> "▲"

        DescribeSuit piece.Suit
    ]

    let DescribePosition (pos: Position) = String.concat "" [
        string (char (int 'a' + pos.File - 1))
        string pos.Rank
    ]

    let DescribePiecePosition (pp: PiecePosition) = String.concat "" [
        "../../../cards/"

        match pp.Piece.Suit with
        | Heart -> "hearts"
        | Club -> "clubs"
        | Diamond -> "diamonds"
        | Spade -> "spades"

        "/"

        match pp.Piece.Team, pp.Type with
        | Light, Rook -> "8"
        | Light, Bishop -> "7"
        | Light, Knight -> "6"
        | Light, Wazir -> "5"
        | Dark, Rook -> "4"
        | Dark, Bishop -> "3"
        | Dark, Knight -> "2"
        | Dark, Wazir -> "1"

        ".png"
    ]

    let GetBoardButton state rank file =
        let pos = { Rank = rank; File = file }
        let pieceAtThisPosition =
            state.Board
            |> Seq.where (fun pp -> pp.Position = pos)
            |> Seq.tryExactlyOne
        match state.Stage, pieceAtThisPosition with
        | ReplaceCapturedPiece capturedPiece, _ ->
            // Determining where to place a captured piece
            let inBack =
                match state.Team, rank with
                | Light, 8
                | Light, 7
                | Dark, 1
                | Dark, 2 -> true
                | _ -> false
            let squaresIWouldAttack =
                Chess.getLegalMoves state.Board { Piece = capturedPiece; Position = pos; Type = Wazir }
            let opponentPositions =
                state.Board
                |> Seq.map (fun pp -> pp.Position)
                |> Set.ofSeq
            let pieceAtThisPosition =
                state.Board
                |> Seq.where (fun pp -> pp.Position = pos)
                |> Seq.tryExactlyOne
            {
                Label =
                    match pieceAtThisPosition with
                    | Some px -> DescribePiecePosition px
                    | _ -> DescribePosition pos
                Enabled =
                    inBack
                    && not (state.Board |> Seq.map (fun x -> x.Position) |> Seq.contains pos)
                    && Set.isEmpty (squaresIWouldAttack |> Set.intersect opponentPositions)
                NextState = lazy (state |> State.PlacePiece pos capturedPiece)
                Color = Some (GetColor capturedPiece.Suit)
            }
        | ChoosePiece card, Some pieceHere when pieceHere.Piece.Team = state.Team ->
            // Determining which piece to move after playing a card
            {
                Label = DescribePiecePosition pieceHere
                Enabled = card.Suit = pieceHere.Piece.Suit || card.Suit = Spade
                NextState = lazy (state |> State.SelectPiece pieceHere.Piece)
                Color = Some (GetColor pieceHere.Piece.Suit)
            }
        | MovePiece (_, piece), _ ->
            // Determine where to move the piece to
            {
                Label =
                    match pieceAtThisPosition with
                    | Some piecePos -> DescribePiecePosition piecePos
                    | None -> ""
                Enabled =
                    state.Board
                    |> Seq.where (fun pp -> pp.Piece = piece)
                    |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                    |> Seq.contains pos
                NextState = lazy (state |> State.MovePiece pos)
                Color =
                    pieceAtThisPosition
                    |> Option.map (fun pp -> GetColor pp.Piece.Suit)
            }
        | _ ->
            // Board inactive
            {
                Label =
                    match pieceAtThisPosition with
                    | Some piecePos -> DescribePiecePosition piecePos
                    | None -> ""
                Enabled = false
                NextState = lazy state
                Color =
                    pieceAtThisPosition
                    |> Option.map (fun pp -> GetColor pp.Piece.Suit)
            }

    let GetBoardButtons state = [
        for rank in List.rev [1..8] do [
            for file in [1..8] do
                GetBoardButton state rank file
        ]
    ]
