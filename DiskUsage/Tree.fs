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
        ParentDir: string
    }

let private addTrailingSepChar (str: string) : string =
    let sepChar = System.IO.Path.DirectorySeparatorChar.ToString()
    if str.EndsWith sepChar then
        str
    else
        str + sepChar

let rec toSummary path =
    let fi = System.IO.FileInfo path

    let isDir =
        fi.Attributes.HasFlag(System.IO.FileAttributes.Directory)

    let isHidden =
        fi.Attributes.HasFlag(System.IO.FileAttributes.Hidden)

    // Return 0 or get a runtime error (data does not exist).
    let size =
        if isDir then int64 0 else fi.Length

    let children =
        if isDir then
            (enumEntries path |> Seq.collect toSummary)
        else
            []

    [
        {
            Name = path
            IsDir = isDir
            IsHidden = isHidden
            Size = size
            Children = children
            ParentDir = fi.DirectoryName |> addTrailingSepChar
        }
    ]

let rec treeToString (depth: int) (summary: Summary seq) : (int * int64 * string * bool * string) seq =
    summary
    |> Seq.filter (fun s -> not s.IsHidden) // mimic os util: "tree"
    |> Seq.collect (fun summary ->
        let children: Summary seq = summary.Children

        // FIXME: bogus -1 value here. I need to somehow accumulate summary.Size from the children.
        if children <> [] then
            [
                (depth, int64 -1, summary.Name, summary.IsDir, summary.ParentDir)
            ]
            |> Seq.append (treeToString (depth + 1) children |> Seq.map id)

        else
            [
                (depth, summary.Size, summary.Name, summary.IsDir, summary.ParentDir)
            ])

let hiddenFile summary = summary.IsHidden

let toLowerName summary = summary.Name.ToLower()

let compute path =
    enumEntries path |> Seq.collect toSummary


let private toHuman size =
    ByteSizeLib
        .ByteSize
        .FromBytes(float size)
        .ToString()


let private printPath (depth, size: int64, path: string, isDir, parentDir) =
    let depthToWS = String.replicate depth "  "

    let simplePath = path.Replace(parentDir, "")
    let fileTypeEmoji = if isDir then "📁" else "📝"
    let depth_ = $"%d{depth}".PadLeft(3)
    printfn $"[%s{(toHuman size).PadLeft(9)}] %s{fileTypeEmoji} [%s{depth_}] %s{depthToWS} %s{simplePath}"

let print rootPath =
    compute (rootPath |> addTrailingSepChar)
    |> treeToString 0
    |> Seq.sortBy (fun (depth, _size, path, _isDir, _parentDir) -> [ path.ToLower(), depth ]) // sort lower to mimic os util "tree"
    |> Seq.iter printPath

// print "/home/benjamin/code/explore/love2d/love-typescript-template/"
