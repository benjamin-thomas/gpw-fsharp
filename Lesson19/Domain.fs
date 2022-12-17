module Domain

open System

type Customer = { Name: string }

type Account =
    {
        AccountId: Guid
        Owner: Customer
        Balance: decimal
    }

type Transaction =
    {
        TimeStamp: DateTime
        AccountId: Guid
        OperationName: String
        Amount: decimal
        Success: Boolean
    }
