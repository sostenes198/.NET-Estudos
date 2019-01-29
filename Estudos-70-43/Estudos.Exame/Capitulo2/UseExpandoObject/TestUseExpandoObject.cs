using System;
using System.Dynamic;

namespace Estudos.Exame.Capitulo2.UseExpandoObject
{
    public class TestUseExpandoObject
    {
        public static void Test()
        {
            dynamic person = new ExpandoObject();
            person.Name = "Soso";
            person.Age = 25;
            Console.WriteLine($"Name: {person.Name} Age: {person.Age}");
        }
    }
}