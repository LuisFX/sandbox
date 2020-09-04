// Instance Methods

// Listing 7-19 shows us adding a Distance instance method to our familiar LatLon record,
// then calling it exactly as one would a class method.

// Listing 7-19. Adding an instance method to a Record Type
type LatLon =
    {
        Latitude : float
        Longitude : float
    }
    with
        // Naive, straight-line distance
        member this.DistanceFrom(other : LatLon) =
            let milesPerDegree = 69.

            ((other.Latitude - this.Latitude) ** 2.) + ((other.Longitude - this.Longitude) ** 2.)
            |> sqrt
            |> (*) milesPerDegree

let coleman =
    {
        Latitude = 31.82
        Longitude = -99.42
    }

let abilene =
    {
        Latitude = 32.45
        Longitude = -99.75
    }

// Are we going to Abilene? Because it's 49 miles!
printfn "Are we going to Abilene? Because it's %0.0f miles!" (abilene.DistanceFrom(coleman))

// Instance methods like this work fine with record types, and are quite a nice solution
// where you want structural (content) equality for instances, and also to have instance
// methods to give you fluent syntax like abilene.DistanceFrom(coleman).