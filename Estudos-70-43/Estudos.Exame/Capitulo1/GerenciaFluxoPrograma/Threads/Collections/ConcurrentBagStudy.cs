using System;
using System.Collections.Concurrent;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Collections
{
    public class ConcurrentBagStudy
    {
        public static void ConcurrentBagTest()
        {
            var bag = new ConcurrentBag<string>();
            bag.Add("Rob");
            bag.Add("Miles");
            bag.Add("Hull");
            string str;

            if (bag.TryPeek(out str))
                Console.WriteLine($"Peek: {str}");
            
            if(bag.TryTake(out str))
                Console.WriteLine($"Take: {str}");
        }
    }
}