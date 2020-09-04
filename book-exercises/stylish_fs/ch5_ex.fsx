open System
open System.Diagnostics

//  EXERCISE 5-1 – CLIPPING A SEQUENCE
// Write a function “clip,” which takes a sequence of values, and returns a sequence of the same
// length, in which the values are the same as the inputs, except elements that were higher than
// a defined ceiling are replaced with that ceiling.
let clip (c:float) s =
    s
    |> Seq.map( fun e ->
        match e > c with
        | false -> e
        | true -> c
    )
 
 // seq [1.0; 2.3; 10.0; -5.0]
[| 1.0; 2.3; 11.1; -5. |]
|> clip 10.


// You can achieve the requirement by writing a function that takes a required ceiling value and a
// sequence. Then you can use Seq.map to map the input values to the lower of either the input
// element or the specified ceiling, using the built-in min function.
let clip2 ceiling (s : seq<_>) =
    s
    |> Seq.map (fun x -> min x ceiling)


// seq [1.0; 2.3; 10.0; -5.0]
[| 1.0; 2.3; 11.1; -5. |]
|> clip 10.

//  EXERCISE 5-2 – MINIMUM AND MAXIMUM
// You come across a function that appears to be designed to calculate the minimum and
// maximum values in a sequence:

let extremes (s : seq<float>) =
    let mutable min = Double.MaxValue
    let mutable max = Double.MinValue
    for item in s do
        if item < min then
            min <- item
        if item > max then
            max <- item
    min, max

 // (-5.0, 11.1)
[| 1.0; 2.3; 11.1; -5. |]
|> extremes

// How would you rewrite the function to avoid using mutable values? You can ignore the
// situation where the input sequence is empty.

let extremesLfx (s:seq<float>) =
    (s|> Seq.min), (s |> Seq.max)




let r = System.Random()
let big = Array.init 1_000_000 (fun _ -> r.NextDouble())
let sw = Stopwatch()
sw.Start()
let min, max = big |> extremesLfx
sw.Stop()
// 8ms for the original, mutable version
// 12ms for the Seq.max/Seq.min version, when s is specified as a seq<float>
// 272ms when is a seq<_>, i.e. as generic
// 17ms when generic, but inlined
printfn "min: %f max: %f - time: %ims" min max sw.ElapsedMilliseconds
