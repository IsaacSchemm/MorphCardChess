namespace Morph.GameState

open System.Drawing

type Card = { Suit: Suit; Rank: int }

type Step = {
    Team: Team
    Card: Card option
    Piece: Piece option
    PromotionChoices: Type list
}

type State = {
    Deck: Card list
    DarkHand: Card list
    LightHand: Card list
    Captured: Piece list
    Board: PiecePosition Set
    Step: Step
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
        Step = {
            Team = firstTeam
            Card = None
            Piece = None
            PromotionChoices = []
        }
    }

    let Draw team state =
        match  state.Deck, state.LightHand.Length with
        | top :: rest, len when len < 3 && team = state.Step.Team ->
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
            Step = {
                state.Step with Card = Some card
            }
    }

    let SelectPiece piece state = {
        state with
            Step = {
                state.Step with Piece = Some piece
            }
    }

    let MovePiece newPosition state =
        match state.Step.Piece with
        | None -> state
        | Some piece ->
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
                        Step = {
                            state.Step with
                                PromotionChoices =
                                    let s =
                                        match state.Step.Card.Value.Rank with
                                        | 13 -> Rook
                                        | 12 -> Bishop
                                        | 11 -> Knight
                                        | _ -> Wazir
                                    [existingPP.Type; s]
                        }
                }
            else
                state

    let PromotePiece t state =
        match state.Step.Piece with
        | None -> state
        | Some piece -> {
            state with
                Board = Set.ofList [
                    for pp in state.Board do
                        if pp.Piece <> piece then
                            yield pp
                        else
                            yield { pp with Type = t }
                ]
                Step = {
                    Team = match state.Step.Team with Light -> Dark | Dark -> Light
                    Card = None
                    Piece = None
                    PromotionChoices = []
                }
        }

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
            if state.Step.Team = team then
                for pc in state.Step.PromotionChoices do
                    {
                        Label = DescribeType pc
                        Enabled = true
                        NextState = lazy (state |> State.PromotePiece pc)
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
        |> Seq.truncate 2
        |> Seq.toList

    let GetHandButtons team state =
        seq {
            let step =
                state.Step.Team = team
                && state.Step.Card = None
                && state.Step.Piece = None
            let hand =
                match team with
                | Light -> state.LightHand
                | Dark -> state.DarkHand
            for card in hand do
                {
                    Label = DescribeCard card
                    Enabled = step && hand.Length >= 3
                    NextState = lazy (state |> State.PlayCard card)
                    Color = Some (GetColor card.Suit)
                }
            while true do
                {
                    Label = "Draw"
                    Enabled = step && state.Deck <> []
                    NextState = lazy (state |> State.Draw team)
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

    let GetBoardButtons state = [
        match state.Captured |> List.where (fun piece -> piece.Team = state.Step.Team) with
        | capturedPiece::_ ->
            for rank in List.rev [1..8] do [
                for file in [1..8] do
                    let pos = { Rank = rank; File = file }
                    let inBack =
                        match state.Step.Team, rank with
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
            ]
        | [] ->
            for rank in List.rev [1..8] do [
                for file in [1..8] do
                    let pos = { Rank = rank; File = file }
                    let pieceAtThisPosition =
                        state.Board
                        |> Seq.where (fun pp -> pp.Position = pos)
                        |> Seq.tryExactlyOne
                    match pieceAtThisPosition, state.Step.Card, state.Step.Piece with
                    | Some myPiece, Some selectedCard, None when myPiece.Piece.Team = state.Step.Team ->
                        {
                            Label = DescribePiecePosition myPiece
                            Enabled = selectedCard.Suit = myPiece.Piece.Suit || selectedCard.Suit = Spade
                            NextState = lazy (state |> State.SelectPiece myPiece.Piece)
                            Color = Some (GetColor myPiece.Piece.Suit)
                        }
                    | _ ->
                        {
                            Label =
                                match pieceAtThisPosition with
                                | Some piecePos -> DescribePiecePosition piecePos
                                | None -> ""
                            Enabled =
                                match state.Step.Piece, state.Step.PromotionChoices with
                                | Some selectedPiece, [] ->
                                    state.Board
                                    |> Seq.where (fun pp -> pp.Piece = selectedPiece)
                                    |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                                    |> Seq.contains pos
                                | _ ->
                                    false
                            NextState = lazy (state |> State.MovePiece pos)
                            Color =
                                pieceAtThisPosition
                                |> Option.map (fun pp -> GetColor pp.Piece.Suit)
                        }
        ]
    ]
