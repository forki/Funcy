﻿using Funcy.Computations;
using Funcy.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcy
{
    public abstract class FuncyList<T> : IStructuralEquatable, IStructuralComparable, IEnumerable<T>, IComputable<T>
    {
        public static FuncyList<T> Cons(T head, FuncyList<T> tail)
        {
            return new Cons<T>(head, tail);
        }

        public static FuncyList<T> Nil()
        {
            return new Nil<T>();
        }

        public static FuncyList<T> Construct(params T[] args)
        {
            FuncyList<T> result = FuncyList<T>.Nil();

            for (int i = args.Length - 1; i >= 0; i--)
            {
                result = FuncyList<T>.Cons(args[i], result);
            }

            return result;
        }

        public Cons<T> ToCons()
        {
            return (Cons<T>)this;
        }

        public Nil<T> ToNil()
        {
            return (Nil<T>)this;
        }

        public abstract bool IsCons { get; }
        public abstract bool IsNil { get; }

        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer)
        {
            return this.Equals(other, comparer);
        }
        public abstract bool Equals(object other, System.Collections.IEqualityComparer comparer);

        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return this.GetHashCode(comparer);
        }
        public abstract int GetHashCode(System.Collections.IEqualityComparer comparer);

        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer)
        {
            return this.CompareTo(other, comparer);
        }
        public abstract int CompareTo(object other, System.Collections.IComparer comparer);

        IFunctor<TReturn> IFunctor<T>.FMap<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap(f);
        }
        public abstract FuncyList<TReturn> FMap<TReturn>(Func<T, TReturn> f);
        
        IApplicative<TReturn> IApplicative<T>.Apply<TReturn>(IApplicative<Func<T, TReturn>> f)
        {
            return this.Apply<TReturn>((FuncyList<Func<T, TReturn>>)f);
        }
        public FuncyList<TReturn> Apply<TReturn>(FuncyList<Func<T, TReturn>> f)
        {
            if (f.IsCons)
            {
                return FuncyList<TReturn>.Construct(f.ToList().SelectMany(fCons => this.FMap(fCons)).ToArray());
            }
            else
            {
                return FuncyList<TReturn>.Nil();
            }
        }

        IApplicative<T> IApplicative<T>.ApplyLeft<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyLeft((FuncyList<TReturn>)other);
        }
        public FuncyList<T> ApplyLeft<TReturn>(FuncyList<TReturn> other)
        {
            return this;
        }

        IApplicative<TReturn> IApplicative<T>.ApplyRight<TReturn>(IApplicative<TReturn> other)
        {
            return this.ApplyRight<TReturn>((FuncyList<TReturn>)other);
        }
        public FuncyList<TReturn> ApplyRight<TReturn>(FuncyList<TReturn> other)
        {
            return other;
        }

        IApplicative<T> IApplicative<T>.Point(T value)
        {
            return this.Point(value);
        }
        public FuncyList<T> Point(T value)
        {
            return FuncyList<T>.Construct(value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.IsCons)
            {
                var target = this;
                do
                {
                    var cons = target.ToCons();
                    yield return cons.Head;
                    target = cons.Tail;
                } while (target.IsCons);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [Obsolete("This method is deprecated. Use FMap method.")]
        IComputable<TReturn> IComputable<T>.Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.Compute<TReturn>(f);
        }
        [Obsolete("This method is deprecated. Use FMap method.")]
        public FuncyList<TReturn> Compute<TReturn>(Func<T, TReturn> f)
        {
            return this.FMap<TReturn>(f);
        }

        IComputable<TReturn> IComputable<T>.ComputeWith<TReturn>(Func<T, IComputable<TReturn>> f)
        {
            return this.ComputeWith(x => (FuncyList<TReturn>)f(x));
        }
        public abstract FuncyList<TReturn> ComputeWith<TReturn>(Func<T, FuncyList<TReturn>> f);
    }

    public class Cons<T> : FuncyList<T>, IExtractor<Tuple<T, FuncyList<T>>>
    {
        private T head;
        public T Head
        {
            get { return this.head; }
        }

        private FuncyList<T> tail;
        public FuncyList<T> Tail
        {
            get { return this.tail; }
        }

        public Cons(T head, FuncyList<T> tail)
        {
            this.head = head;
            this.tail = tail;
        }

        public override bool IsCons { get { return true; } }

        public override bool IsNil { get { return false; } }

        public override bool Equals(object obj, System.Collections.IEqualityComparer comparer)
        {
            if (obj == null) return false;
            if (obj is Cons<T>)
            {
                var other = (Cons<T>)obj;
                return comparer.Equals(this.head, other.head) && this.tail.Equals(other.tail, comparer);
            }
            else
            {
                return false;
            }

        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<T>.Default);
        }

        public override int GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            int hash = 17;
            hash = hash * 31 + comparer.GetHashCode(this.head);
            hash = hash * 31 + this.tail.GetHashCode(comparer);
            return hash;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, System.Collections.IComparer comparer)
        {
            if (other == null) return 0;
            if (other is Cons<T>)
            {
                var cons = (Cons<T>)other;
                int headComparisonResult = comparer.Compare(this.head, cons.head);
                if (headComparisonResult != 0)
                {
                    return headComparisonResult;
                }
                else
                {
                    return this.tail.CompareTo(cons.tail, comparer);
                }
            }
            if (other is Nil<T>)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override FuncyList<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return FuncyList<TReturn>.Cons(f(this.head), this.tail.FMap(f));
        }

        public override FuncyList<TReturn> ComputeWith<TReturn>(Func<T, FuncyList<TReturn>> f)
        {
            return FuncyList<TReturn>.Construct(this.SelectMany(h => f(h)).ToArray());
        }

        public Tuple<T, FuncyList<T>> Extract()
        {
            return Tuple.Create<T, FuncyList<T>>(this.head, this.tail);
        }
    }

    public class Nil<T> : FuncyList<T>
    {
        public Nil() { }

        public override bool IsCons { get { return false; } }

        public override bool IsNil { get { return true; } }

        public override bool Equals(object obj, System.Collections.IEqualityComparer comparer)
        {
            if (obj == null) return false;
            if (obj is Nil<T>)
            {
                var nil = (Nil<T>)obj;
                return comparer.Equals(this.GetType().DeclaringType, nil.GetType().DeclaringType);
            }
            else
            {
                return false;
            }
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj, EqualityComparer<T>.Default);
        }


        public override int GetHashCode(System.Collections.IEqualityComparer comparer)
        {
            return comparer.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(EqualityComparer<T>.Default);
        }

        public override int CompareTo(object other, System.Collections.IComparer comparer)
        {
            if (other == null) return 0;
            if (other is Cons<T>)
            {
                return -1;
            }
            else if (other is Nil<T>)
            {
                var nil = (Nil<T>)other;
                return comparer.Compare(this.GetType().DeclaringType, nil.GetType().DeclaringType);
            }
            else
            {
                return comparer.Compare(this, other);
            }
        }

        public override FuncyList<TReturn> FMap<TReturn>(Func<T, TReturn> f)
        {
            return FuncyList<TReturn>.Nil();
        }

        public override FuncyList<TReturn> ComputeWith<TReturn>(Func<T, FuncyList<TReturn>> f)
        {
            return FuncyList<TReturn>.Nil();
        }
    }

    public static class FuncyListNT
    {
        public static Maybe<T> ElementAt<T>(this FuncyList<T> source, int index)
        {
            if (source is Nil<T>)
            {
                return Maybe<T>.None();
            }

            if (index < 0)
            {
                return Maybe<T>.None();
            }

            if (index >= source.Count())
            {
                return Maybe<T>.None();
            }

            return Maybe<T>.Some(((IEnumerable<T>)source).ElementAt(index));
        }

        public static Maybe<T> Last<T>(this FuncyList<T> source)
        {
            if (source.IsNil)
            {
                return Maybe<T>.None();
            }
            else
            {
                return Maybe<T>.Some(((IEnumerable<T>)source).Last());
            }
        }

        public static FuncyList<T> Take<T>(this FuncyList<T> source, int count)
        {
            if (source.IsNil || count <= 0)
            {
                return FuncyList<T>.Nil();
            }

            return FuncyList<T>.Construct(((IEnumerable<T>)source).Take<T>(count).ToArray());
        }

        public static Maybe<T> First<T>(this FuncyList<T> source)
        {
            if (source.IsNil)
            {
                return Maybe<T>.None();
            }
            else
            {
                return Maybe<T>.Some(source.ToCons().Head);
            }
        }
    }

}
