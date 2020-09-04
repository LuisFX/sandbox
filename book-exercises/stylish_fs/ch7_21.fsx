// Method Overrides

// Sometimes you want to change one of the (very few) methods that a record type has by
// default. The most common one to override is ToString(), which you can use to produce
// a nice printable representation of the record (Listing 7-21).

// Listing 7-21. Overriding a method on a Record
type LatLon = {
    Latitude : float
    Longitude : float }
with
    override this.ToString() = sprintf "%f, %f" this.Latitude this.Longitude

// 51.972300, 1.149700
{ Latitude = 51.9723; Longitude = 1.1497  }
|> printfn "%O"

// In Listing 7-21 I’ve used the “%O” format specifier, which causes the input’s
// ToString() method to be called.
