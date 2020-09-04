// open System

// type BillingDetails = {
//     Name : string
//     Billing : string
//     Delivery : string option
// }

// let myOrder = {
//     Name = "Kit Eason"
//     Billing = "112 Fibonacci Street\nErehwon\n35813"
//     Delivery = None 
// }

// let hisOrder = {
//     Name = "John Doe"
//     Billing = "314 Pi Avenue\nErewhon\n15926"
//     Delivery = Some "16 Planck Parkway\nErewhon\n62291"
// }

// //Listing 3-11. Using Option.map to optionally apply a function, returning an option type

// let printDeliveryAddress (details : BillingDetails) =
//     details.Delivery
//     |> Option.map (fun address -> address.ToUpper())
//     |> Option.iter (fun address -> printfn "Delivery address:\n%s\n%s" (details.Name.ToUpper()) address)
    
// // No output at all
// myOrder |> printDeliveryAddress
// hisOrder |> printDeliveryAddress

// let tryLastLine (address : string) =
//     // let parts = address.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
//     // Could also just do parts |> Array.tryLast

//     // match parts with
//     // | [||] -> None
//     // | parts -> parts |> Array.last |> Some

//     address.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
//     |> Array.tryLast



// let tryPostalCode (codeString : string) =
//     match Int32.TryParse(codeString) with
//     | true, i -> i |> Some
//     | false, _ -> None

// let postalCodeHub (code : int) =
//     if code = 62291 
//     then "Hub 1"
//     else "Hub 2"

// let tryHub (details : BillingDetails) =
//     details.Delivery
//     |> Option.bind tryLastLine
//     |> Option.bind tryPostalCode
//     |> Option.map postalCodeHub
//     // |> Option.iter (fun c -> printfn "Hub: %s" c)


// // None
// myOrder |> tryHub
// // Some "Hub 1"
// hisOrder |> tryHub

// let x = None

// x |> Option.defaultValue "text"
// let y = 
//     Decimal.Parse("10T")


// type PathA = PathA of string
// type FnameA = FnameA of string
// let combinePath (PathA(p)) (FnameA(file)) = System.IO.Path.Combine(p, file)

// combinePath (PathA(@"C:\temp")) (FnameA("temp.txt"))


    
// type SafeString<'a> = { Value : string }

// type PathB = class end
// type FileNameB = class end

// let combinePath (path:SafeString<PathB>) (filename:SafeString<FileNameB>) = 
//     System.IO.Path.Combine(path.Value, filename.Value)

// let myPath : SafeString<PathB> = { Value = "C:\\SomeDir\\" }
// let myFile : SafeString<FileNameB> = { Value = "MyDocument.txt" }

// // works
// let combined = combinePath myPath myFile

// // compile-time failure
// let combined' = combinePath myFile myPath

// let chooser a = a |> Option.map id

// let a = Some "a"
// let b = None
// let z = Some "z"
// let c = [a;b;z]

// let d = c |> List.choose chooser

// //Listing 3-19. Using Option.ofObj
// let myApiFunction (stringParam : string) =
//     let s = 
//         stringParam
//         |> Option.ofObj
//         |> Option.defaultValue "(none)"
//     // You can do things here knowing that s isn't null
 
//     printfn "%s" (s.ToUpper())

// //  // HELLO
// myApiFunction "hello"
// //  // (NONE)
// myApiFunction null


// // Listing 3-20. Using Option.ofNullable
// open System
// let showHeartRate (rate : Nullable<int>) =
//     rate
//     |> Option.ofNullable
//     |> Option.map (fun r -> r.ToString())
//     |> Option.defaultValue "N/A"
//     // 96

// let xx = showHeartRate (Nullable(96))
//  // N/A
// let yy = showHeartRate (Nullable())


// open System
// let random = new Random()
// let tryLocationDescription (locationId : int, description : string byref) : bool =
//     // In reality this would be attempting
//     // to get the description from a database etc.
//     let r = random.Next(1, 100)
//     let ret =
//         if r < 50 then
//             description <- sprintf "Location number %i" r
//             true
//         else
//             description <- null
//             false
//     ret

type Delivery =
    | AsBilling
    | Physical of string
    | Download
    | ClickAndCollect of int

type BillingDetails = {
    Name : string
    Billing : string
    Delivery : Delivery }

// Listing 3-16. Consuming the improved BillingDetails type
let tryDeliveryLabel (billingDetails : BillingDetails) =
    match billingDetails.Delivery with
    | AsBilling -> billingDetails.Billing |> Some
    | Physical address -> address |> Some
    | Download
    | ClickAndCollect _ -> None |> Option.map (fun address -> sprintf "%s\n%s" billingDetails.Name address)

let deliveryLabels (billingDetails : BillingDetails seq) =
    billingDetails
    |> Seq.choose tryDeliveryLabel

let myOrder = {
    Name = "Kit Eason"
    Billing = "112 Fibonacci Street\nErehwon\n35813"
    Delivery = ClickAndCollect 1 
}


let collectionsFor id aseq =
    aseq
    |> 