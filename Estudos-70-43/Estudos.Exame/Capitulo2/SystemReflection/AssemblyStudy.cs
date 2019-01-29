using System;
using System.Reflection;

namespace Estudos.Exame.Capitulo2.SystemReflection
{
    public class AssemblyStudy
    {
        public static void Test()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Full name: {assembly.FullName}");
            var name = assembly.GetName();
            Console.WriteLine($"Major Version: {name.Version.Major}");
            Console.WriteLine($"Minor Version: {name.Version.Minor}");
            Console.WriteLine($"In global assembly cache: {assembly.GlobalAssemblyCache}");
            foreach (var assemblyModule in assembly.Modules)
            {
                Console.WriteLine($"Module: {assemblyModule.Name}");
                foreach (var moduleType in assemblyModule.GetTypes())
                {
                    Console.WriteLine($"Type: {moduleType.Name}");
                    foreach (var memberInfo in moduleType.GetMembers())
                    {
                        Console.WriteLine($"Member: {memberInfo.Name}");
                    }
                }
            }
        }
    }
}