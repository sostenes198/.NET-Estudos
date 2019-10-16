using System;
using System.Threading.Tasks;

namespace AsyncAwait.ConfigureAwait
{
    public class TesteConfigureAwait
    {
        public static void ObterResultado()
        {
            GerarResultadoAsync().ConfigureAwait(false);
        }

        private static Task GerarResultadoAsync()
        {
            return Task.Run(() => Console.WriteLine("Resultado Gerado"));
        }
    }
}