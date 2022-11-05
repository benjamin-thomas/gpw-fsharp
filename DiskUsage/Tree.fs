module Tree

open System

let enumEntries path =
    System.IO.Directory.EnumerateFileSystemEntries path


type Summary =
    {
        Name: string
        IsDir: bool
        Children: Summary seq
    }

let rec toSummary start path =
    let fi = System.IO.FileInfo path

    let isDir =
        fi.Attributes = IO.FileAttributes.Directory

    let getChildren =
        if isDir then
            (enumEntries path |> Seq.map (toSummary start))
        else
            []

    {
        Name = path.Replace(start, "")
        IsDir = isDir
        Children = getChildren
    }


let rec treeToString (summary: Summary seq) : string seq =
    summary
    |> Seq.collect (fun summary ->
        let children: Summary seq = summary.Children

        // I need to groupBy the parent name
        // FIXME: empty directories are not shown
        if children <> [] then
            treeToString children
            |> Seq.map (fun path -> $"  %s{path}")

        else
            [ summary.Name ])

let hiddenFile summary = summary.Name.StartsWith "."

let toLowerName summary = summary.Name.ToLower()

let compute start =
    enumEntries start
    |> Seq.map (toSummary start)
    |> Seq.filter (not << hiddenFile) // mimic os util "tree"
    |> Seq.sortBy toLowerName // mimic os util "tree"



let private addTrailingSlash (str: string) : string =
    if str.EndsWith "/" then
        str
    else
        str + "/"

let print start =
    compute (start |> addTrailingSlash)
    |> treeToString
    |> Seq.iter Console.WriteLine

// printTree "/home/benjamin/code/explore/love2d/love-typescript-template/" ()
