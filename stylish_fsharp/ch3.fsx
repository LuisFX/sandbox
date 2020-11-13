open System

//Listing 3-4
type Shape<'T> =
    | Square of height:'T
    | Rectangle of height:'T * width:'T
    | Circle of radius:'T

let describe (shape: Shape<float>) =
    match shape with
    | Square h -> sprintf "Square of height %f" h
    | Rectangle (h, w) -> sprintf "Rectangle of height %f and width %f" h w
    | Circle r -> sprintf "Circle of radius %f" r

let goldenRect = Rectangle(1.0, 1.61803)

printfn "%s" (describe goldenRect)

//Listing 3-5
let myMiddleName = Some "Luis"
let herMiddleName = None

let displayMiddleName (name: string option) =
    match name with
    | Some n -> n
    | None -> ""

printfn ">>>%s<<<" (displayMiddleName myMiddleName)

printfn ">>>%s<<<" (displayMiddleName herMiddleName)

//Listing 3-6
type BillingDetails = {
    Name: string
    Billing: string
    Delivery: string option
}

let myOrder = {
    Name = "Luis"
    Billing = "123 Fake St"
    Delivery = None
}

let hisOrder = {
    Name = "Kit Easton"
    Billing = "314 Pie Ave"
    Delivery = Some "16 Planck Parkway"
}

 // Error: the expression was expected to have type 'string'
 // but here has type 'string option'
 //// printfn "%s" myOrder.Delivery
 //// printfn "%s" hisOrder.Delivery


 //Listing 3-7
 let addressForPackage (details: BillingDetails) =
    let address =
        match details.Delivery with
        | Some s -> s
        | None -> details.Billing
    
    sprintf "%s\n%s" details.Name address

printfn "%s" (addressForPackage myOrder)
printfn "%s" (addressForPackage hisOrder)

//Listing 3-8. Defaulting an Option Type Instance using Option.defaultValue

let addressForPackageDefault (details: BillingDetails) =
    let address =
        Option.defaultValue details.Billing details.Delivery
    
    sprintf "%s\n%s" details.Name address


printfn "%s" (addressForPackageDefault myOrder)
printfn "%s" (addressForPackageDefault hisOrder)

//Listing 3-9. Using Option.defaultValue in a pipeline
let addressForPackagePipeline (details: BillingDetails) =
    let address =
        details.Delivery
        |> Option.defaultValue details.Billing

    sprintf "%s\n%s" details.Name address

printfn "%s" (addressForPackagePipeline myOrder)
printfn "%s" (addressForPackagePipeline hisOrder)

//Listing 3-10. Using Option.iter to take an imperative action if a value is populated
let printDeliveryAddress (details: BillingDetails) =
    details.Delivery
    |> Option.iter( fun address -> printfn "%s\n%s" details.Name address )

myOrder |> printDeliveryAddress
hisOrder |> printDeliveryAddress

//Listing 3-11. Using Option.map to optionally apply a function, returning an option type.
let printDeliveryAddressOption (details: BillingDetails ) =
    details.Delivery
    |> Option.map ( fun address -> address.ToUpper() )
    |> Option.iter ( fun address -> printfn "Delivery address: \n%s\n%s" (details.Name.ToUpper()) address )

myOrder |> printDeliveryAddressOption
hisOrder |> printDeliveryAddressOption


//Listing 3-12. Using Option.bind to create a pipeline of might-fail operations.
let tryLastLine (address: string) =
    let parts = address.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
    // Could also just do parts |> Array.tryLast

    match parts with
    | [||] -> None
    | parts -> parts |> Array.last |> Some

let tryPostalCode (codeString: string) =
    let tryIntTest = Int32.TryParse("")
    match Int32.TryParse(codeString) with
    | true, i -> i |> Some
    | false, _ -> None

let postalCodeHub (code : int) =
    if code = 62291
    then "Hub 1"
    else "Hub 2"

let tryHub (details: BillingDetails) =
    details.Delivery
    |> Option.bind tryLastLine
    |> Option.bind tryPostalCode
    |> Option.map postalCodeHub

let myO = myOrder |> tryHub |> ignore
hisOrder |> tryHub

//Batch size for firebear 100

