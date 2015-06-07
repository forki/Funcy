﻿#nowarn "67"
namespace Funcy.Test

open System
open Funcy
open Persimmon

module EitherTest =

    let ``Either<TLeft, TRight>.Right creates Right<TLeft, TRight> instance`` =
        test "Either<TLeft, TRight>.Right creates Right<TLeft, TRight> instance" {
            let right = Either<exn, int>.Right(1)
            do! assertEquals typeof<Right<exn, int>> <| right.GetType()
        }
    let ``Either<TLeft, TRight>.Left creates Left<TLeft, TRight> instance`` =
        test "Either<TLeft, TRight>.Left creates Left<TLeft, TRight> instance" {
            let left = Either<exn, string>.Left(Exception("hoge"))
            do! assertEquals typeof<Left<exn, string>> <| left.GetType()
        }
    let ``Right<TLeft, TRight> as IRight<TLeft, TRight> then Value should return its right value`` =
        test "Right<TLeft, TRight> as IRight<TLeft, TRight> then Value should return its right value" {
            let right = Either<exn, float>.Right(2.5) :> IRight<exn, float>
            do! assertEquals 2.5 right.Value
        }
    let ``Right<TLeft, TRight>.IsRight should return true`` = test "Right<TLeft, TRight>.IsRight should return true" {
        let right = Either<exn, string>.Right("hoge")
        do! assertPred right.IsRight
    }
    let ``Right<TLeft, TRight>.IsLeft should return false`` = test "Right<TLeft, TRight>.IsLeft should return false" {
        let right = Either<exn, string>.Right("hoge")
        do! assertPred <| not right.IsLeft
    }
    let ``When Right<TLeft, TRight> as IEither<TLeft, TRight> then IsRight should return true`` =
        test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then IsRight should return true" {
            let right = Either<exn, float32>.Right(-1.0f) :> IEither<exn, float32>
            do! assertPred right.IsRight
        }
    let ``When Right<TLeft, TRight> as IEither<TLeft, TRight> then IsLeft should return false`` =
        test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then IsLeft should return false" {
            let right = Either<exn, obj>.Right(obj()) :> IEither<exn, obj>
            do! assertPred <| not right.IsLeft
        }
    let ``When Right<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance`` =
        test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should return IRight<TLeft, TRight> instance" {
            let right = Either<exn, int>.Right(-1) :> IEither<exn, int>
            do! assertPred (right.ToRight() :? IRight<exn, int>)
        }
    let ``When Right<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should raise InvalidCastException`` =
        test "When Right<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should raise InvalidCastException" {
            let right = Either<exn, string>.Right("egg") :> IEither<exn, string>
            let! e = trap { right.ToLeft() |> ignore }
            do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
        }
    let ``Left<TLeft, TRight> as ILeft<TLeft, TRight> then Value should return its left value`` =
        test "Left<TLeft, TRight> as ILeft<TLeft, TRight> then Value should return its left value" {
            let err = Exception("Left")
            let left = Either<exn, float>.Left(err) :> ILeft<exn, float>
            do! assertEquals err left.Value
        }
    let ``Left<TLeft, TRight>.IsRight should return false`` = test "Left<TLeft, TRight>.IsRight should return false" {
        let left = Either<exn, int>.Left(Exception("fuga"))
        do! assertPred <| not left.IsRight
    }
    let ``Left<TLeft, TRight>.IsLeft should return true`` = test "Left<TLeft, TRight>.IsLeft should return true" {
        let left = Either<exn, int>.Left(Exception("fuga"))
        do! assertPred left.IsLeft
    }
    let ``When Left<TLeft, TRight> as IEither<TLeft, TRight> Then IsRight should return false`` =
        test "When Left<TLeft, TRight> as IEither<TLeft, TRight> Then IsRight should return false" {
            let left = Either<exn, int>.Left(Exception("fuga")) :> IEither<exn, int>
            do! assertPred <| not left.IsRight
        }
    let ``When Left<TLeft, TRight> as IEither<TLeft, TRight> Then IsLeft should return true`` =
        test "When Left<TLeft, TRight> as IEither<TLeft, TRight> Then IsLeft should return true" {
            let left = Either<exn, int>.Left(Exception("fuga")) :> IEither<exn, int>
            do! assertPred left.IsLeft
        }
    let ``When Left<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should raise InvalidCastException`` =
        test "When Left<TLeft, TRight> as IEither<TLeft, TRight> then ToRight should raise InvalidCastException" {
            let left = Either<exn, int list>.Left(Exception("Not List")) :> IEither<exn, int list>
            let! e = trap { left.ToRight() |> ignore }
            do! assertEquals typeof<System.InvalidCastException> <| e.GetType()
        }
    let ``When Left<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance`` =
        test "When Left<TLeft, TRight> as IEither<TLeft, TRight> then ToLeft should return ILeft<TLeft, TRight> instance" {
            let left = Either<exn, bool>.Left(Exception("ToLeft")) :> IEither<exn, bool>
            do! assertPred (left.ToLeft() :? ILeft<exn, bool>)
        }