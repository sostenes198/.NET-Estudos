using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Estudos.Exame.Capitulo2.SystemReflection.FindComponentssInAssemblies
{
    public class FindComponentsTest
    {
        public static void FindComponents()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var accountTypes = new List<Type>();
            foreach (var type in thisAssembly.GetTypes())
            {
                if (type.IsInterface)
                    continue;
                
                if(typeof(IAccount).IsAssignableFrom(type))
                    accountTypes.Add(type);
            }

            foreach (var type in accountTypes)
                Console.WriteLine(type.Name);
        }
        
        public static void FindComponentsLink()
        {
            var accountTypes = from type in Assembly.GetExecutingAssembly().GetTypes()
                where typeof(IAccount).IsAssignableFrom(type) && type.IsInterface == false
                select type;

            foreach (var type in accountTypes)
                Console.WriteLine(type.Name);
        }
    }
}