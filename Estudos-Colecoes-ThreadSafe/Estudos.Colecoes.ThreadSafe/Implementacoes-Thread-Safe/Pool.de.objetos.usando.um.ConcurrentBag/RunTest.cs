using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Colecoes.ThreadSafe.Implementacoes_Thread_Safe.Pool.de.objetos.usando.um.ConcurrentBag
{
    public static class RunTest
    {
        public static void Run()
        {
            using var cts = new CancellationTokenSource();

            // Create an opportunity for the user to cancel.
            _ = Task.Run(() =>
            {
                if (char.ToUpperInvariant(Console.ReadKey().KeyChar) == 'C')
                {
                    cts.Cancel();
                }
            });

            var pool = new ObjectPool<ExampleObject>(() => new ExampleObject());

            // Create a high demand for ExampleObject instance.
            Parallel.For(0, 1000, (i, loopState) =>
            {
                var example = pool.Get();
                try
                {
                    Console.CursorLeft = 0;
                    // This is the bottleneck in our application. All threads in this loop
                    // must serialize their access to the static Console class.
                    Console.WriteLine($"{example.GetValue(i):####.####}");
                }
                finally
                {
                    pool.Return(example);
                }

                if (cts.Token.IsCancellationRequested)
                {
                    loopState.Stop();
                }
            });

            Console.WriteLine("Press the Enter key to exit.");
            Console.ReadLine();
        }
    }
    
    // A toy class that requires some resources to create.
    // You can experiment here to measure the performance of the
    // object pool vs. ordinary instantiation.
    class ExampleObject
    {
        public int[] Nums { get; set; }

        public ExampleObject()
        {
            Nums = new int[1000000];
            var rand = new Random();
            for (int i = 0; i < Nums.Length; i++)
            {
                Nums[i] = rand.Next();
            }
        }

        public double GetValue(long i) => Math.Sqrt(Nums[i]);
    }
}