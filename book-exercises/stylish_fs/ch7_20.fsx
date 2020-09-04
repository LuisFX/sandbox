// Static Methods

// You can also add static methods. If you do this, it’s probably because you want to
// construct a record instance using something other than standard record construction
// syntax. For example, Listing 7-20 adds a TryFromString method to LatLon, which tries
// to parse a comma-separated string into two elements, and then tries to parse these as
// floating-point numbers, before finally constructing a record instance in the usual curlybracket way.

// Listing 7-20. Adding a static method to a Record Type
open System
type LatLon = {
    Latitude : float
    Longitude : float }

with
    static member TryFromString(s : string) =
        match s.Split([|','|]) with
        | [|lats; lons|] ->
            match (Double.TryParse(lats), Double.TryParse(lons)) with
            | (true, lat), (true, lon) ->
                {
                    Latitude = lat
                    Longitude = lon
                } |> Some
            | _ -> None
        | _ ->
            None
// Some {Latitude = 50.514444;
// Longitude = -2.457222;}
let somewhere = LatLon.TryFromString "50.514444, -2.457222"
// None
let nowhere = LatLon.TryFromString "hullo trees"
// This is quite a nice way of effectively adding constructors to record types. It might be
// especially useful it you want to perform validation during construction.