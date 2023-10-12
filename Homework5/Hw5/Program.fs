open Hw5

[<EntryPoint>]
let main args =
    match Parser.parseCalcArguments args with
    | Ok (arg1, operation, arg2) -> printfn "%f" (Calculator.calculate arg1 operation arg2)
    | Error errorValue -> 
        match errorValue with
        | Message.DivideByZero -> printfn "Division by zero"
        | Message.WrongArgFormat -> printfn "Incorrect first or second value entered"
        | Message.WrongArgLength -> printfn "Incorrect number of arguments entered"
        | Message.WrongArgFormatOperation -> printfn "Entered incorrectly operation"
        | _ -> printfn "Unknown error"
    0   