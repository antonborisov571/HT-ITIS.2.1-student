open Hw4

[<EntryPoint>]
let main (args : string[]) =
    try
        let calc = Parser.parseCalcArguments args
        printf "%f" (Calculator.calculate calc.arg1 calc.operation calc.arg2)
    with
        | ex -> printf "%s" (ex.Message)
    0