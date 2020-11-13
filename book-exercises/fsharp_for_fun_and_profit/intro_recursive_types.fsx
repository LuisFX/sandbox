open System
//https://fsharpforfunandprofit.com/posts/recursive-types-and-folds/

//Basic Recursive Types:
// https://fsharpforfunandprofit.com/posts/recursive-types-and-folds/#basic-recursive-type

//Model a Gift
type Book = {Title: string; Price: decimal}
type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {
    ChocolateType: ChocolateType
    Price: decimal
}

type WrappingPaperType =
    | Birthday
    | Holiday
    | SolidColor

type Gift =
    | Book of Book
    | Chocolate of Chocolate
    | Wrapped of Gift * WrappingPaperType
    | Boxed of Gift
    | WithCard of Gift * Message: string

// You can see that three of the cases are “containers” that refer to another Gift. The Wrapped case has some paper and a inner gift, as does the Boxed case, as does the WithACard case. The two other cases, Book and Chocolate, do not refer to a gift and can be considered “leaf” nodes or terminals.
// The presence of a reference to an inner Gift in those three cases makes Gift a recursive type. Note that, unlike functions, the rec keyword is not needed for defining recursive types.
// Let’s create some example values:

//A book
let wolfHall = { Title="Wolf Hall"; Price = 20m }

//A chocolate
let yummyChoc = {
    ChocolateType = Dark
    Price = 15m
}

//A gift
let bday = WithCard( Wrapped(Book wolfHall, Birthday ), Message = "Hppy Bday")

//A gift
let xmasPresent = Wrapped(Boxed(Chocolate yummyChoc), Holiday)


// Guideline: Avoid infinitely recursive types
// I suggest that, in F#, every recursive type should consist of a mix of recursive and non-recursive cases. If there were no non-recursive elements, such as Book, all values of the type would have to be infinitely recursive.

// For example, in the ImpossibleGift type below, all the cases are recursive. To construct any one of the cases you need an inner gift, and that needs to be constructed too, and so on.
// type ImpossibleGift =
//     | Boxed of ImpossibleGift
//     | WithCard of ImpossibleGift * Message:string


let rec cataGift fBook fChoco fWrap fBox fCard gift =
    match gift with
    | Book b -> fBook b
    | Chocolate c -> fChoco c
    | Wrapped (innerGift, style) -> 
        let innerGiftResult = cataGift fBook fChoco fWrap fBox fCard innerGift
        fWrap (innerGiftResult, style)
    | Boxed innerGift ->
        let innerGiftResult = cataGift fBook fChoco fWrap fBox fCard innerGift
        fBox innerGiftResult
    | WithCard (innerGift, message) ->
        let innerGiftResult = cataGift fBook fChoco fWrap fBox fCard innerGift
        fCard (innerGiftResult,message)

let totalGiftCost gift =
    let fBook (b:Book) = b.Price
    let fChoco (c:Chocolate) = c.Price
    let fWrap (innerCost, _) = innerCost + 0.5m
    let fBox innerCost = innerCost + 1.0m
    let fCard (innerCost, _) = innerCost + 2.0m

    cataGift fBook fChoco fWrap fBox fCard gift

let desc gift =
    let fBook (b: Book) = sprintf "%s" b.Title
    let fChoco (c: Chocolate) = sprintf "%A" c.ChocolateType
    let fWrap (innerMessage, style) = sprintf "%s wrapped in %A paper" innerMessage style
    let fBox innerMessage = sprintf "%s in a box" innerMessage
    let fCard (innerMessage, cardMessage) =sprintf "%s with a card saying '%s'" innerMessage cardMessage

    cataGift fBook fChoco fWrap fBox fCard gift


sprintf "%s costing %f" (bday |> desc) (bday |> totalGiftCost)