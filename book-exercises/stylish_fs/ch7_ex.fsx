[<Struct>]
type Position =
    {
        X : float32
        Y : float32
        Z : float32
        Time: System.DateTime
    }

# time "on"
let arr =
    Array.init 1_000_000 (fun i -> 
        {
            X = float32 i
            Y = float32 i
            Z = float32 i
            Time = System.DateTime.Now
        }
    )
#time "off"

// EXERCISE 7-4 – MODIFYING RECORDS

// open System
// [<Struct>]
// type Position = {
//     X : float32
//     Y : float32
//     Z : float32
//     Time : DateTime }

let translate dx dy dz position =
    {
        position
        with
            X = position.X + dx
            Y = position.Y + dy
            Z = position.Z + dz
    }

let p1 =
    {
        X = 1.0f
        Y = 2.0f
        Z = 3.0f
        Time = System.DateTime.MinValue
    }
    
// val p2 : Position = {X = 1.5f;
// Y = 1.5f;
// Z = 4.5f;
// Time = 01/01/0001 00:00:00;}
let p2 = p1 |> translate 0.5f -0.5f 1.5f