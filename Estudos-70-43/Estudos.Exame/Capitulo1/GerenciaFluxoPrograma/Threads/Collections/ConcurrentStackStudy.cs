using System;
using System.Collections.Concurrent;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Collections
{
    public class ConcurrentStackStudy
    {
        public static void ConcurrentStackTest()
        {
            var stack = new ConcurrentStack<string>();
            stack.Push("Rob");
            stack.PushRange(new[] {"Mile", "Soso"});
            string str;
            if (stack.TryPeek(out str))
                Console.WriteLine($"Peek: {str}");
            
            if(stack.TryPop(out str))
                Console.WriteLine($"Pop: {str}");
        }
    }
}