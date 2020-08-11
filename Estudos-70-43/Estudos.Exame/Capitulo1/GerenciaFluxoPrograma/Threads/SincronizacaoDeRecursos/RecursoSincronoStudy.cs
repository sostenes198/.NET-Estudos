using System;
using System.Linq;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.SincronizacaoDeRecursos
{
    public class RecursoSincronoStudy
    {
        private static int[] items = Enumerable.Range(0, 5001).ToArray();

        public static void RecursoSincronoTeste()
        {
            long total = 0;
            for (int i = 0; i < items.Length; i++)
            {
                total = total + items[i];
            }
            
            Console.WriteLine($"Total is {total}");
        }
    }
}