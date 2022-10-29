open System
open Car

let getDestination () =
    Console.Write("\nEnter destination: ")
    Console.ReadLine()

let mutable petrol = 100

while true do
        try
            let destination = getDestination ()
            printfn $"Trying to drive to %s{destination}"
            petrol <- driveTo (petrol, destination)
            printfn $"Made it to %s{destination}! You have %d{petrol} petrol left"
        with ex -> printfn $"ERROR: %s{ex.Message}"