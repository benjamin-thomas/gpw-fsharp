(*
Create and modify an Array
*)
let nums = [| 1; 2; 3; 4 |]
let first = nums[0]
let first3 = nums[0..2]

(*
Mutate the array. Now `nums` equals [|99; 2; 3; 4; 6|].
However, `first` and `first3` haven't changed.
So it looks like accessing an array index copies the value, rather than give a ref to the underlying value.
*)
nums[0] <- 99

(*
    {
        Arr    = [|99; 2; 3; 4|]
        First  = 1
        First3 = [|1; 2; 3|]
    }
*)
struct {|
           Arr = nums
           First = first
           First3 = first3
       |}

(*
Create and change a List (by returning a new List)
*)
let nums2a = [ 1; 2; 3; 4 ]
let nums2b = [ 1..4 ]
nums2a = nums2b // true

match nums2a with
| [] -> printfn "The list is empty!"
| h :: _t -> printfn $"First item is: %d{h}"

let nums3 = 0 :: nums2a // [0; 1; 2; 3; 4]

// @ concatenates two lists
let nums4 = nums3 @ [ 5..7 ] // [0; 1; 2; 3; 4; 5; 6; 7]
