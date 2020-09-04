//Chapter 5 Immutability and Mutation

// Listing 5-4. Linear Search in mutable style
type Student = { Name : string; Grade : char }

let findFirstWithGrade (grade : char) (students : seq<Student>) =
    let mutable result = { Name = ""; Grade = ' ' }
    let mutable found = false
    
    for student in students do
        if not found && student.Grade = grade then
            result <- student
            found <- true
    
    result

//My approach
let findFirstWithGradeLFX grade students =
    students
    |> Seq.tryFind ( fun s -> s.Grade = grade )






// Note that the sequence expression is recursive because 
// it needs to include not only the result from “this” iteration, using yield,
// but also the results from every subsequent iteration, using yield!
// (note the exclamation mark after yield).

//Listing 5-14. Repeat Until in immutable style using a recursive sequence expression

let tryGetSomethingFromApi =
    let mutable thingCount = 0
    let maxThings = 10
    printfn ("thing: %d") thingCount
    fun () ->
        if thingCount < maxThings then
            thingCount <- thingCount+1
            "Soup"
        else
            null // No more soup for you!

let rec apiToSeq() =
    seq {
        match tryGetSomethingFromApi() |> Option.ofObj with
        | Some thing ->
            yield thing
            yield! apiToSeq()
        | None -> ()
    }
let listThingsFromApi() =
    apiToSeq()
    |> Seq.iter (printfn "I got %s")
    // I got Soup (x10)

listThingsFromApi()


// Listing 5-16. Implementing Seq.tryMax and Seq.tryMaxBy
module Seq =
    let tryMax a =
        if Seq.isEmpty a then None
        else Seq.max a

    let tryMaxBy f a =
        if Seq.isEmpty a then None
        else Seq.maxBy f a |> Some

    // Listing 5-17. Using Seq.tryMaxBy
    let tryGetLastStudentByName students =
        students
        |> tryMaxBy ( fun s -> s.Name )

// Listing 5-18. Using Seq.tryMaxBy to find furthest from zero
 // -5.3
let furthestFromZero =
    [| -1.1; -0.1; 0.; 1.1; -5.3 |]
    |> Seq.tryMaxBy abs


// Listing 5-19. Calculating RMS in mutable style
let rms (s : seq<float>) =
    let mutable total = 0.
    let mutable count = 0
    for item in s do
        total <- total + (item ** 2.)
        count <- count + 1

    let average = total / (float count)
    sqrt average

// 120.2081528
[|0.; -170.; 0.; 170.|]
|> rms


let inline sqrtBy a =
    a |> Seq.averageBy sqrt


// Listing 5-21. Cumulative computation in mutable style
let product (s : seq<float>) =
    let mutable total = 1.
    for item in s do
        total <- total * item
    total

[| 1.2; 1.1; 1.5|]
|> product


// Listing 5-22. MINE
[| 1.2; 1.1; 1.5|]
|> Seq.fold (*) 1.

// Listing 5-22. MINE. (using hints from the book with using an accumulator lambda)

let productLambda s =
    s
    |> Seq.fold ( fun acc elem -> acc * elem ) 1.
    // |> Seq.fold (*)  1. //This is the same, when the accumulator isn't mutating

[| 1.2; 1.1; 1.5|]
|> productLambda


