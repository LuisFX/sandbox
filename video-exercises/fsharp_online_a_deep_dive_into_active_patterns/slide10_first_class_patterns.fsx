let unfold (|Q|_|) input =
    let rec loop values = function
        | Q(v, next) -> loop (v::values) next
        | otherwise -> (List.rev values, otherwise)
    loop [] input


type Expr =
    | App of Expr * Expr
    | Lam of head : string * body : Expr
    | Var of name : string

let (|Lambda|_|) = function
    | Lam(head, body) -> Some(head, body)
    | _ -> None

let (|Lambdas|) expr = unfold (|Lambda|_|) expr