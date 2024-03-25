namespace Morph.GameState

type Card = { Suit: Suit; Rank: int }

type Step = {
    Team: Team
    Card: Card option
    Piece: Piece option
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
                                Piece = existingPP.Piece
                                Position = newPosition
                                Type =
                                    match state.Step.Card.Value.Rank with
                                    | 13 -> Rook
                                    | 12 -> Bishop
                                    | 11 -> Knight
                                    | _ -> Wazir
                            }
                        ]
                        Captured = [
                            yield! state.Captured
                            for pp in state.Board do
                                if pp.Position = newPosition && pp.Piece.Team = Dark then
                                    yield pp.Piece
                        ]
                        Step = {
                            Team = match state.Step.Team with Light -> Dark | Dark -> Light
                            Card = None
                            Piece = None
                        }
                }
            else
                state

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
            Captured = []
    }

type InteractiveButton = {
    Label: string
    Enabled: bool
    NextState: State Lazy
}

module Interactive =
    let DescribeSuit suit =
        match suit with
        | Heart -> "♥"
        | Club -> "♣"
        | Diamond -> "♦"
        | Spade -> "♠"

    let DescribeCard card = String.concat " " [
        DescribeSuit card.Suit

        match card.Rank with
        | 13 -> "K"
        | 12 -> "Q"
        | 11 -> "J"
        | r -> string r
    ]

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
                }
            while true do
                {
                    Label = "Draw"
                    Enabled = step && state.Deck <> []
                    NextState = lazy (state |> State.Draw team)
                }
        }
        |> Seq.truncate 3
        |> Seq.toList

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

    let DescribePiecePosition (pp: PiecePosition) = String.concat "\r\n" [
        DescribePiece pp.Piece

        match pp.Type with
        | Rook -> "♜"
        | Bishop -> "♝"
        | Knight -> "♞"
        | Wazir -> ""
    ]

    let GetBoardButtons state = [
        match state.Captured with
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
                    let attackedSquares =
                        state.Board
                        |> Seq.where (fun x -> x.Piece.Team <> state.Step.Team)
                        |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                    let existingPP =
                        state.Board
                        |> Seq.where (fun pp -> pp.Position = pos)
                        |> Seq.tryExactlyOne
                    {
                        Label =
                            match existingPP with
                            | Some px -> DescribePiecePosition px
                            | _ -> DescribePosition pos
                        Enabled =
                            inBack
                            && not (state.Board |> Seq.map (fun x -> x.Position) |> Seq.contains pos)
                            && not (attackedSquares |> Seq.contains pos)
                        NextState = lazy (state |> State.PlacePiece pos capturedPiece)
                    }
            ]
        | [] ->
            for rank in List.rev [1..8] do [
                for file in [1..8] do
                    let pos = { Rank = rank; File = file }
                    let existingPP =
                        state.Board
                        |> Seq.where (fun pp -> pp.Position = pos)
                        |> Seq.tryExactlyOne
                    match existingPP with
                    | Some px when px.Piece.Team = state.Step.Team ->
                        {
                            Label = DescribePiecePosition px
                            Enabled =
                                match state.Step.Card, state.Step.Piece with
                                | Some card, None ->
                                    card.Suit = px.Piece.Suit || card.Suit = Spade
                                | _ ->
                                    false
                            NextState = lazy (state |> State.SelectPiece px.Piece)
                        }
                    | Some px ->
                        {
                            Label = DescribePiecePosition px
                            Enabled =
                                state.Board
                                |> Seq.where (fun pp -> Some pp.Piece = state.Step.Piece)
                                |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                                |> Seq.contains pos
                            NextState = lazy (state |> State.MovePiece pos)
                        }
                    | None ->
                        {
                            Label = DescribePosition pos
                            Enabled =
                                state.Board
                                |> Seq.where (fun pp -> Some pp.Piece = state.Step.Piece)
                                |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                                |> Seq.contains pos
                            NextState = lazy (state |> State.MovePiece pos)
                        }
        ]
    ]
