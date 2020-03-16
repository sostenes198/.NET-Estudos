using System;
using System.Threading;
using Estudos.Exame.GerenciaFluxoPrograma.Threads.Collections;
using Estudos.Exame.GerenciaFluxoPrograma.Threads.SincronizacaoDeRecursos;

namespace Estudos.Exame
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main Method";
            RecursoSincrono.RecursoSincronoTeste();
            SincronizacaoRecursosLock.SincronizarRecursosTeste();
            Console.WriteLine("Digite qualquer coisa pra sair");
            Console.ReadKey();
        }
    }
}