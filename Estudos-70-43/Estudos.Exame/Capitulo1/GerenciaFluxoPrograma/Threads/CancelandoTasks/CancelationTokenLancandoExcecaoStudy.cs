using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.CancelandoTasks
{
    public class CancelationTokenLancandoExcecaoStudy
    {
        private static void Clock(CancellationToken token)
        {
            int tickCount = 0;
            while (token.IsCancellationRequested == false && tickCount < 20)
            {
                tickCount++;
                Console.WriteLine("Tick");
                Thread.Sleep(500);
            }

            token.ThrowIfCancellationRequested();
        }

        public static void CancellationTokenTeste()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var clockTsk = Task.Run(() => Clock(cancellationTokenSource.Token));
            Console.WriteLine("Pressione qualquer tecla para pausar o Clock");
            Console.ReadKey();

            if (clockTsk.IsCompleted)
                Console.WriteLine("Clock foi completo com sucesso");
            else
            {
                try
                {
                    cancellationTokenSource.Cancel();
                    clockTsk.Wait();
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine($"Clock stopped {ex.InnerExceptions[0]}");
                }
            }
        }

        public static void CancelationTokenDanger()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var clock = Task.Run(() => Clock(cancellationTokenSource.Token));
            
            Console.WriteLine("Press any key to leave");

            if (clock.IsCompleted)
                Console.WriteLine("Clock task is completed");
            else
            {
                try
                {
                    cancellationTokenSource.Cancel();
                    clock.Wait();
                }
                catch(AggregateException ex)
                {
                    Console.WriteLine($"Clock stopped: {ex.InnerExceptions[0]}");
                }
            }
        }
        
    }
}