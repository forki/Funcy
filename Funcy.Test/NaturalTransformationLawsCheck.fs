﻿namespace Funcy.Test

open System
open Funcy
open Persimmon
open Persimmon.Dried
open UseTestNameByReflection

module NaturalTransformationLawsCheck =

    (* commutative diagram
    
      C:
        a -> b

      D:
        F(a) -> F(b)
        |       |
        v       v
        G(a) -> G(b)

     *)
    
    module NaturalTransformationLawsInTake =

        let ``FuncyListNT.Take is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.array(Arb.int).NonNull, Arb.int)(fun f a l ->
            let Fa = FuncyList.Construct(a)
            Fa.Take(l).FMap(f) = Fa.FMap(f).Take(l)
        )

        let ``NaturalTransformation laws`` = property {
            apply ``FuncyListNT.Take is natural``
        }

    module NaturalTransformationLawsInTakeFirst =

        let ``FuncyListNT.TakeFirst is natural`` = Prop.forAll(Arb.systemFunc(CoArb.int, Arb.int), Arb.array(Arb.int).NonNull)(fun f a ->
            let Fa = FuncyList.Construct(a)
            Fa.TakeFirst().FMap(f) = Fa.FMap(f).TakeFirst()
        )

        let ``NaturalTransformation laws`` = property {
            apply ``FuncyListNT.TakeFirst is natural``
        }