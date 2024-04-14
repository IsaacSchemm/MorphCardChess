namespace Morph.GameState

type DeckType = Euchre | Pinochle | Poker

type Card = { Suit: Suit; Rank: int }

type MovementStage = {
    Card: Card
    Piece: Piece
}

type PromotionStage = {
    Card: Card
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
    let GetPointsBySuit team suit state =
        state.Points |> Seq.where (fun x -> x = (team, suit)) |> Seq.length

    let IsPass team state =
        state.Points |> Seq.contains (team, Heart)
        && state.Points |> Seq.contains (team, Club)
        && state.Points |> Seq.contains (team, Diamond)

    let GetTotalPoints team state =
        if IsPass team state
        then state.Points |> Seq.where (fun x -> fst x = team) |> Seq.length
        else 0

    let GetUnplayedCards state = seq {
        yield! state.LightHand
        yield! state.DarkHand
        yield! state.Deck
    }

    let GetTeamName team =
        match team with
        | Dark -> "Player 1"
        | Light -> "Player 2"

    let GetPromotionOption rank =
        match rank with
        | 13 -> Rook
        | 12 -> Bishop
        | 11 -> Knight
        | _ -> Wazir

    let DescribeType pieceType =
        match pieceType with
        | Rook -> "♜ Cat"
        | Bishop -> "♝ Snake"
        | Knight -> "♞ Bee"
        | Wazir -> "Human"

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

    let DescribeCard card = String.concat " " [
        DescribeSuit card.Suit

        match card.Rank with
        | 13 -> "K"
        | 12 -> "Q"
        | 11 -> "J"
        | 1 -> "A"
        | r -> string r
    ]

    let GetStatusText state =
        match state.Stage with
        | ReplaceCapturedPiece piece ->
            $"""{GetTeamName state.Team}: choose where to place your captured {piece.Suit} piece back on the board (as a {DescribeType Wazir})"""
        | ChooseCard when Seq.isEmpty (GetUnplayedCards state) ->
            match GetTotalPoints Dark state, GetTotalPoints Light state with
            | 0, 0 -> "No winner (neither team got all 3 captures)"
            | d, l when d = l -> $"Tie game with {l} points"
            | _, 0 -> $"{GetTeamName Dark} wins by getting all 3 captures"
            | 0, _ -> $"{GetTeamName Light} wins by getting all 3 captures"
            | d, l when d > l -> $"{GetTeamName Dark} wins with {d} points"
            | d, l when d < l -> $"{GetTeamName Light} wins with {l} points"
            | _ -> "Could not determine winner"
        | ChooseCard ->
            $"{GetTeamName state.Team}: draw or play a card ({List.length state.Deck} cards left in deck)"
        | ChoosePiece card ->
            $"{GetTeamName state.Team}: select a piece to move with the {card.Suit} card"
        | MovePiece stage ->
            $"{GetTeamName state.Team}: select a square to move your {stage.Piece.Suit} piece to"
        | PromotePiece stage ->
            $"""{GetTeamName state.Team}: choose whether you want to turn your {stage.Piece.Suit} piece into a {stage.Card.Rank |> GetPromotionOption |> DescribeType} by virtue of playing a {stage.Card.Rank}"""

    let private random = new System.Random()

    let CreateStartingState deckType = {
        Deck =
            seq {
                let ranks = if deckType = Poker then [1..13] else [9..13]
                let duplicates = if deckType = Pinochle then 2 else 1

                for _ in Seq.replicate duplicates () do
                    for suit in [Heart; Club; Diamond; Spade] do
                        for rank in ranks do
                            { Suit = suit; Rank = rank }
            }
            |> Seq.sortBy (fun _ -> random.Next())
            |> Seq.toList
        DarkHand = []
        LightHand = []
        Points = []
        Board = Chess.initialBoard
        Team = Dark
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

    let rec RemoveCard card hand =
        match hand with
        | [] -> failwith "Card not found"
        | current :: rest when current = card -> rest
        | current :: rest -> current :: RemoveCard card rest

    let PlayCard card state =
        match state.Team with
        | Light ->
            {
                state with
                    LightHand = state.LightHand |> RemoveCard card
                    Stage = ChoosePiece card
            }
        | Dark ->
            {
                state with
                    DarkHand = state.DarkHand |> RemoveCard card
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
                                Card = movementStage.Card
                                Piece = movementStage.Piece
                                PromotionOptions = [
                                    GetPromotionOption movementStage.Card.Rank
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
    Label: string
    Pieces: PiecePosition list
    Enabled: bool
    NextState: State Lazy
    ButtonSuit: Suit option
    Auto: bool
} with
    member this.ColorClass =
        match this.ButtonSuit with
        | Some Heart -> "hearts"
        | Some Club -> "clubs"
        | Some Diamond -> "diamonds"
        | Some Spade -> "spades"
        | None -> ""

module Interactive =
    let MinLengthButtonList len state items =
        let disabledButton = {
            Label = ""
            Pieces = []
            Enabled = false
            NextState = lazy state
            ButtonSuit = None
            Auto = false
        }

        let count = max 0 (len - List.length items)

        items @ (List.replicate count disabledButton)

    let GetPromotionButtons team state = MinLengthButtonList 2 state [
        if state.Team = team then
            match state.Stage with
            | PromotePiece promotionStage ->
                for pc in promotionStage.PromotionOptions do
                    {
                        Label = State.DescribeType pc
                        Pieces = []
                        Enabled = true
                        NextState = lazy (state |> State.PromotePiece pc)
                        ButtonSuit = Some promotionStage.Piece.Suit
                        Auto = true
                    }
            | _ -> ()
    ]

    let GetHandButtons team state = MinLengthButtonList 3 state [
        let step =
            state.Team = team
            && state.Stage = ChooseCard
        let hand =
            match team with
            | Light -> state.LightHand
            | Dark -> state.DarkHand
        for index in [0..2] do
            if List.length hand > index then
                let card = hand[index]
                {
                    Label = State.DescribeCard card
                    Pieces = []
                    Enabled = step && (hand.Length >= 3 || state.Deck = [])
                    NextState = lazy (state |> State.PlayCard card)
                    ButtonSuit = Some card.Suit
                    Auto = false
                }
            else
                {
                    Label = "Draw"
                    Pieces = []
                    Enabled = step
                    NextState = lazy (state |> State.Draw team)
                    ButtonSuit = None
                    Auto = true
                }
    ]

    let GetFiveButtonRow team state =
        GetPromotionButtons team state @ GetHandButtons team state |> List.truncate 5

    let DescribePiece (piece: Piece) = String.concat " " [
        State.GetTeamName piece.Team
        State.DescribeSuit piece.Suit
    ]

    let DescribePosition (pos: Position) = String.concat "" [
        string (char (int 'a' + pos.File - 1))
        string pos.Rank
    ]

    let GetBoardButton state rank file =
        let pos = { Rank = rank; File = file }
        let pieceAtThisPosition =
            state.Board
            |> Seq.where (fun pp -> pp.Position = pos)
            |> Seq.tryExactlyOne
        let properties =
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
                {
                    Label = ""
                    Pieces = []
                    Enabled =
                        inBack
                        && not (state.Board |> Seq.map (fun x -> x.Position) |> Seq.contains pos)
                        && Set.isEmpty (squaresIWouldAttack |> Set.intersect opponentPositions)
                    NextState = lazy (state |> State.PlacePiece pos capturedPiece)
                    ButtonSuit = Some capturedPiece.Suit
                    Auto = false
                }
            | ChoosePiece card, Some pieceHere when pieceHere.Piece.Team = state.Team ->
                // Determining which piece to move after playing a card
                {
                    Label = ""
                    Pieces = []
                    Enabled = card.Suit = pieceHere.Piece.Suit || card.Suit = Spade
                    NextState = lazy (state |> State.SelectPiece pieceHere.Piece)
                    ButtonSuit = Some pieceHere.Piece.Suit
                    Auto = true
                }
            | MovePiece movementStage, _ ->
                // Determine where to move the piece to
                {
                    Label = ""
                    Pieces = []
                    Enabled =
                        state.Board
                        |> Seq.where (fun pp -> pp.Piece = movementStage.Piece)
                        |> Seq.collect (fun pp -> Chess.getLegalMoves state.Board pp)
                        |> Seq.contains pos
                    NextState = lazy (state |> State.MovePiece pos)
                    ButtonSuit =
                        pieceAtThisPosition
                        |> Option.map (fun pp -> pp.Piece.Suit)
                    Auto = false
                }
            | _ ->
                // Board inactive
                {
                    Label = ""
                    Pieces = []
                    Enabled = false
                    NextState = lazy state
                    ButtonSuit =
                        pieceAtThisPosition
                        |> Option.map (fun pp -> pp.Piece.Suit)
                    Auto = false
                }

        {
            properties with
                Label = String.concat ": " [
                    if properties.Enabled then
                        DescribePosition pos

                    match pieceAtThisPosition with
                    | Some px -> DescribePiece px.Piece
                    | _ -> ()
                ]
                Pieces = Option.toList pieceAtThisPosition
        }

    let GetBoardButtons state = [
        for rank in List.rev [1..8] do [
            for file in [1..8] do
                GetBoardButton state rank file
        ]
    ]

    let GetAllButtons state = [
        yield! GetHandButtons Light state
        yield! GetPromotionButtons Light state
        for row in GetBoardButtons state do
            yield! row
        yield! GetHandButtons Dark state
        yield! GetPromotionButtons Dark state
    ]
