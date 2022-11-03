(List.map (fun i -> i * i) [ 1; 2; 3 ]) = [ 1; 4; 9 ]

let name = "Ben"
let dt = System.DateTime.UtcNow
printfn $"Time is {dt}!"

let a = 1
let b = 2
let c = a + b

let uri = System.Uri "http://fsharp.org"

let (^) x y = float x ** float y


printfn $"Max number of ports: {2 ^ 16}"

let seed =
    int (System.DateTime.UtcNow.ToBinary())

let rand = System.Random(seed)
let randInt = rand.Next()
let randIntGen = fun () -> rand.Next()

open System

let doStuff a b =
    let c = a + b
    Console.WriteLine("{0} + {1} = {2}", a, b, c)
    let d = c * 2
    d

let adder a b = a + b

// Scoping!
let yearsOld =
    let age =
        let year = DateTime.Now.Year
        year - 1981

    $"You are about %d{age} years old!"

let sayHello someValue =
    let innerFn num =
        if num > 10 then "Isacc"
        elif num > 20 then "Fred"
        else "Sara"

    let resultOfInner =
        if someValue < 10.0 then
            innerFn 5
        else
            innerFn 15

    "Hello " + resultOfInner

let result = sayHello 10.5

let now = DateTime.UtcNow

let firstName = "Isaac"

let isFirstName = firstName = "Kate"

// Mutable style
// let mutable petrol = 100.0
//
// let drive distance =
//     if distance = "far" then petrol <- petrol / 2.
//     elif distance = "medium" then petrol <- petrol - 10.
//     else petrol <- petrol - 1.
//
// drive "far"
// drive "medium"
// drive "short"
//
// petrol

// Immutable style
let drive petrol distance =
    if distance = "far" then
        petrol / 2.
    elif distance = "medium" then
        petrol - 10.
    else
        petrol - 1.

let petrol = 100.
let firstState = drive petrol "far"
let secondState = drive firstState "medium"
let finalState = drive secondState "short"
finalState

let drive_ a b = drive b a

let inline debug msg x =
    printfn $"%s{msg}: %A{x}"
    x

let finalState2 =

    petrol
    |> drive_ "far"
    |> drive_ "medium"
    |> debug "before-last"
    |> drive_ "short"

let kettle = 0

let fill _kettle = 100

type Recipient =
    | Teapot
    | Cup

let pour recipient kettle =
    match recipient with
    | Teapot ->
        if kettle >= 50 then
            kettle - 50
        else
            kettle
    | Cup ->
        if kettle >= 10 then
            kettle - 10
        else
            kettle

(*
=== 2 cups + 1 teapot have been filled. Their remains 30dl of water.
kettle |> fill |> pour Cup |> pour Cup |> pour Teapot
val it: int = 30

=== Cup is empty
kettle |> pour Cup
val it: int = 0
*)

open System.Net.Http


let getAsync (url:string) = 
    let httpClient = new HttpClient()
    async {
        let! res = httpClient.GetAsync url |> Async.AwaitTask
        res.EnsureSuccessStatusCode () |> ignore  
        let! content = res.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
    }

let html = getAsync "http://sav.inklusive.fr" |> Async.RunSynchronously

open System
let describeAge age =
    let ageDescription =
        if age < 18 then "Child!"
        elif age < 65 then "Adult!"
        else "OAP!"
    let greeting = "Hello"
    Console.WriteLine("{0}! You are '{1}'.", greeting, ageDescription)
    
let a' = 1
let u = ()
let b' = describeAge 41
//let myUnit = ()

let writeTextToDisk text =
    let path = System.IO.Path.GetTempFileName()
    System.IO.File.WriteAllText(path, text)
    path
    
let createManyFiles =
    writeTextToDisk "hello world" |> ignore
    writeTextToDisk "hello world" |> ignore
    writeTextToDisk "hello world"
    
    