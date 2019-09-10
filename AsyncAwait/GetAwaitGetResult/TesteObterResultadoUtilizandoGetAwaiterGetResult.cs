using System;
using System.Threading.Tasks;

namespace AsyncAwait.GetAwaitGetResult
{
    public class TesteObterResultadoUtilizandoGetAwaiterGetResult
    {
        public static async Task ObterResultado()
        {
            try
            {
                var resultado = GeradorResultado().GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private static async Task<bool> GeradorResultado()
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