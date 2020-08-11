using System;
using System.Collections.Concurrent;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Collections
{
    public class ConcurrentDictionaryStudy
    {
        public static void ConcurrentDicionaryTest()
        {
            var ages = new ConcurrentDictionary<string, int>();
            if (ages.TryAdd("Rob", 21))
                Console.WriteLine("Rob addes successfully");
            Console.WriteLine($"Rob ages {ages["Rob"]}");
            if (ages.TryUpdate("Rob", 22, 21))
                Console.WriteLine("Age updated successfully");
            Console.WriteLine($"Rob's new age {ages["Rob"]}");
            Console.WriteLine($"Rob's age updated to {ages.AddOrUpdate("Rob", 1, (name, age) => age += 1)}");
            Console.WriteLine($"Rob's new age {ages["Rob"]}");
        }
    }
}