using System;
using System.Threading.Tasks;

namespace AsyncAwait.ConfigureAwait
{
    public class TesteConfigureAwaitTryCatch
    {
        public static void ObterResultado()
        {
            GerarResultadoAsync().ConfigureAwait(false);
        }

        private static async Task GerarResultadoAsync()
        {
            try
            {
                await GerarResultadoAsync1();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Task GerarResultadoAsync1()
        {
            return Task.Run(() => throw new Exception("Mensagem de Erro"));
        }
    }
}