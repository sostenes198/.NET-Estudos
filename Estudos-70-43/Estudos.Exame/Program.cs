using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.Exame.GerenciaFluxoPrograma.Threads.CancelandoTasks;
using Estudos.Exame.GerenciaFluxoPrograma.Threads.SincronizacaoDeRecursos;

namespace Estudos.Exame
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Method";


            CancelationTokenLancandoExcecaoEstudo.CancelationTokenDanger();
            
            Console.WriteLine("Digite qualquer coisa pra sair");
            Console.ReadKey();
        }
    }
}