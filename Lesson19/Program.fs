module Program

open System
open Domain
open Operations

let isValidCommand c =
    match c with
    | 'd'
    | 'w'
    | 'x' -> true
    | _ -> false

let isStopCommand c = c = 'x'

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

let processCommand (acc: Account) (t, a) =
    match (t, a) with
    | ('d', v) -> acc |> depositWithAudit v
    | ('w', v) -> acc |> withdrawWithAudit v
    | ('x', v) -> acc
    | _ -> failwith "Impossible"

let transactionsFor ownerName guid =
    let dir = sprintf "/tmp/accounts/%s_%O/" ownerName guid

    if not (IO.Directory.Exists(dir)) then
        None
    else
        let lines =
            IO.Directory.EnumerateFiles dir
            |> Seq.toList
            |> List.map (IO.File.ReadAllText)

        Some(String.Join("\n", lines))



[<EntryPoint>]
let main _ =
    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let openingAccount =
        let transactions =
            transactionsFor name Guid.Empty
            |> Transactions.deserialized

        loadAccount { Name = name } Guid.Empty transactions

    let commands =
        seq {
            while true do
                Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
                yield Console.ReadKey().KeyChar
                Console.WriteLine()
        }

    let getAmount cmd =
        Console.WriteLine()
        Console.Write("Enter Amount: ")
        let input = Console.ReadLine()
        (cmd, Decimal.Parse(input))

    let closingAccount =
        commands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.map getAmount
        |> Seq.fold processCommand openingAccount

    Console.Clear()
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore

    0
