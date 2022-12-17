module Operations

open System
open Domain

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount account =
    if amount > account.Balance then
        account
    else
        { account with
            Balance = account.Balance - amount
        }

/// Deposits an amount into an account
let deposit amount account =
    { account with
        Balance = account.Balance + amount
    }

let newAccount (acc: Account) (tx: Transaction) : Account =
    let newAccount =
        match tx.OperationName with
        | "withdraw" -> (withdraw tx.Amount acc)
        | "deposit" -> (deposit tx.Amount acc)
        | _ -> failwith "Bad data!"

    printfn "OB=%O, NB=%O" acc.Balance newAccount.Balance
    newAccount

let loadAccount (owner: Customer) (accountID: Guid) (transactions: Transaction list) : Account =
    let startBalance = 0M

    let account =
        {
            Owner = owner
            Balance = startBalance
            AccountId = accountID
        }

    let res: Account =
        transactions
        |> List.sortBy (fun tx -> tx.TimeStamp)
        |> List.filter (fun tx -> tx.AccountId = accountID && tx.Success)
        |> List.fold (fun acc tx -> newAccount acc tx) account

    res


/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName audit operation amount (account: Account) =
    let newAccount: Account = operation amount account

    let transaction: Transaction =
        {
            TimeStamp = DateTime.UtcNow
            OperationName = operationName
            AccountId = account.AccountId
            Amount = amount
            Success = (account <> newAccount)
        }

    audit account.AccountId account.Owner.Name transaction

    newAccount
