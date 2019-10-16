using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskFormaIncorreta
    {
        public static async Task<int> ObterResultado()
        {
            return await ObterResultado1();
        }

        private static async Task<int> ObterResultado1()
        {
            return await ObterResultado2();
        }

        private static async Task<int> ObterResultado2()
        {
            return await ObterResultado3();
        }

        private static async Task<int> ObterResultado3()
        {
            return await Task.Run(() => 10);
        }
    }
}