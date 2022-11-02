module BankSys.Audit

open Operations
let transferWithConsoleAudit = transfer System.Console.WriteLine
let transferWithFileAudit logFile = transfer (fun x -> System.IO.File.AppendAllText(logFile, x + "\n"))