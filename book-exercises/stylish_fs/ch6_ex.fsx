// EXERCISE 6-1 – PATTERN MATCHING ON RECORDS WITH DUS

// Exercise: Let’s say you want to amend the code from Listing 6-12 so that a meter reading can
// have a date. This is the structure you might come up with:

// How would you amend the body of the formatReading function so that it formats your new
// MeterReading type in the following form?
// "Your readings on: 24/02/2019: Day: 0003432 Night: 0098218"
// "Your reading on: 23/03/2019 was 0012982"
// You can use DateTime.ToShortDateString() to format the date.

type MeterValue =
    | Standard of int
    | Economy7 of Day:int * Night:int

type MeterReading =
    {
        ReadingDate : System.DateTime
        MeterValue : MeterValue
    }


let formatReading (reading : MeterReading) =
    let dateString = reading.ReadingDate.ToShortDateString()
    
    match reading.MeterValue with
    | Standard stdReading -> sprintf "Your reading on: %A %07i" dateString stdReading
    | Economy7(Day=day; Night=night) -> sprintf "Your readings on: %A Day: %07i Night: %07i" dateString day night

//Book solution 1. record pattern match on the whole record structure.
let formatReadingBook1 (reading : MeterReading) =
    match reading with
    | { ReadingDate = readingDate; MeterValue = Standard reading } -> sprintf "Your reading on: %s was %07i" (readingDate.ToShortDateString()) reading
    | { ReadingDate = readingDate; MeterValue = Economy7(Day=day; Night=night) } -> sprintf "Your readings on: %s were Day: %07i Night: %07i" (readingDate.ToShortDateString()) day night

//Book solution 2. Record pattern match on the parameter declaration.
let formatReadingBook2 { ReadingDate = date; MeterValue = meterValue } =
    let dateString = date.ToShortDateString()
    match meterValue with
    | Standard reading -> sprintf "Your reading on: %s was %07i" dateString reading
    | Economy7(Day=day; Night=night) -> sprintf "Your readings on: %s were Day: %07i Night: %07i" dateString day night

// While all three solutions work, I feel like the book's solution 2 is more idiomatic. Solution 1 seems verbose and confusing.

let reading1a = { ReadingDate = System.DateTime.Now; MeterValue = Standard 12982 }
let reading2a = { ReadingDate = System.DateTime.Now.AddYears(1); MeterValue = Economy7(Day=3432, Night=98218) }

reading1a |> formatReading
reading2a |> formatReading






// EXERCISE 6-2 – RECORD PATTERN MATCHING AND LOOPS

// Exercise: Start with this code from Listing 6-19:

type FruitBatch =
    {
        Name : string
        Count : int
    }

let fruits =
    [
        { Name = "Apples"; Count = 3 }
        { Name = "Oranges"; Count = 4 }
        { Name = "Bananas"; Count = 2 }
    ]

// There are 3 Apples
// There are 4 Oranges
// There are 2 Bananas

for { Name = name; Count = count } in fruits do
    printfn "There are %i %s" count name

// There are 3 Apples
// There are 4 Oranges
// There are 2 Bananas

fruits
|> List.iter (fun  { Name = name; Count = count } ->
    printfn "There are %i %s" count name
)

// Add a record type called FruitBatch to the code, using field names Name and Count.
// How can you alter the fruits binding to create a list of FruitBatch instances, and the
// for loop and iter lamba so that they have the same output as they did before you added
// the record type?



// EXERCISE 6-3 – ZIP+4 CODES AND PARTIAL ACTIVE PATTERNS

// Exercise: In the United States, postal codes can take the form of simple 5-digit Zip codes, or
// ‘Zip+4’ codes, which have five digits, a hyphen, then four more digits. Here is some code that
// defines active patterns to identify Zip and Zip+4 codes, but with the body of the Zip+4 pattern
// omitted. The exercise is to add the body.

open System
open System.Text.RegularExpressions

let zipCodes = 
    [
        "90210"
        "94043"
        "94043-0138"
        "10013"
        "90210-3124"
        "1OO13" 
    ]

let (|USZipCode|_|) s =
    let m = Regex.Match(s, @"^(\d{5})$")
    if m.Success then
        USZipCode s |> Some
    else
        None

let (|USZipPlus4Code|_|) s =
    let m = Regex.Match(s,  @"^(\d{5})\-(\d{4})$")   
    if m.Success then
        USZipPlus4Code(m.Groups.[1].Value, m.Groups.[2].Value) |> Some
    else
        None

zipCodes
|> List.iter (fun z ->
    match z with
    | USZipCode c -> printfn "A normal zip code: %s" c
    | USZipPlus4Code(code, suffix) -> printfn "A Zip+4 code: prefix %s, suffix %s" code suffix
    | n ->  printfn "Not a zip code: %s" n)

// Hint: a regular expression to match Zip+4 codes is “^(\d{5})\-(\d{4})$”. When this
// expression matches, you can use m.Groups.[1].Value and m.Groups.[2].Value to
// pick out the prefix and suffix digits.