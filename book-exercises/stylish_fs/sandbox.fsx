[|0..255|]
|> Array.iter( fun i ->
    // System.Console printfn "%d - %c" i (char i) 
    System.Console.WriteLine("This is an integer: {0} - {1}" , i, (char i))

)

System.Console.OutputEncoding <- System.Text.Encoding.UTF8;
[|0..1000|]
|> Array.iter( fun i -> 
    System.Console.WriteLine("{0} >> {1}", i, (char i))
)

let inline averageOrZero a =
    if Array.isEmpty a then
        LanguagePrimitives.GenericZero<'T>
    else
        a |> Array.average


//Recursive sequence

let randApi () =
    let r = System.Random()
    r.Next(0, 5)

let rec testRecSeq() =
    seq {
        match randApi() with
        | 0 -> ()
        | a ->
            yield a
            yield! testRecSeq()
    }

testRecSeq()
|> Seq.iter(printfn "Roll: %d" )

//Sequence of exaclty 1?
let seqExactlyOne() =
    seq{
         1
    }

seqExactlyOne() |> Seq.iter( printfn "One: %d" )


//tryMax and tryMaxBy

let tryMax a =
    if Array.isEmpty a then None
    else Array.max a

let tryMaxBy f a =
    if Array.isEmpty a then None
    else Array.maxBy f a |> Some


