module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> ArgumentException("Entered incorrectly operation") |> raise
    
let parseCalcArguments(args : string[]) =
    if (isArgLengthSupported args <> true) then ArgumentException("Incorrect number of arguments entered") |> raise

    match Double.TryParse args[0] with
    | true, arg1 -> None |> ignore
    | _ -> ArgumentException("Incorrect first value entered") |> raise

    match Double.TryParse args[2] with
    | true, arg2 -> None |> ignore
    | _ -> ArgumentException("Incorrect second value entered") |> raise

    let calc = {arg1 = Double.Parse args[0]; arg2 = Double.Parse args[2]; operation = parseOperation args[1]}
    calc