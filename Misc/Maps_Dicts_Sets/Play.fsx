open System.Collections.Generic

// Mutable dict, like C#
let inventory = Dictionary<string, int>()
inventory.Add("Apples", 1)
inventory.Add("Oranges", 2)
inventory.Add("Bananas", 3)
inventory.Remove("Oranges") // true if success, otherwise false (key not present)
inventory.["Apples"] // raises an exception if not found (keep dot syntax or formatter removes comment!)
inventory.TryGetValue("Bananas") // (true, 3)
inventory.TryGetValue("Bogus") // (false, 0)
inventory.Clear() // remove all

let status = Dictionary<_, _>() // <int, string> is inferred by the next line
status.Add(404, "Not Found!")
status.Add(500, "Server Error")

let prices = Dictionary() // <string, float> is inferred by the next line, no need for type holes.
prices.Add("Apples", 0.98)

// Immutable dict
let inventory2 =
    [
        ("Apples", 1)
        ("Oranges", 2)
        ("Bananas", 3)
    ]
    |> dict

inventory2.["Apples"] // raises an exception if not found (keep dot syntax or formatter removes comment!)

(* raises NotSupportedException, because immutable

inventory2.Add("Pear", 4)
inventory2.Remove("Apples")
inventory2.Clear

*)

// Create a mutable dict from an immutable dict
let inventory3 = inventory2 |> Dictionary

// Maps
let inventory4 =
    [
        ("Apples", 1)
        ("Oranges", 2)
        ("Bananas", 3)
    ]
    |> Map.ofList

inventory4.["Apples"] // raises an exception if not found (keep dot syntax or formatter removes comment!)

let inventory5 =
    inventory4
    |> Map.remove "Apples"
    |> Map.add "Pears" 4

// Sets
let myBasket =
    [
        "Apple"
        "Apple"
        "Orange"
        "Banana"
        "Pineapple"
    ]

let fruitsILike =
    // No dups
    myBasket |> Set.ofList // set ["Apple"; "Banana"; "Orange"; "Pineapple"]

let yourBasket =
    [ "Kiwi"; "Banana"; "Grapes" ]

let fruitsYouLike =
    // No dups
    yourBasket |> Set.ofList // set ["Banana"; "Grapes"; "Kiwi"]

let allFruitsList =
    // ["Apple"; "Orange"; "Banana"; "Pineapple"; "Kiwi"; "Grapes"]
    (myBasket @ yourBasket) |> List.distinct

let allFruits = fruitsILike + fruitsYouLike // set ["Apple"; "Banana"; "Grapes"; "Kiwi"; "Orange"; "Pineapple"]

let allFruits2 =
    Set.union fruitsILike fruitsYouLike // set ["Apple"; "Banana"; "Grapes"; "Kiwi"; "Orange"; "Pineapple"]

let fruitsOnlyLikedByMe =
    allFruits - fruitsYouLike // set ["Apple"; "Orange"; "Pineapple"]

let fruitsOnlyLikedByMe2 =
    Set.difference allFruits fruitsYouLike // set ["Apple"; "Orange"; "Pineapple"]

let fruitsOnlyLikedByYou =
    allFruits - fruitsILike // set ["Grapes"; "Kiwi"]

let fruitsOnlyLikedByYou2 =
    Set.difference allFruits fruitsILike // set ["Grapes"; "Kiwi"]

let fruitsWeBothLike =
    Set.intersect fruitsILike fruitsYouLike // set ["Banana"]

Set.isSubset (set [ 1; 2; 3 ]) (set [ 1; 2; 3; 4 ]) // true
Set.isSubset (set [ 1; 2; 3; 4 ]) (set [ 1; 2; 3 ]) // false
set [ 1..100 ] |> Set.isSubset (set [ 1..3 ]) // true

set [ 1..100 ] |> Set.isSubset (set [ 4; 8; 16 ]) // true
set [ 1..100 ] |> Set.isSubset (set [ 99..100 ]) // true
set [ 1..100 ] |> Set.isSubset (set [ 99..101 ]) // false
