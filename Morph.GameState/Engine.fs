namespace Morph.GameState

module Engine =
    let GetPossibleNextState state = [
        let buttons = Interactive.GetAllButtons state

        for button in buttons do
            if button.Enabled then
                let nextState = button.NextState.Value
                if nextState <> state then
                    yield nextState
    ]

    let ScoreTeam team state = List.sum [
        let pointCounts = state.Points |> Seq.countBy id |> Map.ofSeq
        let getBySuit suit = pointCounts |> Map.tryFind (team, suit) |> Option.defaultValue 0

        yield 2000 * getBySuit Heart
        yield 2000 * getBySuit Club
        yield 2000 * getBySuit Diamond

        if [Heart; Club; Diamond] |> List.forall (fun x -> getBySuit x > 0) then
            yield 5000

        for pp in state.Board do
            if pp.Piece.Team = team && pp.Type <> Wazir then
                if pp.Piece.Suit = Spade
                then yield 50
                else yield 100

        let vulnerablePositions =
            state.Board
            |> Seq.where (fun pp -> pp.Piece.Team <> team)
            |> Seq.collect (Chess.getLegalMoves state.Board)
            |> Set.ofSeq

        for myPiece in state.Board |> Seq.where (fun o -> o.Piece.Team = team) do
            if Set.contains myPiece.Position vulnerablePositions then
                yield -500

            let legalMoves = Chess.getLegalMoves state.Board myPiece

            yield min (Set.count legalMoves) 5

            let attacked =
                state.Board
                |> Seq.where (fun pp -> pp.Piece.Team <> team)
                |> Seq.exists (fun pp -> Set.contains pp.Position legalMoves)

            if attacked then
                yield 250

        let hand =
            match team with
            | Light -> state.LightHand
            | Dark -> state.DarkHand

        for card in hand do
            if card.Rank > 10 then yield 1
            if card.Suit = Spade then yield 1
    ]

    let ScoreState state =
        ScoreTeam Light state - ScoreTeam Dark state

    type Chain = { stack: State list }

    let rec GetPossibleStateChains chain =
        match chain with
        | { stack = [] } -> []
        | { stack = state :: _ } -> [
            let nextStates = GetPossibleNextState state
            let truncated =
                if state.Stage.IsReplaceCapturedPiece
                then [nextStates |> Seq.maxBy ScoreState]
                else nextStates
            for next in truncated do
                if next.Team = state.Team then
                    yield! GetPossibleStateChains { stack = next :: chain.stack }
                else
                    yield { stack = next :: chain.stack }
        ]

    let ScoreStateChain chain =
        match chain with
        | { stack = [] } -> 0
        | { stack = state :: _ } -> ScoreState state

    let GetBestStateChain state =
        let initialChain =
            { stack = [state] }
        let topFive =
            GetPossibleStateChains initialChain
            |> Seq.sortByDescending ScoreStateChain
            |> Seq.truncate 5
            |> Seq.cache
        topFive
        |> Seq.choose (fun candidate ->
            GetPossibleStateChains candidate
            |> Seq.sortBy ScoreStateChain
            |> Seq.tryHead
            |> Option.map (fun nextChain -> {|
                candidate = candidate
                score = ScoreStateChain nextChain
            |}))
        |> Seq.sortByDescending (fun x -> x.score)
        |> Seq.map (fun x -> x.candidate)
        |> Seq.tryHead
        |> Option.orElse (Seq.tryHead topFive)
        |> Option.defaultValue initialChain
