using System;

namespace Estudos.Exame.Capitulo2.ConvertTypes
{
    public class TestConverType
    {
        public static void Test()
        {
            Miles miles = new Miles(100);
            Kilometers kilometers = miles; // implicity operator Miles to Kilometers
            Console.WriteLine($"Kilometers: {kilometers.Distance}");

            int intMiles = (int)miles; // explicitly operator Miles to int
            Console.WriteLine($"Int Miles: {intMiles}");
        }
    }
}