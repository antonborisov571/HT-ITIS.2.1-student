module Hw5.MaybeBuilder

open System

type MaybeBuilder() =
    member builder.Bind(a, f): Result<'e,'d> =
        match a with
        |Ok success -> f success
        |Error error -> Error error
    member builder.Return x: Result<'a,'b> =
        Ok x
let maybe = MaybeBuilder()