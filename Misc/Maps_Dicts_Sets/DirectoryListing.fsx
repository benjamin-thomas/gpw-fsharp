open System
open System.IO

(*
    dotnet fsi DirectoryListing.fsx
*)
let hiddenFile (di: DirectoryInfo) =
    di.Attributes.HasFlag FileAttributes.Hidden

let daysAgo ago =
    // +1 AND %4d due to space padding bug? https://github.com/dotnet/fsharp/issues/14247
    let pl = "999 days ago".Length + 1
    if ago = 0 then "TODAY".PadLeft(pl)
    else if ago = 1 then "YESTERDAY".PadLeft(pl)
    else $"% 4d{ago} days ago" // aligns nicely up to 999 days

let daysSince (fromUtc: DateTime) toUtc =
    let span: TimeSpan = fromUtc - toUtc
    span.Days

let now = DateTime.UtcNow

Directory.EnumerateDirectories "/home/benjamin/code/explore"
|> Seq.map DirectoryInfo
|> Seq.filter (not << hiddenFile)
|> Seq.map (fun di -> (di.Name, di.CreationTimeUtc))
|> Map.ofSeq
// I could apply the mapping from Seq.map but this is for demo...
// |> Map.map (fun _name timeUtc -> daysSince now timeUtc)
|> Map.map (fun _ -> daysSince now) // simplified from above.
|> Map.iter (fun name ago -> printfn $"%s{name.PadRight(20)}: created %s{daysAgo ago}")
