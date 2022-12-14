let sum elems =
    let mutable acc = 0

    for elem in elems do
        acc <- acc + elem

    acc

sum [ 1; 2; 3 ]

let length elems =
    let mutable total = 0

    for _ in elems do
        total <- total + 1

    total


length [ 1; 2; 3 ]

let max elems =
    let mutable mx = List.head elems

    for elem in elems do
        if elem > mx then mx <- elem

    mx

max [ 1; 4; 3 ]

let sum' lst =
    Seq.fold (fun acc num -> acc + num) 0 lst

sum' [ 1; 2; 3; 4 ]

let length' lst = List.fold (fun acc _ -> acc + 1) 0 lst

let lengthStrict lst =
    match lst with
    | [] -> failwith "Empty list!"
    | h :: t -> List.fold (fun acc _ -> acc + 1) h t

lengthStrict [ 1; 2; 3 ]
// lengthStrict [] // throws!

let max' =
    function
    | [] -> failwith "oops"
    | h :: t -> List.fold (fun mx n -> if n > mx then n else mx) h t

let maxNonEmpty h t =
    List.fold (fun mx n -> if n > mx then n else mx) h t

// max' [] // throws!

maxNonEmpty 1 [ 8; 3 ]
maxNonEmpty 9 [ 5; 3 ]

// Other, more "readable" versions
let sum2 lst =
    lst |> List.fold (fun acc n -> acc + n) 0

sum2 [ 1; 2 ]
sum2 []

let sum3 lst =
    (0, lst) ||> List.fold (fun acc n -> acc + n)

sum3 [ 1; 2 ]
sum3 []

// The function could have been shortened like this anyways
let sum4 =
    List.fold (fun acc n -> acc + n) 0

sum4 [ 1..3 ]

// Or
let sum5 =
    0 |> List.fold (fun acc n -> acc + n)

sum5 [ 1..3 ]

// With a non empty variation

let sumNonEmpty h t = List.fold (fun acc n -> acc + n) h t

sumNonEmpty 1 [ 2; 3 ]

// Shortened to

let sumNonEmpty' h = h |> List.fold (fun acc n -> acc + n)

sumNonEmpty' 8 [ 2; 3 ]

// Reduce throws on non empty (good!)
List.reduce (+) [ 1; 2; 3 ]
List.reduce (fun acc n -> acc + n) [ 1; 2; 3 ]
// List.reduce (+) [] // throws!

List.fold (fun acc n -> acc + n) 1 [ 2; 3 ] // 6
List.foldBack (fun acc n -> acc + n) [ 1; 2 ] 3 // 6

List.scan (fun acc n -> acc + n) 0 [ 1; 2; 3; 4 ] // [0; 1; 3; 6; 10]

1
|> List.unfold (fun state ->
    if state > 100 then
        None
    else
        Some(state, state * 2)) // [1; 2; 4; 8; 16; 32; 64]

(*
    Accumulate through a `while` loop
*)

let countCharsImperative () =
    let mutable totalChars = 0

    let path =
        "/home/benjamin/code/github.com/benjamin-thomas/gpw-fsharp/Lesson18/Fold.fsx"

    let sr =
        new System.IO.StreamReader(System.IO.File.OpenRead path)

    while (not sr.EndOfStream) do
        let line = sr.ReadLine()
        totalChars <- totalChars + line.ToCharArray().Length

    totalChars

countCharsImperative ()

(*
    Accumulate with a `while` loop, by simulating input via `yield`.
    `seq` and `yield` go hand in hand
*)
let countCharsSemiFunctional () =
    let path =
        "/home/benjamin/code/github.com/benjamin-thomas/gpw-fsharp/Lesson18/Fold.fsx"

    let lines =
        seq {
            use sr =
                new System.IO.StreamReader(System.IO.File.OpenRead path)

            while (not sr.EndOfStream) do
                yield sr.ReadLine()
        }

    (0, lines)
    ||> Seq.fold (fun total line -> total + line.Length)

// Generate a list of rules

// Rule is a "type alias", erased at runtime.
type Rule = string -> bool * string

let rules: Rule list =
    [
        fun text -> (text.Split ' ').Length = 3, "Must be three words"
        fun text -> text.Length <= 30, "Max length is 30 characters"
        fun text ->
            text
            |> Seq.filter System.Char.IsLetter
            |> Seq.forall System.Char.IsUpper,
            "All letters must be caps"
    ]

// Manually build a "super" rule
let validateManual (rules: Rule list) word =
    let passed, error = rules[0]word

    if not passed then
        false, error
    else
        let passed, error = rules[1]word

        if not passed then
            false, error
        else
            let passed, error = rules[2]word

            if not passed then
                false, error
            else
                true, ""


// Build a "super" rule with `reduce`

let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
        fun word ->
            let passed, error = firstRule word
            if passed then
                let passed, error = secondRule word
                if passed then true, "" else false, error
            else false, error)
    
let validate = buildValidator rules
validate "THIS WILL PASS"