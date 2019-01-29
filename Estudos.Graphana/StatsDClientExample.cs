using System;
using System.Threading;

namespace Estudos.Graphana
{
    public static class StatsDClientExample
    {
        public static void Run()
        {
            Console.WriteLine("Sending metrics to StatsD");

            while(true)
            {
                for (int i = 0; i < 10; i++)
                {
                    var statsDClient = new StatsDClient("127.0.0.1");
           
                    statsDClient.Send("shoehub.test:1|C");
                }
                Thread.Sleep(500);
                Console.WriteLine("Metric was sent!");
            }
        }
    }
}