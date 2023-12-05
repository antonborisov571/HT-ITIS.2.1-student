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
            let value1 = ctx.GetQueryStringValue("value1")
            let operation = ctx.GetQueryStringValue("operation")
            let value2 = ctx.GetQueryStringValue("value2")
            
            match (value1, operation, value2) with 
            | (Ok value1, Ok operation, Ok value2) ->
                match Parser.parseCalcArguments [|value1; parseSign operation; value2|] with 
                | Ok (arg1, op, arg2) -> Ok (string (Calculator.calculate arg1 op arg2))
                | Error errorValue -> 
                    match errorValue with
                    | Message.WrongArgFormatOperation -> Error $"Could not parse value '{operation}'"
                    | Message.WrongArgFormat -> 
                        match Parser.parseDouble value1 with 
                        | Ok value -> Error $"Could not parse value '{value2}'"
                        | _ -> Error $"Could not parse value '{value1}'"
                    | _ -> Ok "DivideByZero"
            | _ -> Error "One parameter is not passed"


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
        services.AddGiraffe().AddMiniProfiler(fun option -> option.RouteBasePath <- "/profiler") |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseMiniProfiler().UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0