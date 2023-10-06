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

    let (isConverted1, arg1) = Double.TryParse args[0]
    let (isConverted2, arg2) = Double.TryParse args[2]

    match (isConverted1, isConverted2) with
    | (true, true) -> {arg1 = arg1; arg2 = arg2; operation = parseOperation args[1]}
    | _ -> ArgumentException("Incorrect first or second value entered") |> raise