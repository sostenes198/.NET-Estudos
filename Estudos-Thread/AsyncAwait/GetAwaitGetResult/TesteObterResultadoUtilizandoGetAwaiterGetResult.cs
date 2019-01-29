using System;
using System.Threading.Tasks;

namespace AsyncAwait.GetAwaitGetResult
{
    public class TesteObterResultadoUtilizandoGetAwaiterGetResult
    {
        public static void ObterResultado()
        {
            try
            {
                GerarResultado().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task<bool> GerarResultado()
        {
            await LancarExecao();
            return await Task.Run(() => true);
        }

        private static Task LancarExecao()
        {
            return Task.Run(() => throw new Exception("Mensagem de erro"));
        }
    }
}