#r "C:\\Users\\luisf\\.nuget\\packages\\fscheck\\3.0.0-alpha4\\lib\\netstandard1.6\\FsCheck"
#r "C:\\Users\\luisf\\.nuget\\packages\\expecto\\9.0.2\\lib\\netstandard2.0\\Expecto"

open FsCheck
open Expecto
open Prop

let revRevIsOrig (xs:list<int>) =
    List.rev(List.rev xs) = xs

Check.Quick revRevIsOrig


let revIsOrig (xs:list<int>) = 
    List.rev xs = xs

Check.Quick revIsOrig
Check.Verbose revIsOrig



let config = { FsCheck.Config.Default with MaxTest = 10000 }

let properties =
  testList "FsCheck samples" [
    testProperty "Addition is commutative" <| fun a b ->
      a + b = b + a
      
    testProperty "Reverse of reverse of a list is the original list" <|
      fun (xs:list<int>) -> List.rev (List.rev xs) = xs

    // you can also override the FsCheck config
    testPropertyWithConfig config "Product is distributive over addition" <|
      fun a b c ->
        a * (b + c) = a * b + a * c
  ]

Tests.runTests defaultConfig properties