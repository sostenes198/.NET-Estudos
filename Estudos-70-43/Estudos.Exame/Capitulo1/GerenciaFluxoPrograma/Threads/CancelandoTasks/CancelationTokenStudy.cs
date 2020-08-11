using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.CancelandoTasks
{
    public class CancelationTokenStudy
    {
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private static void Clock()
        {
            while (_cancellationTokenSource.IsCancellationRequested == false)
            {
                Console.WriteLine("Tick");
                Thread.Sleep(500);
            }
        }

        public static void CancellationTokenTeste()
        {
            Task.Run(Clock);
            Console.WriteLine("Pressoine qualquer tecla para pausar o tick");
            Console.ReadKey();
            _cancellationTokenSource.Cancel();
            Console.WriteLine("Tick pausado");
        }
    }
}