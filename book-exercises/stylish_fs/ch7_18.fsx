// Listing 7-18. Instantiating a circular set of recursive records
// You probably don't want to do this!

type Point = { X : float32; Y : float32 }

type UiControl = {
    Name : string
    Position : Point
    Parent : UiControl }

let rec form = {
        Name = "MyForm"
        Position = { X = 0.f; Y = 0.f }
        Parent = button 
    }
    and button : UiControl = {
        Name = "MyButton"
        Position = { X = 10.f; Y = 20.f }
        Parent = form }