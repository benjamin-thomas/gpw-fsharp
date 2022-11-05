type User = { Name: string; Town: string }

let users =
    [
        { Name = "Isaac"; Town = "London" }
        { Name = "Sara"; Town = "Birmingham" }
        {
            Name = "Michelle"
            Town = "Manchester"
        }
    ]

let town user = user.Town

// List all the user's town
users |> List.map town

// ---- MAP
let nums = [ 1..10 ]
let double n = n * 2

// Imperative style
let out = ResizeArray()

for num in nums do
    out.Add(double num)

// Functional style
let outFunc = nums |> List.map double

// Parentheses are optional with tuples
[ "Isaac", 30; "John", 25 ] = [ ("Isaac", 30); ("John", 25) ]

[
    "Isaac", 30
    "John", 25
    "Sarah", 18
    "Faye", 27
] = [
    ("Isaac", 30)
    ("John", 25)
    ("Sarah", 18)
    ("Faye", 27)
]

// `iter` applies a function returning a unit (having side-effect)
users
|> List.iteri (fun i user -> printfn $"%d{i + 1}) Hello %s{user.Name}!")

// collect is a flatMap: the mapping function must return a list!
type Order = { OrderId: int }

type Customer =
    {
        CustomerId: int
        Orders: Order list
        Town: string
    }

let customers: Customer list = []

let orders: Order list =
    customers |> List.collect (fun c -> c.Orders)

// PAIRWISE
[ 1; 2; 3; 4 ] |> List.pairwise = [ (1, 2); (2, 3); (3, 4) ]
List.pairwise [] // runtime error!
List.pairwise ([]: int list) = [] // true!
List.pairwise [ 1 ] = [] // true
List.pairwise [ 1; 2 ] = [ (1, 2) ] // true
List.pairwise [ 1; 2; 3 ] = [ (1, 2); (2, 3) ] // true
// Pairwise can be useful to calculate the time difference between a list of dates.
open System

let dates =
    [
        DateTime(2022, 11, 3)
        DateTime(2022, 11, 1) // -2 days diff from nov 3rd
        DateTime(2022, 10, 25) // -7 days diff from nov 1st
    ]

dates
|> List.pairwise
|> List.map (fun (a, b) -> b - a)
|> List.map (fun time -> time.TotalDays) = [ -2.0; -7.0 ]

// ALL_PAIRS
List.allPairs [ 1..2 ] [ "A"; "B"; "C" ] = [
    (1, "A")
    (1, "B")
    (1, "C")
    (2, "A")
    (2, "B")
    (2, "C")
] // true

// WINDOWED resembles pairwise
[ 1; 2; 3; 4 ] |> List.windowed 2 = [ [ 1; 2 ]; [ 2; 3 ]; [ 3; 4 ] ]
[ 1; 2; 3; 4 ] |> List.windowed 3 = [ [ 1; 2; 3 ]; [ 2; 3; 4 ] ]
[ 1; 2; 3; 4 ] |> List.windowed 4 = [ [ 1; 2; 3; 4 ] ]
[ 1; 2; 3; 4 ] |> List.windowed 5 = [] // not enough elements

// List.sum [] // compile error, this is nice!
List.head [] // no compile error, use try variant
List.item 0 [] // no compile error, use try variant
List.take 99 [] // no compile error, no try variant!

// EXISTS
List.exists (fun item -> item = 2) [ 1..3 ]
List.exists (fun x -> x = 2) [ 1..3 ]
List.exists (fun x -> x = 2) [] // false, OK!

// EXISTS2
List.exists2 (fun x y -> x = 99 || y = 99) [ 1..3 ] [ 1..4 ] // runtime error, list length differ
List.exists2 (fun x y -> x = 99 || y = 99) [ 1..3 ] [ 4..6 ] // false
List.exists2 (fun x y -> x = 99 || y = 99) [ 1..3 ] [ 98..100 ] // true
List.exists2 (fun x y -> x = 99 || y = 99) [ 1..3 ] [] // runtime error, list length differ
List.exists2 (fun x y -> x = 99 || y = 99) [] [] // false, OK!
let hasSpecialNum x y = x = 99 || y = 99
([ 1..3 ], [ 4..6 ]) ||> List.exists2 hasSpecialNum // false
([ 1..3 ], [ 98..100 ]) ||> List.exists2 hasSpecialNum // true



// FORALL
List.forall (fun x -> x < 10) [ 1..9 ] // true
List.forall (fun x -> x < 10) [ 1..10 ] // false
List.forall (fun x -> x < 10) [] // true! (true is the default, that's nasty)
List.forall (fun x -> x > 10) [] // true! (true is the default, that's nasty)

// FORALL2
let isPositive x y = x > 0 && y > 0
List.forall2 isPositive [ 1..3 ] [ 2..4 ] // true
List.forall2 (fun x y -> x > 0 && y > 10) [ 1..3 ] [ 2..4 ] // false
([ 1..3 ], [ 2..4 ]) ||> List.forall2 isPositive // true

([ 1..3 ], [ 2..4 ])
||> List.forall2 (fun x y -> x > 0 && y > 10) // false

List.forall2 isPositive [] [ 1 ] // runtime error, list length differ
List.forall2 isPositive [] [] // true! (true is the default, that's nasty)
([1..3], [1..3]) ||> List.forall2 (=) // true
([1..3], [2..4]) ||> List.forall2 (=) // false
let eq1 x y = x = 1 && y = 1
([1;1;1], [1;1;1]) ||> List.forall2 eq1 // true
([1;1;1], [1;1;2]) ||> List.forall2 eq1 // false

// CONTAINS
[1..3] |> List.contains 99 // false
[1..100] |> List.contains 99 // true
List.contains 99 [] // false, OK!

// DISTINCT
List.distinct [1;1;2;3;2] = [1;2;3] // true
List.distinct [] // runtime error
List.distinct ([] : int list) = [] // true, OK!
