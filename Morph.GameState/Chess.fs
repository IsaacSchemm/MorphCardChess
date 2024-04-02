namespace Morph.GameState

type Team = Dark | Light

type Suit = Heart | Club | Diamond | Spade

type Piece = {
    Team: Team
    Suit: Suit
}

type Type = Wazir | Knight | Bishop | Rook

type Position = {
    Rank: int
    File: int
}

type Offset = {
    Vertical: int
    Horizontal: int
}

type PiecePosition = {
    Piece: Piece
    Position: Position
    Type: Type
}

type Boagrd = Piece Set

type Direction = Up | Down | Left | Right

module Chess =
    let initialBoard = set [
        let p team suit rank file = {
            Piece = {
                Team = team
                Suit = suit
            }
            Position = {
                Rank = rank
                File = file
            }
            Type = Wazir
        }

        p Dark Heart 2 3
        p Dark Club 2 4
        p Dark Diamond 2 5
        p Dark Spade 2 6
        p Light Heart 7 6
        p Light Club 7 5
        p Light Diamond 7 4
        p Light Spade 7 3
    ]

    let getNewPosition offset position = {
        Rank = position.Rank + offset.Vertical
        File = position.File + offset.Horizontal
    }

    let getPieceByPosition position board =
        board
        |> Seq.where (fun x -> x.Position = position)
        |> Seq.map (fun x -> x.Piece)
        |> Seq.tryExactlyOne

    let outOfBounds position =
        position.Rank < 1
        || position.Rank > 8
        || position.File < 1
        || position.File > 8

    let getLegalMoves board piecePosition = set [
        match piecePosition.Type with
        | Rook ->
            for (x, y) in [(0, 1); (0, -1); (1, 0); (-1, 0)] do
                let mutable pos = piecePosition.Position
                let mutable quit = false
                while not quit do
                    pos <- { Rank = pos.Rank + y; File = pos.File + x }
                    if outOfBounds pos then
                        quit <- true
                    else
                        match getPieceByPosition pos board with
                        | None ->
                            yield pos
                        | Some otherPiece ->
                            if otherPiece.Team <> piecePosition.Piece.Team then
                                yield pos
                            quit <- true
        | Bishop ->
            for (x, y) in [(1, 1); (-1, -1); (1, -1); (-1, 1)] do
                let mutable pos = piecePosition.Position
                let mutable quit = false
                while not quit do
                    if outOfBounds pos then
                        quit <- true
                    else
                        pos <- { Rank = pos.Rank + y; File = pos.File + x }
                        match getPieceByPosition pos board with
                        | None ->
                            yield pos
                        | Some otherPiece ->
                            if otherPiece.Team <> piecePosition.Piece.Team then
                                yield pos
                            quit <- true
        | Knight ->
            for (x, y) in [
                (1, 2)
                (2, 1)
                (-1, 2)
                (-2, 1)
                (1, -2)
                (2, -1)
                (-1, -2)
                (-2, -1)
            ] do
                let pos = piecePosition.Position
                let newPosition = { Rank = pos.Rank + y; File = pos.File + x }

                if not (outOfBounds newPosition) then
                    match getPieceByPosition newPosition board with
                    | Some otherPiece when otherPiece.Team = piecePosition.Piece.Team -> ()
                    | _ -> yield newPosition
        | Wazir ->
            for (x, y) in [
                (1, 0)
                (-1, 0)
                (0, 1)
                (0, -1)
            ] do
                let pos = piecePosition.Position
                let newPosition = { Rank = pos.Rank + y; File = pos.File + x }

                if not (outOfBounds newPosition) then
                    match getPieceByPosition newPosition board with
                    | Some otherPiece when otherPiece.Team = piecePosition.Piece.Team -> ()
                    | _ -> yield newPosition
    ]

    let stringify board = String.concat "\n" [
        for v in [1..8] do
            let rank = 9 - v
            String.concat "" [
                for file in [1..8] do
                    let pos = { Rank = rank; File = file }
                    match getPieceByPosition pos board with
                    | None -> "."
                    | Some piece ->
                        match piece.Suit with
                        | Heart -> "h"
                        | Club -> "c"
                        | Diamond -> "d"
                        | Spade -> "s"
            ]
    ]

    let stringifyAttack board context = String.concat "\n" [
        let attacking = getLegalMoves board context

        for v in [1..8] do
            let rank = 9 - v
            String.concat "" [
                for file in [1..8] do
                    let pos = { Rank = rank; File = file }
                    match Set.contains pos attacking, getPieceByPosition pos board with
                    | (true, _) ->
                        "x"
                    | (false, None) -> "."
                    | (false, Some piece) ->
                        match piece.Suit with
                        | Heart -> "h"
                        | Club -> "c"
                        | Diamond -> "d"
                        | Spade -> "s"
            ]
    ]
