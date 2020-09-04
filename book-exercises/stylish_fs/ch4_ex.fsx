module Houses =
    type House = { Address : string; Price : decimal }

    type PriceBand = Cheap | Medium | Expensive
    
    /// Make an array of 'count' random houses.
    let getHouses count =
        let random = System.Random(Seed = 1)
        Array.init count (fun i ->
            {
                Address = sprintf "%i Stochastic Street" (i+1)
                Price = random.Next(50_000, 500_000) |> decimal 
            }
        )

    let random = System.Random(Seed = 1)
 
    /// Try to get the distance to the nearest school.
    /// (Results are simulated)
    let trySchoolDistance (house : House) =
        // Because we simulate results, the house parameter isn’t actually used.
        let dist = random.Next(10) |> double
        if dist < 8.
        then Some dist
        else None
    
    // Return a price band based on price.
    let priceBand (price : decimal) =
        if price < 100_000m then
            Cheap
        else if price < 200_000m then
            Medium
        else 
            Expensive

module Exercise_04_01 =
    open Houses
    
    let houses = Houses.getHouses 20
    let res =
        houses
        |> Seq.map(fun a ->
            sprintf "Address: %s - Price: %f" a.Address a.Price

        )

module Exercise_04_02 =
    open Houses
    let houses = Houses.getHouses 20
    let res =
        houses
        |> Seq.averageBy (fun h -> h.Price)

module Exercise_04_03 =
    open Houses
    let houses =
        Houses.getHouses 20
        |> Seq.filter(fun h -> h.Price >= 250_000m)

module Exercise_04_04 =
    open Houses
    let houses =
        Houses.getHouses 20
        |> Seq.choose(fun h ->
                match (trySchoolDistance h) with
                | Some d -> Some(d, h)
                | None -> None
            )

module Exercise_04_06 =
    open Houses
    Houses.getHouses 20
    |> Seq.filter (fun h -> h.Price > 200_000m)
    |> Seq.sortByDescending(fun h -> h.Price)
    |> Seq.iter(fun h ->
        printfn "Address: %s - Price: %f" h.Address h.Price
    )

module Exercise_04_07 =
    open Houses
    Houses.getHouses 20
    |> Seq.filter ( fun h -> h.Price > 200_000m)
    |> Seq.averageBy (fun h -> h.Price)

module Exercise_04_08 =
    let houses =
        Houses.getHouses 20
        |> Seq.choose( fun h ->
            match (Houses.trySchoolDistance h) with
            | Some d -> Some(h, d)
            | None -> None
        )
        |> Seq.find (fun (h,d) -> h.Price < 100_000m)

// Take a sample of 20 houses, and create an array of tuples, where the first element of each
// tuple is a price band (created using the provided priceBand function), and the second is a
// sequence of all the houses that fall into the band.
// It’s OK if a band is omitted when there are no houses in that band. Within a grouping, the
// houses should be in ascending order of price
module Exercise_04_09 =
    open Houses
    let houses =
        getHouses 20
        |> Seq.groupBy ( fun h -> priceBand h.Price  )
        |> Seq.map ( fun (b, h) ->
            b, h |> Seq.sortBy ( fun h -> h.Price)
        )

module Average =
    // Listing 4-11. A generic function to compute an array average, or zero when the array is empty
    let inline averageOrZero a =
        if Array.isEmpty a then
            LanguagePrimitives.GenericZero
        else
            a |> Array.average
    
    // Listing 4-12. A function to compute an array average, or a default when the array is empty
    let inline averageOr dflt a =
        if Array.isEmpty a then
            dflt
        else
            a |> Array.average


    
    // 370.m
    let ex3 = [|10.m; 100.m; 1000.m|] |> averageOrZero
    // 370.f
    let ex3f = [|10.f; 100.f; 1000.f|] |> averageOrZero
    // 0.m
    let ex4:decimal = [||] |> averageOrZero
    // 0.f
    let ex4f:float32 = [||] |> averageOrZero
    // let ex4f:float32 = [||] |> averageOrZero<float32>


    // 370.m
    let ex5 = [|10.m; 100.m; 1000.m|] |> averageOr 0.m
    // 370.f
    let ex5f = [|10.f; 100.f; 1000.f|] |> averageOr 0.f
    // 0.m
    let ex6 = [||] |> averageOr 0.m
    // 0.f
    let ex6f = [||] |> averageOr 0.f

    
// Take a sample of 20 houses and find the average price of all the houses that cost over $200,000.
// You’ll need to make sure you handle the case where no houses in the sample cost over $200,000. 
// (You will need to change the price criterion a little to test this.)
// You should be able to complete this exercise using two collection functions, but you may need
// to define one of these functions yourself.

module Array =
    let inline tryAverageBy f a =
        if a |> Array.isEmpty then None
        else a |> Array.averageBy f |> Some

module Exercise_04_10 =
    let houseAverage price =
        Houses.getHouses 20
        |> Array.filter ( fun h -> h.Price > price )
        |> Array.tryAverageBy ( fun h -> h.Price )

let res =
    match Exercise_04_10.houseAverage 500_000m with
    | Some a -> sprintf "Average: %f" a
    | None -> sprintf "Query returned no results!"

// Take a sample of 20 houses and find the first house that costs less than $100,000 and for which we can calculate the distance to a school. 
// The results should include the house instance and the calculated distance to school.
// You’ll need to make sure you handle the case where no houses meet the criteria. (You will need to change the price criterion a little to test this.)
// You should be able to complete this exercise using two collection functions.

module Exercise04_11 =

    let hasSchool h =
        match Houses.trySchoolDistance h with
        | Some d -> true
        | None -> false


    let cheapHouseWithKnownSchoolDistance =
        Houses.getHouses 20
        |> Array.filter ( fun h -> h.Price > 1000_000m && hasSchool h )
        |> Array.tryHead

module BOOK_Exercise04_11 =
    open Houses

    let cheapHouseWithKnownSchoolDistance =
        getHouses 20
        |> Array.filter (fun h -> h.Price < 100_000m)
        |> Array.tryPick (fun h ->
            match h |> trySchoolDistance with
            | Some d -> Some(h, d)
            | None -> None
        )

let hh =
    BOOK_Exercise04_11.cheapHouseWithKnownSchoolDistance       