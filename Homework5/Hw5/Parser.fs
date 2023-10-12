module Hw5.Parser

open System
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<(string * string * string), Message> =
    match args.Length = 3 with
    | true -> Ok (args[0], args[1], args[2])
    | false -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with 
    | "+" -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | "-" -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | "*" -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | "/" -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseDouble (arg: string): Result<float, Message> =
    match Double.TryParse arg with
    | (true, value) -> Ok value
    | _ -> Error Message.WrongArgFormat

let parseArgs (args: string[]): Result<(float * CalculatorOperation * float), Message> =
    maybe {
        let! args = isArgLengthSupported args
        let! arg1, operation, arg2 = isOperationSupported args
        let! arg1 = parseDouble arg1
        let! arg2 = parseDouble arg2
        return (arg1, operation, arg2)
    } 

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<(float * CalculatorOperation * float), Message> =
    match operation, arg2.ToString() with
    | CalculatorOperation.Divide, "0" -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)
    
let parseCalcArguments (args: string[]): Result<(float * CalculatorOperation * float), Message> =
    maybe {
        let! args = parseArgs args
        let! args = isDividingByZero args
        return  args
    }