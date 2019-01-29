using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Estudos.Tests.Mutation
{
    public abstract class Value<T> where T : Value<T>
    {
        static readonly Member[] Members = GetMembers().ToArray();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(T) && Members.All(
                m =>
                {
                    var otherValue = m.GetValue(obj);
                    var thisValue = m.GetValue(this);
                    return m.IsNonStringEnumerable
                    ? GetEnumerablesValues(otherValue).SequenceEqual(GetEnumerablesValues(thisValue))
                    : otherValue?.Equals(thisValue) ?? thisValue == null;
                }
                );
        }

        public override int GetHashCode() => CombineHashCodes(Members.Select(
            m => m.IsNonStringEnumerable 
            ? CombineHashCodes(GetEnumerablesValues(m.GetValue(this))) 
            : m.GetValue(this)
            ));
        

        private static IEnumerable<Member> GetMembers()
        {
            var t = typeof(T);
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
            while (t != typeof(object))
            {
                if (t == null) continue;
                foreach (var p in t.GetProperties(flags))
                {
                    yield return new Member(p);
                }
                foreach (var f in t.GetFields(flags))
                {
                    yield return new Member(f);
                }

                t = t.BaseType;
            }
        }

        static IEnumerable<object> GetEnumerablesValues(object obj)
        {
            var enumerator = ((IEnumerable)obj).GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        static int CombineHashCodes(IEnumerable<object> objs)
        {
            unchecked
            {
                return objs.Aggregate(17, (current, obj) => current * 59 + (obj?.GetHashCode() ?? 0));
            }
        }

        public static bool operator ==(Value<T> left, Value<T> right) => Equals(left, right);
        public static bool operator !=(Value<T> left, Value<T> right) => !Equals(left, right);

        struct Member
        {
            #region Attributes

            public readonly string Name;
            public readonly Func<object, object> GetValue;
            public readonly bool IsNonStringEnumerable;
            public readonly Type Type;

            #endregion

            public Member(MemberInfo info)
            {
                switch (info)
                {
                    case FieldInfo field:
                        Name = field.Name;
                        GetValue = obj => field.GetValue(obj);
                        IsNonStringEnumerable = typeof(IEnumerable).IsAssignableFrom(field.FieldType) && field.FieldType != typeof(string);
                        Type = field.FieldType;
                        break;
                    case PropertyInfo prop:
                        Name = prop.Name;
                        GetValue = obj => prop.GetValue(obj);
                        IsNonStringEnumerable = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string);
                        Type = prop.PropertyType;
                        break;
                    default:
                        throw new ArgumentException("Membro não é um field ou property?", info.Name);
                }
            }
        }
    }
}
