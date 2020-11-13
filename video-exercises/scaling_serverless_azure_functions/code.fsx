// https://www.youtube.com/watch?v=iKi0dhA9-Go&t=623s
open System.Net.Http
open System.Text

#r "/Users/luisfx/.nuget/packages/fsharp.data/3.3.3/lib/netstandard2.0/FSharp.Data.dll"

open System.Net
open FSharp.Data

let currencyUrl = "https://api.exchangeratesapi.io/latest?base=USD"

let client = new WebClient()
let result = client.DownloadString(currencyUrl)
printfn "%s" result

let [<Literal>] CurrencySampleLiteral = __SOURCE_DIRECTORY__ + "/currency_sample.json"
type Currency = JsonProvider<CurrencySampleLiteral> //, RootName="MXN">

let currencySample = Currency.GetSample()
let req = "https://api.exchangeratesapi.io/latest?base=USD"

// let currencyAsync = Currency.AsyncLoad(req)
let currency = Currency.Load(req)


let mxn = currency.Rates.Mxn
let mxnString = sprintf @"{""text"":""Currency: 1 USD = %f MXN""}" mxn 



//SLACK WEBHOOK
//curl -X POST -H 'Content-type: application/json' --data '{"text":"Hello, World!"}' 
//https://hooks.slack.com/services/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

let slackUrl = "https://hooks.slack.com/services/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"
let slackMsg = new StringContent(mxnString, Encoding.UTF8)
let client2 = new HttpClient()
client2.PostAsync(slackUrl, slackMsg) |> ignore

