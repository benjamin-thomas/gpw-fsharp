open BankSys.Domain
open BankSys.Audit

// SETUP
let me = { Name = "Ben" }
let shop = { Name = "Shop" }
let employer = { Name = "Employer" }

let myAccount =
    { Owner = me
      Id = 1
      StartBalance = 100 }

let shopAccount =
    { Owner = shop
      Id = 2
      StartBalance = 0 }

let employerAccount =
    { Owner = employer
      Id = 3
      StartBalance = 600 }

let transactionsStart: Transaction list = []

// UTILS

let rec getAmount () =
    printf "How much did you spend on groceries? "
    let line = System.Console.ReadLine()
    match System.Int32.TryParse line with
    | true, n -> n
    | _ ->
        printfn "Bad input, try again!"
        getAmount ()

// let buyGroceries = transfer myAccount shopAccount

let logFile = "/tmp/tmp"
System.IO.File.Delete(logFile)

// START
let mutable transactions = transactionsStart
while true do
    let amount = getAmount ()
    transactions <- (transferWithFileAudit logFile) myAccount shopAccount amount transactions
    // transactions <- transferWithConsoleAudit myAccount shopAccount amount transactions
    printfn ""
