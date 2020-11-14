namespace Company.Function

open System
open System.Net
open System.Net.Http
open System.Text
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Host
open Microsoft.Extensions.Logging

open FSharp.Data

module TimerTrigger =

    let [<Literal>] CurrencySampleLiteral = "./currency_sample.json"
    type Currency = JsonProvider<CurrencySampleLiteral> //, RootName="MXN">

    let postUpdatedCurrencyToSlack() =
        let currencyUrl = "https://api.exchangeratesapi.io/latest?base=USD"
        let client = new WebClient()
        let result = client.DownloadString(currencyUrl)
        //printfn "%s" result

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



    [<FunctionName("TimerTrigger")>]
    let run([<TimerTrigger("0 */5 * * * *")>]myTimer: TimerInfo, log: ILogger) =
        let msg = sprintf "F# Time trigger function executed at: %A" DateTime.Now
        postUpdatedCurrencyToSlack()
        log.LogInformation msg
