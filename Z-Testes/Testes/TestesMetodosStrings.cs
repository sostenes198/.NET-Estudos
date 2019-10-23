using System;
using System.Diagnostics;

namespace Testes
{
    public static class TestesMetodosStrings
    {
        
        public static bool TesteStringIsNullOrEmpty(string stringTeste)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = string.IsNullOrEmpty(stringTeste);
            stopwatch.Stop();
            
            Console.WriteLine($"IsNullOrEmpty: {stopwatch.Elapsed}");

            return result;
        }
        
        public static bool IsNullOrWhiteSpace(string stringTeste)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = string.IsNullOrWhiteSpace(stringTeste);
            stopwatch.Stop();
            
            Console.WriteLine($"IsNullOrWhiteSpace: {stopwatch.Elapsed}");
            
            return result;
        }
        
        public static bool SmartStringIsNullOrEmpty(string stringTeste)
        {
            var stopwatch = new Stopwatch();
            bool result;
            stopwatch.Start();
            try
            {
                var valor = stringTeste[0];
                result = false;
            }
            catch
            {
                result = true;
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"SmartStringIsNullOrEmpty: {stopwatch.Elapsed}");
            }
            
            return result;
        }
    }
}