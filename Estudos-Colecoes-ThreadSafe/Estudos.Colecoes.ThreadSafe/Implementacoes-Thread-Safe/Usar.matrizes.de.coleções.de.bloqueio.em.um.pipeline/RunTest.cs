using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Colecoes.ThreadSafe.Implementacoes_Thread_Safe.Usar.matrizes.de.coleções.de.bloqueio.em.um.pipeline
{
    public static class RunTest
    {
        public static void Run()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            // Start up a UI thread for cancellation.
            Task.Run(() =>
            {
                if (Console.ReadKey(true).KeyChar == 'c')
                    cts.Cancel();
            });

            //Generate some source data.
            BlockingCollection<int>[] sourceArrays = new BlockingCollection<int>[5];
            for (int i = 0; i < sourceArrays.Length; i++)
                sourceArrays[i] = new BlockingCollection<int>(500);
            Parallel.For(0, sourceArrays.Length * 500, (j) =>
            {
                int k = BlockingCollection<int>.TryAddToAny(sourceArrays, j);
                if (k >= 0)
                    Console.WriteLine("added {0} to source data", j);
            });

            foreach (var arr in sourceArrays)
                arr.CompleteAdding();

            // First filter accepts the ints, keeps back a small percentage
            // as a processing fee, and converts the results to decimals.
            var filter1 = new PipelineFilter<int, decimal>
            (
                sourceArrays,
                (n) => Convert.ToDecimal(n * 0.97),
                cts.Token,
                "filter1"
            );

            // Second filter accepts the decimals and converts them to
            // System.Strings.
            var filter2 = new PipelineFilter<decimal, string>
            (
                filter1.MOutput,
                (s) => String.Format("{0}", s),
                cts.Token,
                "filter2"
            );

            // Third filter uses the constructor with an Action<T>
            // that renders its output to the screen,
            // not a blocking collection.
            var filter3 = new PipelineFilter<string, string>
            (
                filter2.MOutput,
                (s) => Console.WriteLine("The final result is {0}", s),
                cts.Token,
                "filter3"
            );

            // Start up the pipeline!
            try
            {
                Parallel.Invoke(
                    () => filter1.Run(),
                    () => filter2.Run(),
                    () => filter3.Run()
                );
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                    Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                cts.Dispose();
            }

            // You will need to press twice if you ran to the end:
            // once for the cancellation thread, and once for this thread.
            Console.WriteLine("Press any key.");
            Console.ReadKey(true);
        }
    }
}