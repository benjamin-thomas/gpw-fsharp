module Tree

let enumEntries path =
    System.IO.Directory.EnumerateFileSystemEntries path


type Summary =
    {
        Name: string
        IsDir: bool
        IsHidden: bool
        Size: int64
        Children: Summary seq
    }

let rec toSummary start path =
    let fi = System.IO.FileInfo path

    let isDir =
        fi.Attributes.HasFlag(System.IO.FileAttributes.Directory)

    let isHidden =
        fi.Attributes.HasFlag(System.IO.FileAttributes.Hidden)

    // Return 0 or get a runtime error (data does not exist).
    let size =
        if isDir then int64 0 else fi.Length

    let getChildren =
        if isDir then
            (enumEntries path |> Seq.map (toSummary start))
        else
            []

    {
        Name = path.Replace(start, "")
        IsDir = isDir
        IsHidden = isHidden
        Size = size
        Children = getChildren
    }


let rec treeToString (depth: int) (summary: Summary seq) : (int * int64 * string) seq =
    summary
    |> Seq.collect (fun summary ->
        let children: Summary seq = summary.Children

        // I need to groupBy the parent name
        // FIXME: empty directories are not shown
        if children <> [] then
            treeToString (depth + 1) children |> Seq.map id

        else
            [ (depth, summary.Size, summary.Name) ])

let hiddenFile summary = summary.IsHidden

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

let private toHuman size =
    let sizeToHuman =
        ByteSizeLib
            .ByteSize
            .FromBytes(float size)
            .ToString()

    $"[%s{sizeToHuman}]".PadRight(10)

let print start =
    compute (start |> addTrailingSlash)
    |> treeToString 0
    |> Seq.iter (fun (depth, size, path) ->
        let depthToWS = String.replicate depth "  "

        printfn $"%s{depthToWS} %s{toHuman size} %s{path}")

// print "/home/benjamin/code/explore/love2d/love-typescript-template/"
