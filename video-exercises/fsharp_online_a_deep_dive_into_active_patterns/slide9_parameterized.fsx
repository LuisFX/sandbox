open System.Text.RegularExpressions
open System

let hexToDecimal hex =
    // System.Int32.Parse (hex, Globalization.NumberStyles.HexNumber)
    Convert.ToInt32(hex, 16)

let (|Grouped|_|) pattern value =
    let m = Regex.Match( value, pattern )
    if m.Success |> not || m.Groups.Count < 1 then
        None
    else
    [ for g in m.Groups -> g.Value ]
        |> List.tail //drop "root" match
        |> List.map hexToDecimal 
        |> Some


let hexColorToRgbColor hex =
    match hex with
    | Grouped "#(\d{2})(\d{2})(\d{2})" [ r; g; b ] ->
        printfn "RGB(%d, %d, %d)" r g b
    | otherwise ->
        printfn "'%s' is not a hex-color" otherwise



hexColorToRgbColor "#888888";