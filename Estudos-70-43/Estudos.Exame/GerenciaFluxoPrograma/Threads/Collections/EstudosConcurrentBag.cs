using System;
using System.Collections.Concurrent;

namespace Estudos.Exame.GerenciaFluxoPrograma.Threads.Collections
{
    public class EstudosConcurrentBag
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