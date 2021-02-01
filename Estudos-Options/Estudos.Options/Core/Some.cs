using System;
using System.Collections.Generic;

namespace Estudos.Options.Core
{
    public sealed class Some<T> : Option<T>, IEquatable<Some<T>>
    {
        public T Content { get; }

        public Some(T value)
        {
            Content = value;
        }
        
        public static implicit operator Some<T>(T value) =>
            new Some<T>(value);

        public static implicit operator T(Some<T> some) => some.Content;

        public override Option<TResult> Map<TResult>(Func<T, TResult> map) => map(Content);

        public override Option<TResult> MapOptional<TResult>(Func<T, Option<TResult>> map) => map(Content);
        public override T Reduce(T whenNone) => Content;

        public override T Reduce(Func<T> whenNone) => whenNone();

        public bool Equals(Some<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Content, other.Content);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Some<T> && Equals((Some<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Content);
        }

        public static bool operator ==(Some<T> a, Some<T> b) =>
            (a is null && b is null) ||
            (!(a is null) && a.Equals(b));

        public static bool operator !=(Some<T> a, Some<T> b) => !(a == b);
    }
}