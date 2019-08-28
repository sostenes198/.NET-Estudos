using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncAwait.IterarNumeros;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tempoAsync = await TesteIterarAsync();
            var tempoSync = TesteIterarSync();
            var tempoAsyncContinueAwait = TesteIterarASyncContinueAwait();
            Console.WriteLine($"Tempo para processar 20 numeros async                         : {tempoAsync}");
            Console.WriteLine($"Tempo para processar 20 numeros sync                          : {tempoSync}");
            Console.WriteLine($"Tempo para processar 20 numeros async configuere await false  : {tempoAsyncContinueAwait}");
        }

        private static async Task<TimeSpan> TesteIterarAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await TesteIterarNumeroAsync.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        private static TimeSpan TesteIterarSync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TesteIterarNumeroSync.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        private static TimeSpan TesteIterarASyncContinueAwait()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TesteIterarNumeroAsyncContiueWith.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}