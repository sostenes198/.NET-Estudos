using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.AsyncAwait.ImpSynchronizationContext
{
    public static class Demonstration
    {

        public static void RunAsync()
        {
            AsyncPump.Run(async delegate

            {
                await DemoAsync();
            });
        }
        
        public static async Task DemoAsync()

        {

            var d = new Dictionary<int, int>();

            for (int i = 0; i < 10; i++)

            {

                int id = Thread.CurrentThread.ManagedThreadId;

                int count;

                d[id] = d.TryGetValue(id, out count) ? count+1 : 1;

 

                await Task.Yield();

            }

            foreach (var pair in d) Console.WriteLine(pair);

        }
    }
}