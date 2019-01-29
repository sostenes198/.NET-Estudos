using System;
using System.Globalization;

namespace Estudos.Exame.Capitulo2.FormatString
{
    public class FormatStringStudy
    {
        public static void Test()
        {
            var i = 99;
            var pi = 3.131592654;
            
            Console.WriteLine("{0,-10:D} {0,-10:X} {1,5:N2}", i, pi);
        }

        public static void TestMusicTrackerFormatString()
        {
            var song = new MusicTrack("Soso O Melhor", "Do meu jeito");
            Console.WriteLine($"Track: {song:F}");
            Console.WriteLine($"Artist: {song:A}");
            Console.WriteLine($"Title: {song:T}");
        }

        public static void TestFormatProvider()
        {
            var bankBalance = 123.45;
            var usProvider = new CultureInfo("en-US");
            Console.WriteLine($"US balance: {bankBalance.ToString("C", usProvider)}");
            var ukProvider = new CultureInfo("en-GB");
            Console.WriteLine($"UK balance: {bankBalance.ToString("C", ukProvider)}");
        }
    }
}