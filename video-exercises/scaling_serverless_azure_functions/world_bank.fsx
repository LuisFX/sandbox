#r "/Users/luisfx/.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"

open FSharp.Data

let [<Literal>] WorldBankLiteral = __SOURCE_DIRECTORY__ + "/WorldBank.json"
type WorldBank = JsonProvider<WorldBankLiteral>
let doc = WorldBank.GetSample()

let wbReq = "http://api.worldbank.org/country/cz/indicator/GC.DOD.TOTL.GD.ZS?format=json"

let docAsync = WorldBank.Load(wbReq)

// Print general information
let info = docAsync.Record
printfn "Showing page %d of %d. Total records %d" 
  info.Page info.Pages info.Total

// Print all data points
for record in doc.Array do
  record.Value |> Option.iter (fun value ->
    printfn "%d: %f" record.Date value)