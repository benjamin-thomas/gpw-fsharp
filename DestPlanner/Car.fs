// https://github.com/isaacabraham/get-programming-fsharp/blob/master/src/code-listings/lesson-08/
module Car

type Petrol = Petrol of int
let fullTank = Petrol 100

type Destination =
    | Home
    | Office
    | Stadium
    | GasStation

let petrolRequiredFor dest =
    match dest with
    | Home -> Petrol 25
    | Office -> Petrol 50
    | Stadium -> Petrol 25
    | GasStation -> Petrol 10

let strToDest str =
    match str with
    | "home" -> Home
    | "office" -> Office
    | "stadium" -> Stadium
    | "station" -> GasStation
    | _ -> failwith $"Unknown dest: %s{str}"

let drive dest (Petrol petrol) =
    let (Petrol required) =
        petrolRequiredFor dest

    let rest = petrol - required

    if rest >= 0 then
        if dest = GasStation then
            Petrol (rest + 50)
        else
            Petrol rest
    else
        failwith "Not enough"


/// Drives to a given destination given a starting amount of petrol
let driveTo (petrol, destination) =
    let (Petrol rem) =
        drive (strToDest destination) (Petrol petrol) in rem
