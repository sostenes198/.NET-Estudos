using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Estudos.Global.Atributos;

namespace Estudos.Global.Helpers
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Type> ObterEntidadesAssemblyAbstrato(string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
                return new Stack<Type>();

            return Assembly.Load(nameSpace)
                .GetTypes()
                .Where(lnq => lnq.IsInterface && lnq.IsAbstract);
        }

        public static IEnumerable<Type> ObterEntidadeAssemblyImplementacao(string nameSpace)
        {
            if (string.IsNullOrEmpty(nameSpace))
                return new Stack<Type>();

            return Assembly.Load(nameSpace.Trim()).GetTypes()
                .Where(lnq => lnq.IsClass && !lnq.IsAbstract
                                          && lnq.GetInterfaces().Any() && lnq.GetCustomAttributes(typeof(IoCAttribute), true).Any());
        }


        //public static IEnumerable<Type> ObterEntidadesAssemblyAbstrato<T>(string nameSpace)
        //  where T : class
        //{
        //    return Assembly.Load(nameSpace)
        //        .GetTypes()
        //        .Where(lnq => lnq.IsInterface && lnq.IsAbstract
        //                && typeof(T).IsAssignableFrom(lnq) && lnq.GetInterfaces().Any());

        //}

        //public static IEnumerable<Type> ObterEntidadeAssemblyImplementacao<T>(string nameSpace)
        //    where T : class
        //{
        //    return Assembly.Load(nameSpace).GetTypes()
        //        .Where(lnq => lnq.IsClass && !lnq.IsAbstract
        //                && typeof(T).IsAssignableFrom(lnq) && lnq.GetInterfaces().Any()
        //                && lnq.GetCustomAttributes(typeof(IoCAttribute), true).Any());
        //}
    }
}