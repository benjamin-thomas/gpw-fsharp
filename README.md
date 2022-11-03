# Reading Get Programming with F#

To create a sub-project, run from the root:

```bash
dotnet new console -lang "F#" -o ProjName
```

## Custom Rider setup

```
Send line or lines to REPL with:
    <F4>
    <C-à>

Switch to the REPL window, then back to the code window with:

    <Alt-Num4>, <Esc>

Clear the REPL window with:

    <Alt-Shift-Num4>
```

I also chose to format brackets + curly brackets on the same line. It makes shifting items around easier + removes annoying git diff noise.

```
File > Settings > Editor > F# > Formatting Style > Align opening and closing braces...
```
