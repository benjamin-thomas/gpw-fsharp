module Transactions

open Domain
open System

let serialized (transaction: Transaction) =
    sprintf "%O***%s***%M***%b" transaction.TimeStamp transaction.OperationName transaction.Amount transaction.Success

let deserialized (input: string option) =
    let fromLine (s: string) : Transaction =
        let elems = s.Split("***") in

        {
            TimeStamp = DateTime.Parse(elems[0]) // can't parse as UTC!
            AccountId = Guid.Empty
            OperationName = elems[1]
            Amount = Decimal.Parse(elems[2])
            Success = Boolean.Parse(elems[3])
        }

    match input with
    | None -> []
    | Some (txt) ->
        let lines = (txt.Split("\n") |> Array.toList)
        lines |> List.map fromLine
