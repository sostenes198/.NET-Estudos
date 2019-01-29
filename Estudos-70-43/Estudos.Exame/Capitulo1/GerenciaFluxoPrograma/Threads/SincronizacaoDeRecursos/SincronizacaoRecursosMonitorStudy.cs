using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.SincronizacaoDeRecursos
{
    public class SincronizacaoRecursosMonitorStudy
    {
        private static long sharedTotal;
        private static object sharedTotalLock = new object();

        private static int[] items = Enumerable.Range(0, 5001).ToArray();

        static void AddRangerOfValues(int start, int end)
        {
            long subTotal = 0;
            while (start < end)
            {
                subTotal = subTotal + items[start];
                start++;
            }

            Monitor.Enter(sharedTotalLock);
            sharedTotal = sharedTotal + subTotal;
            Monitor.Exit(sharedTotalLock);
        }

        public static void SincronizarRecursosTeste()
        {
            var tasks = new List<Task>();
            int rangeSize = 1000;
            int rangeStart = 0;

            while (rangeStart < items.Length)
            {
                int rangeEnd = rangeStart + rangeSize;
                if (rangeEnd > items.Length)
                    rangeEnd = items.Length;

                int rs = rangeStart;
                int re = rangeEnd;
                tasks.Add(Task.Run(() => AddRangerOfValues(rs, re)));

                rangeStart = rangeEnd;
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The total is {sharedTotal}");
        }
    }
}