using System;
using System.Collections.Concurrent;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Collections
{
    public class ConcurrentQueueStudy
    {
        public static void ConcurrentQueueTest()
        {
            var queue = new ConcurrentQueue<string>();
            queue.Enqueue("Rob");
            queue.Enqueue("Miles");

            if (queue.TryPeek(out var str))
                Console.WriteLine($"Peek {str}");
            
            if(queue.TryDequeue(out var str1))
                Console.WriteLine($"Dequeue: {str1}");
        }
    }
}