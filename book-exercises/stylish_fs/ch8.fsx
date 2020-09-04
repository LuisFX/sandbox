// Listing 8-1. A console input prompt class

open System
type ConsolePrompt(message : string) =
    member this.GetValue() =
        printfn "%s:" message
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            input
        else
            Console.Beep()
            this.GetValue()

let firstPrompt = ConsolePrompt("Please enter your first name")
let lastPrompt = ConsolePrompt("Please enter your last name")
let demo() =
    let first, last = firstPrompt.GetValue(), lastPrompt.GetValue()
    printfn "Hello %s %s" first last

// > demo();;
// Please enter your first name:
// Kit
// Please enter your last name:
// Eason
// Hello Kit Eason

// Same, but with a function instead of classes (LuisFX version)
let rec consolePrompt message =
    printfn "%s:" message
    let input = Console.ReadLine()
    if not (String.IsNullOrWhiteSpace(input)) then
        input
    else
        Console.Beep()
        consolePrompt message


let demo2() =
    let firstPrompt2 = consolePrompt "Please enter your first name"
    let lastPrompt2 = consolePrompt "Please enter your last name"
    let first, last = firstPrompt2, lastPrompt2
    printfn "Hello %s %s" first last

demo2()



// Listing 8-2. A class with code in its constructor
open System
type ConsolePrompt2(message : string) =
    do
        if String.IsNullOrWhiteSpace(message) then
            raise <| ArgumentException("Null or empty", "message")

    let message = message.Trim()
    member this.GetValue() =
        printfn "%s:" message
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            input
        else
            Console.Beep()
            this.GetValue()

let demo3() =
    // System.ArgumentException: Null or empty
    // Parameter name: message
    let first = ConsolePrompt2(null)
    printfn "Hello %s" (first.GetValue())




// Values as Members

// If you want to publish constructor values (or values derived from them) as members, you
// can do so as in Listing 8-3.

// Listing 8-3. Values as members
open System
type ConsolePrompt3(message : string) =
    do
        if String.IsNullOrWhiteSpace(message) then
            raise <| ArgumentException("Null or empty", "message")

    let message = message.Trim()
    member __.Message = message

    member this.GetValue() =
        printfn "%s:" message
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            input
        else
            Console.Beep()
            this.GetValue()

let first = ConsolePrompt3("First name")
// First name
printfn "%s" first.Message



// Getters and Setters

// If you want simple properties that can be retrieved and set, but without any logic to
// compute the values or validate or process input, you can use member val syntax with
// default getters and setters. For example, in Listing 8-4 I’ve amended ConsolePrompt
// so that you can control whether there is a “beep” when the user enters invalid input
// (for example, an empty string). There’s a settable BeepOnError property that GetValue
// consults to decide whether to beep.

// Listing 8-4. Adding a mutable property with member val and default getter and setter
open System
type ConsolePrompt4(message : string) =
    do
        if String.IsNullOrWhiteSpace(message) then
            raise <| ArgumentException("Null or empty", "message")

    let message = message.Trim()
    member val BeepOnError = true
        with get, set
        
    member __.Message = message
    member this.GetValue() =
        printfn "%s:" message
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            input
        else
            if this.BeepOnError then
                Console.Beep()
            this.GetValue()

let demo4() =
    let first = ConsolePrompt4("First name")
    first.BeepOnError <- false
    let name = first.GetValue()

    // No beep on invalid input!
    printfn "%s" name

// Using a so-called auto-implemented property is a reasonable thing to do here:
// BeepOnError is not an important enough thing to have as a constructor parameter, and a
// default value is fine; but one would also like to have the flexibility to change it. And as a
// Boolean it hardly needs validation!




// Additional Constructors

// You might disagree with my assertion above that BeepOnError isn’t an important
// enough property to have in a constructor parameter. After all, by making it a member
// val we have cracked open the door to mutability, something we could have avoided by
// making beepOnError a constructor parameter. Luckily you can dodge the whole issue by
// declaring additional constructors (Listing 8-5).

// Listing 8-5. An additional constructor
open System
type ConsolePrompt5(message : string, beepOnError : bool) =
    do
        if String.IsNullOrWhiteSpace(message) then
            raise <| ArgumentException("Null or empty", "message")
    
    let message = message.Trim()

    new (message : string) = ConsolePrompt5(message, true)
    
    member this.GetValue() =
        printfn "%s:" message
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            input
        else
            if beepOnError then
                Console.Beep()
            this.GetValue()

let demo5() =
    let first = ConsolePrompt5("First name", false)
    let last = ConsolePrompt5("Second name")
    // No beep on invalid input!
    let firstName = first.GetValue()
    // Beep on invalid input!
    let lastName = last.GetValue()
    printfn "Hello %s %s" firstName lastName


// Explicit Getters and Setters

// Going back to mutable properties: sometimes auto-implemented properties aren’t
// sufficient for your needs. What do you do if, for instance, you want to validate the value
// that is being set, or calculate a value on demand? For these kinds of operations, you can
// use explicit getters and setters. Let’s extend the ConsolePrompt class so that you can set
// the foreground and background colors of the prompt, and let’s also make a rule that the
// foreground and background colors can’t be the same (Listing 8-6).

//Listing 8-6. Explicit getters and setters
open System

type ConsolePrompt6(message : string) =
    let mutable foreground = ConsoleColor.White
    let mutable background = ConsoleColor.Black
    
    member __.ColorScheme
        with get() =
            foreground, background

        and set(fg, bg) =
            if fg = bg then
                raise <| ArgumentException("Foreground, background can't be same")
            
            foreground <- fg
            background <- bg

    member this.GetValue() =
        Console.ForegroundColor <- foreground
        Console.BackgroundColor <- background
        printfn "%s:" message
        Console.ResetColor()
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            input
        else
            this.GetValue()

let demo6() =
    let first = ConsolePrompt6("First name")
    // System.ArgumentException: Foreground, background can't be same
    first.ColorScheme <- (ConsoleColor.Blue, ConsoleColor.Yellow)
    printfn "Color Scheme!"
    // Note For simplicity, in Listing 8-6 I’ve omitted some of the features we added to
    // ConsolePrompt in previous listings.

do
    printfn "Running Demo 6"
    demo6()