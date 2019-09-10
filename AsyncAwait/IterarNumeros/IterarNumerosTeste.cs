using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncAwait.IterarNumeros
{
    public class IterarNumerosTeste
    {
        public static async Task TesteITerarNumeros()
        {
            var tempoAsync = await TesteIterarAsync();
            var temporAsyncSemEsperarRetorno = await TesteIterarAsyncSemCriarNovaTaskAsync();
            var tempoSync = TesteIterarSync();
            var tempoAsyncContinueAwait = TesteIterarASyncConfigureAwait();
            Console.WriteLine($"Tempo para processar 20 numeros async                         : {tempoAsync}");
            Console.WriteLine($"Tempo para processar 20 numeros async sem criar duas tasks    : {temporAsyncSemEsperarRetorno}");
            Console.WriteLine($"Tempo para processar 20 numeros sync                          : {tempoSync}");
            Console.WriteLine($"Tempo para processar 20 numeros async configuere await false  : {tempoAsyncContinueAwait}");
        }

        private static async Task<TimeSpan> TesteIterarAsync()
        {
            Console.WriteLine($"Thread Corrente Iterar Numero Async: {Task.CurrentId}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await TesteIterarNumeroAsync.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        private static async Task<TimeSpan> TesteIterarAsyncSemCriarNovaTaskAsync()
        {
            Console.WriteLine($"Thread Corrente Sem Criar Nova Task: {Task.CurrentId}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await TesteIterNumeroAsyncSemCriarNovaTask.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        private static TimeSpan TesteIterarSync()
        {
            Console.WriteLine($"Thread Corrente Iterar número sync: {Task.CurrentId}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TesteIterarNumeroSync.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        private static TimeSpan TesteIterarASyncConfigureAwait()
        {
            Console.WriteLine($"Thread Corrente Iterar número Configure Await: {Task.CurrentId}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TesteIterarNumeroAsyncConfigureAwait.InterarNumero();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}