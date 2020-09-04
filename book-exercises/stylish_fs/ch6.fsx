//  PATTERN MATCHING

(*
    Recover a matched value
    Well it can recover the value that actually matched at runtime, if you follow the final case
    with as x (Listing 6-4).
*)

// Listing 6-4. Recovering a matched value
let caseSwitch3 = 3

// "Maybe 3, maybe 4. But actually 3."
match caseSwitch3 with
| 1 -> printfn "Case 1"
| 2 -> printfn "Case 2"

| 3
| 4 as x -> printfn "Maybe 3, maybe 4. But actually %i." x
| _ -> printfn "Default case"


(*
    When Guards
    If you want a bit more branching, using the value recovered in a match case, you can use
    a when guard. A when guard is a bit like an if expression, and it uses the recovered value
    for some comparison. Only if the comparison returns true is the following code executed (Listing 6-5).
*)

// Listing 6-5. Matching with a when guard
let caseSwitch4 = 11

// "Less than a dozen"
match caseSwitch4 with
| 1 -> printfn "One"
| 2 -> printfn "A couple"
| x when x < 1 -> printfn "Nothing"
| x when x < 12 -> printfn "Less than a dozen"
| x when x = 12 -> printfn "A dozen"
| _ -> printfn "More than a dozen"


(*
    Pattern Matching on Arrays and Lists
    What if the value being matched is a bit more structured – say an array? We can pattern
    match on arrays and pick out cases having specific element counts (Listing 6-6).
*)

// Listing 6-6. Pattern matching on arrays
let arr0 = [||]
let arr1 = [|"One fish" |]
let arr2 = [|"One fish"; "Two fish" |]
let arr3 = [|"One fish"; "Two fish"; "Red fish"|]
let arr4 = [|"One fish"; "Two fish"; "Red fish"; "Blue fish" |]

let arr = arr1

// "A pond containing one fish: One fish"
match arr with
| [||] -> "An empty pond"
| [| fish |] -> sprintf "A pond containing one fish: %s" fish
| [| f1; f2 |] -> sprintf "A pond containing two fish: %s and %s" f1 f2
| _ -> "Too many fish to list!"


(*
    This process of recovering the constituents of a structured type is often called decomposition.
    Array decomposition is a little limited, as you have to specify either arrays of specific
    sizes (including size zero), or a catch-all case using an underscore. List decomposition is
    a bit more powerful, as befits the linked structure of a list (Listing 6-7).
*)

// Listing 6-7. Pattern matching on lists
let list0 = [                                                   ]
let list1 = ["One fish"                                         ]
let list2 = ["One fish"; "Two fish"                             ]
let list3 = ["One fish"; "Two fish"; "Red fish"                 ]
let list4 = ["One fish"; "Two fish"; "Red fish"; "Blue fish"    ]

let list = list4

// Chapter 6 Pattern Matching

// "A pond containing one fish: One fish (and 3 more fish)"
match list with
| [] -> "An empty pond"
| [ fish ] -> sprintf "A pond containing one fish only: %s" fish
| f::t when f = "One fish" -> sprintf "%s" f
| f as x::t  -> sprintf "%s" f
| head::tail -> sprintf "A pond containing one fish: %s (and %i more fish)" head ( tail |> List.length )
    //head::tail is just convention. h::t is also used to conciseness.

(*
    Using the "cons" (::) operator
     you can use the cons operator to join a single element onto the beginning of a list
    (e.g., "One fish" :: [ "Two fish"; "Red fish" ])
*)

let constructedList = "1st Element" :: list3
let constructedList2 = constructedList :: [list4; list2] //NOTE: Because the first element is a list, it expects a list of lists after the cons operator.

// Listing 6-9. Pattern matching on tuples using match
open System
let tryParseInt (s:string) =
    match Int32.TryParse(s) with
    | true, i -> Some i
    | false, _ -> None

// Some 30
"30" |> tryParseInt
// None
"3X" |> tryParseInt



// Listing 6-12. Pattern Matching on a DU
type MeterReading =
    | Standard of int
    | Economy7 of Day:int * Night:int

let formatReading (reading : MeterReading) =
    match reading with
    | Standard reading -> sprintf "Your reading: %07i" reading
    | Economy7(Day=day; Night=night) -> sprintf "Your readings: Day: %07i Night: %07i" day night

let reading1a = Standard 12982
let reading2a = Economy7(Day=3432, Night=98218)

reading1a |> formatReading
reading2a |> formatReading


// Table 6-1. DU Labeled Payload Elements Construction and Decomposition Syntax

// Action Syntax
// Construction Economy7(Day=3432, Night=98218)
// Decomposition Economy7(Day=day; Night=night)

// Listing 6-13. DUs and pattern matching without payload labels
type MeterReadingB =
    | StandardB of int
    | Economy7B of int * int

let formatReading2 (reading : MeterReadingB) =
    match reading with
    | StandardB reading -> sprintf "Your reading: %07i" reading
    | Economy7B(day, night) -> sprintf "Your readings: Day: %07i Night: %07i" day night

let reading1b = Standard 12982
let reading2b = Economy7(3432, 98218)

reading1a |> formatReading
reading2a |> formatReading

// Listing 6-14. Implementing complex numbers using a single-case DU
type Complex = Complex of Real:float * Imaginary:float

let add (Complex(Real=a;Imaginary=b)) (Complex(Real=c;Imaginary=d)) =
    Complex(Real=(a+c), Imaginary=(b+d))

// let (+) = add
let c1 = Complex(Real = 0.2, Imaginary = 3.4)
let c2 = Complex(Real = 2.2, Imaginary = 9.8)
// Complex (2.4,13.2)
let c3 = add c1  c2


// Listing 6-17. Pattern matching in a let binding
type Complex2 = Complex2 of Real2:float * Imaginary2:float

let c1_2 = Complex2 (Real2 = 0.2, Imaginary2 = 3.4)
let (Complex2(real, imaginary)) = c1_2

// 0.200000, 3.400000
printfn "%f, %f" real imaginary
// You can also use the component labels if you want to, that is
let (Complex2(Real2=real_a; Imaginary2=imaginary_a)) = c1_2


//Listing 6-19. Pattern matching in loops
let fruits =
    [
        "Apples", 3;
        "Oranges", 4;
        "Bananas", 2 
    ]

// There are 3 Apples
// There are 4 Oranges
// There are 2 Bananas
for (name, count) in fruits do printfn "There are %i %s" count name

// There are 3 Apples
// There are 4 Oranges
// There are 2 Bananas
fruits |> List.iter (fun (name, count) -> printfn "There are %i %s" count name)

// Listing 6-20. Pattern matching in loop over a multi-case DU (bad practice!)
type Shape =
    | Circle of Radius:float
    | Square of Length:float
    | Rectangle of Length:float * Height:float

let shapes =
    [
        Circle 3.
        Square 4.
        Rectangle(5., 6.)
        Circle 4. 
    ]

// Circle of radius 3.000000
// Circle of radius 4.000000
for ( Circle r ) in shapes do
    printfn "Circle of radius %f" r



// Pattern Matching and Enums

// If you want a Discriminated Union to be treated more like a C# enum, you must assign
// each case a distinct value, where the value is one of a small set of simple types such as
// byte, int32, and char. Listing 6-21 shows how to combine this feature, together with the
// Sytem.Flags attribute, to make a simplistic model of the Unix file permissions structure.

// Listing 6-21. Simple model of Unix file permissions

open System
[<Flags>]
type FileMode =
    | None = 0uy
    | Read = 4uy
    | Write = 2uy
    | Execute = 1uy

let canRead (fileMode : FileMode) =
    fileMode.HasFlag FileMode.Read

let modea = FileMode.Read
let modeb = FileMode.Write
let modec = modea ^^^ modeb

// True
canRead (modea)
// False
canRead (modeb)
// True
canRead (modec)
let moded = modec ^^^ FileMode.Execute

canRead (moded)


// Listing 6-22. Pattern matching on an enum DU without a default case
module Listing6_22 =
    
    open System

    [<Flags>]
    type FileMode =
        | None = 0uy
        | Read = 4uy
        | Write = 2uy
        | Execute = 1uy

    let describe (fileMode : FileMode) =
        let read =
            // Compiler warning: Incomplete pattern matches...
            match fileMode with
            | FileMode.None -> "cannot"
            | FileMode.Read -> "can"
            | FileMode.Write -> "cannot"
            | FileMode.Execute -> "cannot"
    
        printfn "You %s read the file"
    // Because it makes a hole in type safety, I always avoid using enum DU’s except in very
    // specific scenarios, typically those involving language interop.

//Single Case Active Patterns

// The simplest form of Active Pattern is the Single Case Active Pattern. You declare it by
// writing a case name between (| and |) (memorably termed banana clips), followed by a
// single parameter, then some code that maps from the parameter value to the case.
// For instance, in Listing 6-23 we have an Active Pattern that takes a floating-point
// value and approximates it to a sensible value for a currency, which for simplicity we are
// assuming always has two decimal places.

// Listing 6-23. A Single Case Active Pattern
open System
let (|Currency|) (x : float) =
    Math.Round(x, 2)
    
// true
match 100./3. with
| Currency 33.33 -> true
| _ -> false


// Listing 6-24 shows us using Currency in the three contexts we have seen for other pattern matching: 
// match expressions, let bindings, and function parameters.

// Listing 6-24. Recovering decomposed values with Active Patterns

(* Commented out because we are declaring it in a previous listing.
// open System
// let (|Currency|) (x : float) =
//     Math.Round(x, 2)
*)

// "That didn't match: 33.330000"
// false
match 100./3. with
| Currency 33.34 -> true
| Currency c ->
    printfn "That didn't match: %f" c
    false

let (|Currency2|) (fl:float) =
    fl * 2.

// Cs: 33.330000
let (Currency c) = 1000./30.
printfn "Cs: %f" c

let add2 (Currency c1) (Currency c2) =
    c1 + c2

// 66.66
add2 (100./3.) (1000./30.)


// Listing 6-25. Categorizing wind turbines using Multi-Case Active Patterns and Regex
open System.Text.RegularExpressions

let (|Mitsubishi|Samsung|Other|) (s : string) =
    let m = Regex.Match(s, @"([A-Z]{3})(\-?)(.*)")
    if m.Success then
        match m.Groups.[1].Value with
        | "MWT" -> Mitsubishi
        | "SWT" -> Samsung
        | _ -> Other
    else
        Other

// From https://eerscmap.usgs.gov/uswtdb/
let turbines = [
    "MWT1000"; "MWT1000A"; "MWT102/2.4"; "MWT57/1.0"
    "SWT1.3_62"; "SWT2.3_101"; "SWT2.3_93"; "SWT-2.3-101"
    "40/500" ]

// MWT1000 is a Mitsubishi turbine
// ...
// SWT1.3_62 is a Samsung turbine
// ...
// 40/500 is an unknown turbine
turbines
|> Seq.iter (fun t ->
    match t with
    | Mitsubishi -> printfn "%s is a Mitsubishi turbine" t
    | Samsung -> printfn "%s is a Samsung turbine" t
    | Other -> printfn "%s is an unknown turbine" t)


turbines
|> Seq.countBy(fun t ->
    match t with
    | Mitsubishi -> "M"
    | Samsung -> "S"
    | _ -> "O"
)

// Partial Active Patterns

// Partial Active Patterns divide the world into things that match by some condition and
// things that don’t. If we just wanted to pick out the Mitsubishi turbines from the previous
// example, we could change the code to look like Listing 6-26.

// Listing 6-26. Categorizing wind turbines using Partial Active Patterns
open System.Text.RegularExpressions

let (|Mitsubishi|_|) (s : string) =
    let m = Regex.Match(s, @"([A-Z]{3})(\-?)(.*)")
    if m.Success then
        match m.Groups.[1].Value with
        | "MWT" -> Some s
        | _ -> None
    else
        None

// From https://eerscmap.usgs.gov/uswtdb/
let turbines = [
    "MWT1000"; "MWT1000A"; "MWT102/2.4"; "MWT57/1.0"
    "SWT1.3_62"; "SWT2.3_101"; "SWT2.3_93"; "SWT-2.3-101"
    "40/500" ]

turbines
|> Seq.iter ( fun t ->
    match t with
    | Mitsubishi m -> printfn "%s is a Mitsubishi turbine" m
    | s -> printfn "%s is not a Mitsubishi turbine" s )



// Parameterized Active Patterns

// You can parameterize Active Patterns, simply by adding extra parameters before the
// final one. (The last parameter is reserved for the primary input of the Active Pattern.)
// Say, for example, you had to validate postal codes for various regions. US postal codes
// (zip codes) consist of five digits, while UK ones have a rather wacky format consisting of
// letters and numbers (e.g., “RG7 1DP”). Listing 6-27 uses an active pattern, parameterized
// using a regular expression to define a valid format for the region in question.

// Listing 6-27. Using parameterized Active Patterns to validate postal codes

open System
open System.Text.RegularExpressions

let zipCodes = [ "90210"; "94043"; "10013"; "1OO13" ]
let postCodes = [ "SW1A 1AA"; "GU9 0RA"; "PO8 0AB"; "P 0AB" ]

let regexZip = @"^\d{5}$"
// Simplified regex (the official regex for UK postcodes is much longer!)
let regexPostCode = @"^(\d|[A-Z]){2,4} (\d|[A-Z]){3}"

let (|PostalCode|) pattern s =
    let m = Regex.Match(s, pattern)
    if m.Success then
        Some s
    else
        None

// None
let (PostalCode regexZip z) = "WRONG"
let (PostalCode regexPostCode z) = "WRONGAGAIN"

// ["90210"; "94043"; "10013"]
let validZipCodes =
    zipCodes
    |> List.choose (fun (PostalCode regexZip p) -> p)

// ["SW1A 1AA"; "GU9 0RA"; "PO8 0AB"]
let validPostCodes =
    postCodes
    |> List.choose (fun (PostalCode regexPostCode p) -> p)

// Pattern Matching with ‘&’

// Occasionally it’s useful to be able to 'and’ together items in a pattern match. Imagine,
// for example, your company is offering a marketing promotion that is only available to
// people living in ‘outer London’ (in the United Kingdom), as identified by their postcode.
// To be eligible, the user needs to provide a valid postcode, and that postcode must begin
// with one of a defined set of prefixes. Listing 6-28 shows one approach to coding this
// using active patterns.

// Listing 6-28. Using & with Active Patterns
open System.Text.RegularExpressions

let (|PostCode|) s =
    let m = Regex.Match(s, @"^(\d|[A-Z]){2,4} (\d|[A-Z]){3}")
    if m.Success then
        Some s
    else
        None

let outerLondon =
    ["BR";"CR";"DA";"EN";"HA";"IG";"KT";"RM";"SM";"TW";"UB";"WD"]

let (|OuterLondon|) (s : string) =
    outerLondon
    |> List.tryFind (s.StartsWith)

let promotionAvailable (postcode : string) =
    match postcode with
    | PostCode(Some p) & OuterLondon(Some o) -> printfn "We can offer the promotion in %s (%s)" p o
    | PostCode(Some p) & OuterLondon(None) -> printfn "We cannot offer the promotion in %s" p
    | _ -> printfn "Invalid postcode"

//let demo() =
// "We cannot offer the promotion in RG7 1DP"
"RG7 1DP" |> promotionAvailable
// "We can offer the promotion in RM3 5NA (RM)"
"RM3 5NA" |> promotionAvailable
// "Invalid postcode"
"Hullo sky" |> promotionAvailable



// Pattern Matching on Types

// Occasionally even functional programmers have to deal with type hierarchies!
// Sometimes it’s because we are interacting with external libraries like System.Windows.
// Forms, which make extensive use of inheritance. Sometimes it’s because inheritance is
// genuinely the best way to model something, even in F#. Whatever the reason, this can
// place us in a position where we need to detect whether an instance is of a particular type,
// or is of a descendent of that type. You won’t be surprised to learn that F# achieves this
// using pattern matching.
// In Listing 6-29 we define a two-level hierarchy with a top-level type of Person,
// and one lower-level type Child, which inherits from Person and adds some extra
// functionality, in this case just the ability to print the parent’s name. 
// (For simplicity I’m assuming one parent per person.)

// Listing 6-29. Pattern matching on type
type Person (name : string) =
    member __.Name = name

type Child(name, parent : Person) =
    inherit Person(name)
    member __.ParentName = parent.Name

let alice = Person("Alice")
let bob = Child("Bob", alice)
let people = [ alice; bob :> Person ]

// Person: Alice
// Child: Bob of parent Alice
people
|> List.iter ( fun person ->
    match person with
    | :? Child as child -> printfn "Child: %s of parent %s" child.Name child.ParentName
    | _ as person -> printfn "Person: %s" person.Name
)


// Pattern Matching on Null

// Remember back in Chapter 3 we used Option.ofObj and Option.defaultValue to
// process a nullable string parameter? Listing 6-30 shows an example of that approach.

// Listing 6-30. Using Option.ofObj
let myApiFunction (stringParam : string) =
    let s =
        stringParam
        |> Option.ofObj
        |> Option.defaultValue "(none)"
    // You can do things here knowing that s isn't null
    
    sprintf "%s" (s.ToUpper())

// HELLO
myApiFunction "hello"
// (NONE)
myApiFunction null

// Well there is an alternative, because you can pattern match on the literal null. Here’s
// Listing 6-30, redone using null pattern matching (Listing 6-29).

// Listing 6-31. Pattern matching on null
let myApiFunction2 (stringParam : string) =
    match stringParam with
    | null -> "(NONE)"
    | _ -> stringParam.ToUpper()

// HELLO
myApiFunction2 "hello"
// (NONE)
myApiFunction2 null