let add x y = x + y

//these following 2 are exactly the same, just refactored
let add' = fun x y -> x + y
let add'' = (+)

let add''' x = fun y -> x + y


//partially applied

let addPart = add 5
let a = addPart 10

//composition
// (2 * (number + 3)) ^2
let operation number = (2. * (number + 3. )) ** 2. 

//infix operator (the operator in between the two numbers)
let add3 number = number + 3

//prefix operators. 
//In F# operators can be called as functions. 
let add3' number = (+) 3 number

//we can then remove the "last argument" of the function because of currying
let add3'' = ( + ) 3. 
//in this particular case, the (+) funcion takes two parameters, 
//so the parameter passed to add3'' gets automatically passed to the (+) function

let times2 = ( * ) 2.

let pow2 number = number **  2.

//using piping
let operator' number =
    number
    |> add3''
    |> times2
    |> pow2

//using function composition
//this example is "point free" notation, which means, it takes an argument, but it's not visible from looking a the code.
//we must infer from the signature of the code.
let operator'' = add3'' >> times2 >> pow2

//defining the composition operator ">>"
let (>>) f g =
    //  the linter recommends to refactor lambda into composition: f >> g
    // another way to write the code below is: g ( f x )
    fun x ->
        x
        |> f
        |> g









