using System.Threading.Tasks;

namespace AsyncAwait.GetAwaitGetResult
{
    public class GetAwaitGetResultTest
    {
        public static async Task TestarGetAwaiterGetResult()
        {
            await TesteObterResultadoUtilizandoResult.ObterResultado();
            await TesteObterResultadoUtilizandoGetAwaiterGetResult.ObterResultado();
        }
    }
}