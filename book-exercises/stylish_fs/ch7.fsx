// Listing 7-1. Declaring a record type

open System
type FileDescription = {
    Path : string
    Name : string
    LastModified : DateTime 
}

// Listing 7-2. Instantiating record type instances
open System.IO

let fileSystemInfo (rootPath : string) =
    Directory.EnumerateFiles(rootPath, "*.*",  SearchOption.TopDirectoryOnly)
    |> Seq.map ( fun path ->
        {
            Path = path |> Path.GetDirectoryName
            Name = path |> Path.GetFileName
            LastModified = (FileInfo(path)).LastWriteTime 
        }
    )
    // |> Seq.take 5

// Listing 7-3. Accessing record type fields using dot notation

// Name: ad.png Path: c:\temp Last modified: 15/08/2017 22:07:34
// Name: capture-1.avi Path: c:\temp Last modified: 27/02/2017 22:04:31
// ...

fileSystemInfo "C:\\Users\\luisf"
|> Seq.iter (fun info -> // info is a FileDescription instance
        printfn "Name: %s Path: %s Last modified: %A" info.Name info.Path info.LastModified
    )



// Listing 7-4. Declaring a record instance as mutable
type MyRecord = {
        String : string
        Int : int
    }

let mutable myRecord = { 
        String = "Hullo clouds"
        Int = 99 
    }

// {String = "Hullo clouds";
// Int = 99;}
printfn "%A" myRecord
myRecord <- { String = "Hullo sky"; Int = 100 }

// {String = "Hullo sky";
// Int = 100;}
printfn "%A" myRecord



// Listing 7-5. Declaring record fields as mutable
type MyRecordB = {
        mutable String : string
        mutable Int : int 
    }

let myRecordB = {
        String = "Hullo clouds"
        Int = 99 
    }

// {String = "Hullo clouds";
// Int = 99;}
printfn "%A" myRecord
myRecordB.String <- "Hullo sky"
// {String = "Hullo sky";
// Int = 99;}
printfn "%A" myRecord


// Listing 7-6. “Amending” a record using copy and update
type MyRecordC = {
        String : string
        Int : int
    }

let myRecordC = { String = "Hullo clouds"; Int = 99 }

// {String = "Hullo clouds";
// Int = 99;}
printfn "%A" myRecordC
let myRecord2 = { myRecordC with String = "Hullo sky" }
// {String = "Hullo sky";
// Int = 99;}
printfn "%A" myRecord2



// Listing 7-7. F# Object Oriented class types versus records
open System

type FileDescriptionOO(path:string, name:string, lastModified:DateTime) =
    member __.Path = path
    member __.Name = name
    member __.LastModified = lastModified

open System.IO

let fileSystemInfoOO (rootPath : string) =
    Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories)
    |> Seq.map (fun path ->
        FileDescriptionOO(
            path |> Path.GetDirectoryName,
            path |> Path.GetFileName,
            (FileInfo(path)).LastWriteTime)
        )

// Listing 7-8. Representing latitude and longitude using a class
type LatLon(latitude : float, longitude : float) =
    member __.Latitude = latitude
    member __.Longitude = longitude

// You might think that if two positions have the same latitude and longitude values,
// they would be considered equal. But they are not!1

// Listing 7-9. Some types are less equal than others!
let waterloo = LatLon(51.5031, -0.1132)
let victoria = LatLon(51.4952, -0.1441)
let waterloo2 = LatLon(51.5031, -0.1132)
// false
printfn "%A" (waterloo = victoria)
// true
printfn "%A" (waterloo = waterloo)
// false!
printfn "%A" (waterloo = waterloo2)




// Listing 7-10. Default structural (content) equality with record types
type LatLonRec = 
    {
        Latitude : float
        Longitude : float 
    }
let waterlooRec = { Latitude = 51.5031; Longitude = -0.1132 }
let victoriaRec = { Latitude = 51.4952; Longitude = -0.1441 }
let waterloo2Rec = { Latitude = 51.5031; Longitude = -0.1132 }
// false
printfn "%A" (waterlooRec = victoriaRec)
// true
printfn "%A" (waterlooRec = waterlooRec)
// true
printfn "%A" (waterlooRec = waterloo2Rec)


// Listing 7-11. Do all the fields of your record implement the right equality?
type Surveyor(name : string) =
    member __.Name = name

//This record mixes regular record fields with OO classes
type LatLon3 = 
    {
        Latitude : float
        Longitude : float
        SurveyedBy : Surveyor
    }

let waterloo3 = { Latitude = 51.5031; Longitude = -0.1132; SurveyedBy = Surveyor("Kit") }
let waterloo3_2 = { Latitude = 51.5031; Longitude = -0.1132; SurveyedBy = Surveyor("Kit") }

// true
printfn "%A" (waterloo3 = waterloo3)
// false
printfn "%A" (waterloo3 = waterloo3_2)



// Listing 7-12. Forcing reference equality for record types

[<ReferenceEquality>]
type LatLon4 = {
        Latitude : float
        Longitude : float
    }

let waterloo4 = { Latitude = 51.5031; Longitude = -0.1132 }
let waterloo4_2 = { Latitude = 51.5031; Longitude = -0.1132 }
// true
printfn "%A" (waterloo4 = waterloo4)
// false
printfn "%A" (waterloo4 = waterloo4_2)


// Listing 7-13. Marking a record type as a Struct
type LatLon5 = {
    Latitude : float
    Longitude : float }

[<Struct>]
type LatLonStruct = {
    Latitude : float
    Longitude : float }

#time "on"
// Real: 00:00:00.159, CPU: 00:00:00.156, GC gen0: 5, gen1: 3, gen2: 1
let llMany =
    Array.init 1_000_000 (fun x ->
        {
            LatLon5.Latitude = float x
            LatLon5.Longitude = float x 
        }
    )


// Real: 00:00:00.046, CPU: 00:00:00.046, GC gen0: 0, gen1: 0, gen2: 0
let llsMany =
    Array.init 1_000_000 (fun x ->
        {
            LatLonStruct.Latitude = float x
            LatLonStruct.Longitude = float x
        }
    )

#time "off"




// Listing 7-14. Struct Records must be mutable instances to mutate fields
[<Struct>]
type LatLonStructMutable = 
    {
        mutable Latitude : float
        mutable Longitude : float
    }

let waterloo6 = { Latitude = 51.5031; Longitude = -0.1132 }
// Error: a value must be mutable in order to mutate the contents.
waterloo6.Latitude <- 51.5032
let mutable waterloo6_2 = { Latitude = 51.5031; Longitude = -0.1132 }
waterloo6_2.Latitude <- 51.5032


// Listing 7-15. A generic record type
type LatLon<'T> = {
    mutable Latitude : 'T
    mutable Longitude : 'T }

// LatLon<float>
let waterloo = { Latitude = 51.5031; Longitude = -0.1132 }
// LatLon<float32>
let waterloo2 = { Latitude = 51.5031f; Longitude = -0.1132f }
// Error: Type Mismatch...
printfn "%A" (waterloo = waterloo2)


// Listing 7-16. Pinning down the generic parameter type of a record type

type LatLon7<'T> = {
    mutable Latitude : 'T
    mutable Longitude : 'T }

// LatLon<float>
let waterloo7 : LatLon7<float> = { Latitude = 51.5031; Longitude = -0.1132 }

// Error: The expression was expected to have type 'float32'
// but here has type 'float'.
let waterloo7_2 : LatLon<float32> = { Latitude = 51.5031f; Longitude = -0.1132 }


// Listing 7-17. A recursive Record Type
type Point = { X : float32; Y : float32 }

type UiControl = {
    Name : string
    Position : Point
    Parent : UiControl option }

let form = {
    Name = "MyForm"
    Position = { X = 0.f; Y = 0.f }
    Parent = None }

let button = {
    Name = "MyButton"
    Position = { X = 10.f; Y = 20.f }
    Parent = Some form }