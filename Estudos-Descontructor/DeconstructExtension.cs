using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Descontructor
{
    public static class DeconstructExtension
    {
        public static void Deconstruct<T>(this T[] array, out T first, out T[] rest)
        {
            first = array.Length > 0 ? array[0] : default;
            rest = array.Skip(1).ToArray();
        }

        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T[] rest)
        {
            (first, (second, rest)) = array;
        }

        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T[] rest)
        {
            (first, second, (third, rest)) = array;
        }

        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T[] rest)
        {
            (first, second, third, (fourth, rest)) = array;
        }

        public static void Deconstruct<T>(this T[] array, out T first, out T second, out T third, out T fourth, out T fifth, out T[] rest)
        {
            (first, second, third, fourth, (fifth, rest)) = array;
        }


        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out IEnumerable<T> rest)
        {
            var enumerable = seq as T[] ?? seq.ToArray();
            first = enumerable.FirstOrDefault();
            rest = enumerable.Skip(1);
        }

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out IEnumerable<T> rest)
        {
            (first, (second, rest)) = seq;
        }

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out IEnumerable<T> rest)
        {
            (first, second, (third, rest)) = seq;
        }

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out IEnumerable<T> rest)
        {
            (first, second, third, (fourth, rest)) = seq;
        }

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out T fifth, out IEnumerable<T> rest)
        {
            (first, second, third, fourth, (fifth, rest)) = seq;
        }

        public static void Deconstruct(this PropertyInfo p, out bool isStatic,
            out bool isReadOnly, out bool isIndexed,
            out Type propertyType)
        {
            var getter = p.GetMethod;

            // Is the property read-only?
            isReadOnly = !p.CanWrite;

            // Is the property instance or static?
            isStatic = getter.IsStatic;

            // Is the property indexed?
            isIndexed = p.GetIndexParameters().Length > 0;

            // Get the property type.
            propertyType = p.PropertyType;
        }

        public static void Deconstruct(this PropertyInfo p, out bool hasGetAndSet,
            out bool sameAccess, out string access,
            out string getAccess, out string setAccess)
        {
            hasGetAndSet = sameAccess = false;
            string getAccessTemp = null;
            string setAccessTemp = null;

            MethodInfo getter = null;
            if (p.CanRead)
                getter = p.GetMethod;

            MethodInfo setter = null;
            if (p.CanWrite)
                setter = p.SetMethod;

            if (setter != null && getter != null)
                hasGetAndSet = true;

            if (getter != null)
            {
                if (getter.IsPublic)
                    getAccessTemp = "public";
                else if (getter.IsPrivate)
                    getAccessTemp = "private";
                else if (getter.IsAssembly)
                    getAccessTemp = "internal";
                else if (getter.IsFamily)
                    getAccessTemp = "protected";
                else if (getter.IsFamilyOrAssembly)
                    getAccessTemp = "protected internal";
            }

            if (setter != null)
            {
                if (setter.IsPublic)
                    setAccessTemp = "public";
                else if (setter.IsPrivate)
                    setAccessTemp = "private";
                else if (setter.IsAssembly)
                    setAccessTemp = "internal";
                else if (setter.IsFamily)
                    setAccessTemp = "protected";
                else if (setter.IsFamilyOrAssembly)
                    setAccessTemp = "protected internal";
            }

            // Are the accessibility of the getter and setter the same?
            if (setAccessTemp == getAccessTemp)
            {
                sameAccess = true;
                access = getAccessTemp;
                getAccess = setAccess = string.Empty;
            }
            else
            {
                access = null;
                getAccess = getAccessTemp;
                setAccess = setAccessTemp;
            }
        }
    }
}