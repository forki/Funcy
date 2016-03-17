﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcy.Computations
{
    public interface IApplicativeTC<TApplicative, TSource> : IFunctorTC<TApplicative, TSource> where TApplicative : IPointed<TApplicative>
    {
        IApplicativeTC<TApplicative, TReturn> Apply<TReturn>(IApplicativeTC<TApplicative, Func<TSource, TReturn>> f);
        IApplicativeTC<TApplicative, TSource> ApplyLeft<TReturn>(IApplicativeTC<TApplicative, TReturn> other);
        IApplicativeTC<TApplicative, TReturn> ApplyRight<TReturn>(IApplicativeTC<TApplicative, TReturn> other);
        IApplicativeTC<TApplicative, TReturn> FMapA<TReturn>(Func<TSource, TReturn> f);
    }
}
