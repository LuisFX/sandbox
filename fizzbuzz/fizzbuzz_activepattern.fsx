// https://stackoverflow.com/a/2429874/9549817

  // If the value is divisible, then we return 'Some()' which
  // represents that the active pattern succeeds - the '()' notation
  // means that we don't return any value from the pattern (if we
  // returned for example 'Some(i/divisor)' the use would be:
  //     match 6 with 
  //     | DivisibleBy 3 res -> .. (res would be asigned value 2)
  // None means that pattern failed and that the next clause should 
  // be tried (by the match expression)
let (|DivisibleBy|_|) divisor i = 
  if i % divisor = 0 then Some () else None 

for i in 1..100 do
  match i with
  // & allows us to run more than one pattern on the argument 'i'
  // so this calls 'DivisibleBy 3 i' and 'DivisibleBy 5 i' and it
  // succeeds (and runs the body) only if both of them return 'Some()'
  | DivisibleBy 3 re3 & DivisibleBy 5 re5 -> printfn "FizzBuzz"
  | DivisibleBy 3 res -> printfn "Fizz"  
  | DivisibleBy 5 res -> printfn "Buzz"
  | _ -> printfn "%d" i