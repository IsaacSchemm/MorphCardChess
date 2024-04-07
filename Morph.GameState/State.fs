namespace Morph.GameState

open System.Drawing

type Card = { Suit: Suit; Rank: int }

type MovementStage = {
    Card: Card
    Piece: Piece
}

type PromotionStage = {
    Piece: Piece
    PromotionOptions: Type list
    CapturedPiece: Piece option
}

type Stage =
| ReplaceCapturedPiece of Piece
| ChooseCard
| ChoosePiece of Card
| MovePiece of MovementStage
| PromotePiece of PromotionStage

type State = {
    Deck: Card list
    DarkHand: Card list
    LightHand: Card list
    Points: (Team * Suit) list
    Board: PiecePosition Set
    Team: Team
    Stage: Stage
}

module State =
    let IsPass team state =
        state.Points |> Seq.contains (team, Heart)
        && state.Points |> Seq.contains (team, Club)
        && state.Points |> Seq.contains (team, Diamond)

    let GetTotalPoints team state =
        if IsPass team state
        then state.Points |> Seq.where (fun x -> fst x = team) |> Seq.length
        else 0

    let Describe state =
        match state.Stage, state.DarkHand, state.LightHand, state.Team with
        | ChooseCard, [], [], _ ->
            match GetTotalPoints Dark state, GetTotalPoints Light state with
            | 0, 0 -> "No winner (neither team got all 3 captures)"
            | _, 0 -> "Player 1 wins by getting all 3 captures"
            | 0, _ -> "Player 2 wins by getting all 3 captures"
            | d, l when d > l -> $"Player 1 wins with {d} points"
            | d, l when d < l -> $"Player 2 wins with {l} points"
            | d, l when d = l -> $"Tie game with {l} points"
            | _ -> "Could not determine winner"
        | _, _, _, Dark ->
            $"Player 1's turn ({List.length state.Deck} cards left in deck)"
        | _, _, _, Light ->
            $"Player 2's turn ({List.length state.Deck} cards left in deck)"

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
        Points = []
        Board = Chess.initialBoard
        Team = firstTeam
        Stage = ChooseCard
    }

    let Draw team state =
        match state.Deck, state.LightHand.Length with
        | top::rest, len when len < 3 && team = state.Team ->
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
                state with
                    Stage = MovePiece {
                        Card = card
                        Piece = piece
                    }
            }
        | _ -> state

    let MovePiece newPosition state =
        match state.Stage with
        | MovePiece movementStage ->
            let existingPP =
                state.Board
                |> Seq.where (fun pp -> pp.Piece = movementStage.Piece)
                |> Seq.head
            let legalMoves = Chess.getLegalMoves state.Board existingPP
            if Seq.contains newPosition legalMoves then
                let capturedPiece = List.tryExactlyOne [
                    for pp in state.Board do
                        if pp.Position = newPosition && pp.Piece <> movementStage.Piece then
                            yield pp.Piece
                ]

                {
                    state with
                        Points = [
                            if movementStage.Piece.Suit <> Spade && capturedPiece <> None then
                                yield (state.Team, movementStage.Piece.Suit)

                            yield! state.Points
                        ]
                        Board = Set.ofList [
                            for pp in state.Board do
                                if pp.Position <> newPosition && pp.Piece <> movementStage.Piece then
                                    yield pp
                            yield {
                                existingPP with
                                    Position = newPosition
                            }
                        ]
                        Stage =
                            PromotePiece {
                                Piece = movementStage.Piece
                                PromotionOptions = [
                                    match movementStage.Card.Rank with
                                    | 13 -> Rook
                                    | 12 -> Bishop
                                    | 11 -> Knight
                                    | _ -> Wazir

                                    existingPP.Type
                                ]
                                CapturedPiece = capturedPiece
                            }
                }
            else
                state
        | _ -> state

    let PromotePiece t state =
        match state.Stage with
        | PromotePiece promotionStage ->
            let nextStage =
                match promotionStage.CapturedPiece with
                | Some capturedPiece -> ReplaceCapturedPiece capturedPiece
                | None -> ChooseCard
            {
                state with
                    Board = Set.ofList [
                        for pp in state.Board do
                            if pp.Piece <> promotionStage.Piece then
                                yield pp
                            else
                                yield { pp with Type = t }
                    ]
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
            Stage = ChooseCard
    }

type Label = Text of text: string | Image of path: string

type InteractiveButton = {
    Label: Label
    Enabled: bool
    NextState: State Lazy
    Color: Color option
    Auto: bool
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

    let DescribeScore team state =
        seq {
            let myPoints = [
                for pointTeam, suit in state.Points do
                    if team = pointTeam then
                        suit
            ]

            for suit in myPoints do
                DescribeSuit suit

            let hasAll =
                myPoints |> List.contains Heart
                && myPoints |> List.contains Club
                && myPoints |> List.contains Diamond

            if hasAll then
                sprintf "(%d)" (List.length myPoints)
        }
        |> Seq.sortBy id
        |> String.concat " "

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
        | Rook -> "♜ Cat"
        | Bishop -> "♝ Snake"
        | Knight -> "♞ Bee"
        | Wazir -> "Human"

    let GetPromotionButtons team state =
        seq {
            if state.Team = team then
                match state.Stage with
                | PromotePiece promotionStage ->
                    for pc in promotionStage.PromotionOptions do
                        {
                            Label = Text (DescribeType pc)
                            Enabled = true
                            NextState = lazy (state |> State.PromotePiece pc)
                            Color = None
                            Auto = true
                        }
                | _ -> ()
            while true do
                {
                    Label = Text ""
                    Enabled = false
                    NextState = lazy state
                    Color = None
                    Auto = false
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
                    Label = Text (DescribeCard card)
                    Enabled = step && (hand.Length >= 3 || state.Deck = [])
                    NextState = lazy (state |> State.PlayCard card)
                    Color = Some (GetColor card.Suit)
                    Auto = false
                }
            for _ in state.Deck do
                {
                    Label = Text "Draw"
                    Enabled = step
                    NextState = lazy (state |> State.Draw team)
                    Color = None
                    Auto = true
                }
            while true do
                {
                    Label = Text ""
                    Enabled = false
                    NextState = lazy state
                    Color = None
                    Auto = false
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
                    | Some px -> Image (DescribePiecePosition px)
                    | _ when inBack -> Text (DescribePosition pos)
                    | _ -> Text ""
                Enabled =
                    inBack
                    && not (state.Board |> Seq.map (fun x -> x.Position) |> Seq.contains pos)
                    && Set.isEmpty (squaresIWouldAttack |> Set.intersect opponentPositions)
                NextState = lazy (state |> State.PlacePiece pos capturedPiece)
                Color = Some (GetColor capturedPiece.Suit)
                Auto = false
            }
        | ChoosePiece card, Some pieceHere when pieceHere.Piece.Team = state.Team ->
            // Determining which piece to move after playing a card
            {
                Label = Image (DescribePiecePosition pieceHere)
                Enabled = card.Suit = pieceHere.Piece.Suit || card.Suit = Spade
                NextState = lazy (state |> State.SelectPiece pieceHere.Piece)
                Color = Some (GetColor pieceHere.Piece.Suit)
                Auto = true
            }
        | MovePiece movementStage, _ ->
            // Determine where to move the piece to
            {
                Label =
                    match pieceAtThisPosition with
                    | Some piecePos -> Image (DescribePiecePosition piecePos)
                    | None -> Text ""
                Enabled =
                    state.Board
                    |> Seq.where (fun pp -> pp.Piece = movementStage.Piece)
                    |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                    |> Seq.contains pos
                NextState = lazy (state |> State.MovePiece pos)
                Color =
                    pieceAtThisPosition
                    |> Option.map (fun pp -> GetColor pp.Piece.Suit)
                Auto = false
            }
        | _ ->
            // Board inactive
            {
                Label =
                    match pieceAtThisPosition with
                    | Some piecePos -> Image (DescribePiecePosition piecePos)
                    | None -> Text ""
                Enabled = false
                NextState = lazy state
                Color =
                    pieceAtThisPosition
                    |> Option.map (fun pp -> GetColor pp.Piece.Suit)
                Auto = false
            }

    let GetBoardButtons state = [
        for rank in List.rev [1..8] do [
            for file in [1..8] do
                GetBoardButton state rank file
        ]
    ]

    let GetAllButtons state = [
        yield! GetHandAndPromotionButtons Light state
        for row in GetBoardButtons state do
            yield! row
        yield! GetHandAndPromotionButtons Dark state
    ]
