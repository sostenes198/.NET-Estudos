using Estudos.Global.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Estudos.IoC.Helpers
{
    public static class AssemblyHelper
    {

        public static IEnumerable<Type> ObterEntidadesAssemblyAbstrato<T>(string nameSpace)
          where T : class
        {
            return Assembly.Load(nameSpace)
                .GetTypes()
                .Where(lnq => lnq.IsInterface && lnq.IsAbstract
                        && typeof(T).IsAssignableFrom(lnq) && lnq.GetInterfaces().Any());

        }

        public static IEnumerable<Type> ObterEntidadeAssemblyImplementacao<T>(string nameSpace)
            where T : class
        {
            return Assembly.Load(nameSpace).GetTypes()
                .Where(lnq => lnq.IsClass && !lnq.IsAbstract
                        && typeof(T).IsAssignableFrom(lnq) && lnq.GetInterfaces().Any()
                        && lnq.GetCustomAttributes(typeof(IoCAttribute), true).Any());
        }

    }
}
