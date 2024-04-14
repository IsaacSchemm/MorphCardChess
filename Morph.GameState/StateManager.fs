namespace Morph.GameState

type StateManager() =
    let stateChanged = new Event<unit>()

    let mutable states = []
    let mutable index = -1

    let rec findNextState state =
        let buttons =
            Interactive.GetAllButtons state
            |> List.where (fun b -> b.Enabled)
            |> List.map (fun b -> b.NextState.Value, b.Auto)
            |> List.distinct

        match buttons with
        | [(st, true)] -> findNextState st
        | _ -> state

    [<CLIEvent>]
    member _.StateChanged = stateChanged.Publish

    member val AutoAdvance = true with get, set

    member this.State
        with get () = states[index]
        and set requestedNextState =
            match states |> List.tryFindIndex (fun x -> x = requestedNextState) with
            | Some i ->
                index <- i
            | None ->
                while index > 0 do
                    states <- List.tail states
                    index <- index - 1
                let effectiveNextState =
                    if this.AutoAdvance
                    then findNextState requestedNextState
                    else requestedNextState
                states <- effectiveNextState :: states
                index <- 0
            stateChanged.Trigger()

    member _.Clear() =
        index <- -1
        states <- []

    member _.PreviousStates =
        states
        |> Seq.skip (index + 1)

    member _.NextStates = seq {
        let mutable i = index
        while i > 0 do
            i <- i - 1
            states[i]
    }
