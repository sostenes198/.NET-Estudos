using System;
using System.Diagnostics;
using System.Reflection;

namespace Estudos.Exame.Capitulo2.SystemReflection
{
    public class MethodInfoStudy
    {
        public static void Test()
        {
            Console.WriteLine("Get type information for the Person class");
            var type = typeof(Person);
            
            Console.WriteLine("Get the method infor for the CreatePerson method");
            var createPersonMethodInfo = type.GetMethod("CreatePerson");
            
            Console.WriteLine("GTet the IL instructions for the CreatePerson method");
            var createPersonBody = createPersonMethodInfo.GetMethodBody();
            
            Console.WriteLine("Print IL instructions");
            foreach (var b in createPersonBody.GetILAsByteArray())
            {
                Console.Write("{0:X}", b);
            }
            Console.WriteLine();
            
            Console.WriteLine("Create paramater array for the method");
            var input = new object[] {"Soso"};
            
            var person = new Person();
            Console.WriteLine("Call Invoke on the method info");
            Console.WriteLine("Cast the result to an Person");
            var result = (Person) createPersonMethodInfo.Invoke(person, input);
            Console.WriteLine($"Name of person: {result.Name}");
            
            Console.WriteLine("Call InvokeMember on the type");
            var result2 = (Person) type.InvokeMember("CreatePerson",
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                null,
                person,
                input);
            Console.WriteLine($"Name of person2: {result2.Name}");
        }
    }
}