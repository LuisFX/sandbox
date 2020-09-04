 type BillingDetails = 
    {
        Name : string
        Billing : string
        Delivery : string option }

let myOrder =
    {
        Name = "Kit Eason"
        Billing = "112 Fibonacci Street\nErehwon\n35813"
        Delivery = None }

let hisOrder = 
    {
        Name = "John Doe"
        Billing = "314 Pi Avenue\nErewhon\n15926"
        Delivery = None }

let herOrder =
    {
        Name = "Jane Smith"
        Billing = null
        Delivery = None }

let orders = [| myOrder; hisOrder; herOrder |]

let countNonNullBilling (orders: seq<BillingDetails>) =

    // NEEDS REFACTOR
    // orders
    // |> Seq.map (fun a -> a.Billing)
    // |> Seq.map Option.ofObj
    // |> Seq.sumBy (fun a -> match a with Some _ -> 0 | None -> 1)

    orders |> (Seq.map((fun a -> a.Billing) >> Option.ofObj))
    |> Seq.sumBy( fun a -> match a with Some _ -> 0 | None -> 1 )



