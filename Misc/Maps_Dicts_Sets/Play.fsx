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
