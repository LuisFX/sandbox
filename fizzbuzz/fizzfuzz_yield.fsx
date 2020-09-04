// https://stackoverflow.com/a/20253123/9549817

for n in 1..100 do 
    let s = seq { 
            if n % 3 = 0 then yield "Fizz"
            if n % 5 = 0 then yield "Buzz" 
        } 
    if Seq.isEmpty s then printf "%d"n
    printfn "%s"(s |> String.concat "")

// https://stackoverflow.com/a/2429908/9549817
// Doesn't account for the numbers which are not divisible 
// by either 3 or 5,
for i in 1..100 do
    for divisor, str in [ (3, "Fizz"); (5, "Buzz") ] do
        if i % divisor = 0 then printf "%s" str
    printfn ""