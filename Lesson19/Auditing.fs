module Auditing

open Operations
open Domain

/// Logs to the console
let printTransaction _ accountId transaction =
    printfn "Account %O: %O" accountId (Transactions.serialized transaction)

// Logs to both console and file system
let composedLogger =
    let loggers =
        [
            FileRepository.writeTransaction
            printTransaction
        ]

    fun accountId owner message ->
        loggers
        |> List.iter (fun logger -> logger accountId owner message)
