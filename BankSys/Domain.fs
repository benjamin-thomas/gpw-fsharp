namespace BankSys.Domain

type Customer = { Name: string }

type Account =
    { Owner: Customer
      Id: int
      StartBalance: int }

type Transaction =
    { FromAccount: Account
      ToAccount: Account
      Amount: int }


