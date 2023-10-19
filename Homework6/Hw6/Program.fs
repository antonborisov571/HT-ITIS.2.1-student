module Hw6.App

open System
open Hw5
open System.Globalization
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

let parseSign sign =
    match sign with
    | "Plus" -> "+"
    | "Minus" -> "-"
    | "Multiply" -> "*"
    | "Divide" -> "/"
    | error -> error 

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let result: Result<string, string> = 
            let query = ctx.Request.Query
            let args = [|
                ctx.TryGetQueryStringValue("value1").Value ; 
                ctx.TryGetQueryStringValue("operation").Value |> parseSign; 
                ctx.TryGetQueryStringValue("value2").Value
            |]

            match Parser.parseCalcArguments args with 
            | Ok (arg1, operation, arg2) -> Ok (string (Calculator.calculate arg1 operation arg2))
            | Error errorValue -> 
                match errorValue with
                | Message.WrongArgFormatOperation -> Error $"Could not parse value '{args[1]}'"
                | Message.WrongArgFormat -> 
                    match Parser.parseDouble args[0] with 
                    | Ok value -> Error $"Could not parse value '{args[2]}'"
                    | _ -> Error $"Could not parse value '{args[0]}'"
                | _ -> Ok "DivideByZero"


        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use //calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0