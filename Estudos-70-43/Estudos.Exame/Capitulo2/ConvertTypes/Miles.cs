using System;

namespace Estudos.Exame.Capitulo2.ConvertTypes
{
    public class Miles
    {
        public double Distance { get; }

        public static implicit operator Kilometers(Miles miles)
        {
            Console.WriteLine("Implicit conversaion from miles to kilometers");
            return new Kilometers(miles.Distance * 1.6);
        }

        public static explicit operator int(Miles miles)
        {
            Console.WriteLine("Explicit conversion from miles to int");
            return (int) (miles.Distance + 0.5);
        }

        public Miles(double miles)
        {
            Distance = miles;
        }
    }
}