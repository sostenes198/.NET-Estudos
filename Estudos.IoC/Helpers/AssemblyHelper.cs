using Estudos.Global.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudos.IoC.Helpers
{
    public static class AssemblyHelper
    {

        public static IEnumerable<Type> ObterEntidadesAssemblyAbstrato<T>()
          where T : class
        {
            return typeof(T).Assembly.GetExportedTypes()
                .Where(lnq => lnq.IsInterface && lnq.IsAbstract
                        && typeof(T).IsAssignableFrom(lnq) && lnq.GetInterfaces().Any());

        }

        public static IEnumerable<Type> ObterEntidadeAssemblyImplementacao<T>()
            where T : class
        {
            return typeof(T).Assembly.GetExportedTypes()
                .Where(lnq => lnq.IsClass && !lnq.IsAbstract
                        && typeof(T).IsAssignableFrom(lnq) && lnq.GetInterfaces().Any()
                        && lnq.GetCustomAttributes(typeof(IoCAttribute), true).Any());
        }

    }
}
