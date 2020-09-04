// https://stackoverflow.com/a/23044181/9549817

let fizzy num =     
   match num%3, num%5 with      
      | 0,0 -> "fizzbuzz"
      | 0,_ -> "fizz"
      | _,0 -> "buzz"
      | _,_ -> num.ToString()

let prnt = printfn "%s"

[1..100]
  |> List.map fizzy
  |> List.iter prnt
