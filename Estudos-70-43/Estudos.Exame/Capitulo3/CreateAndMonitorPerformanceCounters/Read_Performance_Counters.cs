using System;
using System.Diagnostics;
using System.Threading;

namespace Estudos.Exame.Capitulo3.CreateAndMonitorPerformanceCounters
{
    public class Read_Performance_Counters
    {
        public static void ProcessoTime()
        {
            var processor = new PerformanceCounter(
                "Processor information",
                "% Processor Time",
                "_Total");

            while (true)
            {
                Console.WriteLine($"Processor time {processor.NextValue()}");
                Thread.Sleep(500);
                if(Console.KeyAvailable)
                    break;
            }
        }
    }
}