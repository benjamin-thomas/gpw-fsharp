module BankSys.Operations

open BankSys.Domain

let calcBalance account transactions =
    let transactionsForAccount =
        transactions
        |> List.filter (fun tx -> account = tx.FromAccount || account = tx.ToAccount)

    let applyAccountTransaction =
        fun acc tx ->
            if tx.FromAccount = account then
                acc - tx.Amount
            else
                acc + tx.Amount

    transactionsForAccount
    |> List.fold applyAccountTransaction account.StartBalance
    
let private balanceRecap account nextBalance =
                $"%s{account.Owner.Name} balance = %d{nextBalance}"    

let private transferFinal audit fromAccount toAccount amount transactions = 
            let transaction =
                { FromAccount = fromAccount
                  ToAccount = toAccount
                  Amount = amount }

            let currFromBalance =
                calcBalance fromAccount transactions

            let nextFromBalance =
                calcBalance fromAccount (transaction :: transactions)

            if nextFromBalance >= 0 then
                let nextToBalance =
                    calcBalance toAccount (transaction :: transactions)

                let fromBalanceRecap =
                    balanceRecap fromAccount nextFromBalance

                let toBalanceRecap =
                    balanceRecap toAccount nextToBalance

                audit $"OK (%s{fromBalanceRecap}, %s{toBalanceRecap})"
                transaction :: transactions
            else
                audit $"REJECTED! Not enough funds (curr = %d{currFromBalance}, tx_amount = %d{amount})"
                transactions

let transfer audit fromAccount toAccount amount transactions =
    if amount <= 0 then
        audit $"REJECTED! Transaction amount must be positive (tx_amount = %d{amount})"
        transactions
    else
        transferFinal audit fromAccount toAccount amount transactions
