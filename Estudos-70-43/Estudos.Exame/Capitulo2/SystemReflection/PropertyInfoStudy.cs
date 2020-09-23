using System;

namespace Estudos.Exame.Capitulo2.SystemReflection
{
    public class PropertyInfoStudy
    {
        public static void Test()
        {
            var type = typeof(Person);
            foreach (var propertyInfo in type.GetProperties())
            {
                Console.WriteLine($"Property Name: {propertyInfo.Name}");
                if (propertyInfo.CanRead)
                {
                    Console.WriteLine("Can Read");
                    Console.WriteLine($"Set method: {propertyInfo.GetMethod}");
                }

                if (propertyInfo.CanWrite)
                {
                    Console.WriteLine("Can Write");
                    Console.WriteLine($"Set Mehotd: {propertyInfo.SetMethod}");
                }
            }
        }
    }
}