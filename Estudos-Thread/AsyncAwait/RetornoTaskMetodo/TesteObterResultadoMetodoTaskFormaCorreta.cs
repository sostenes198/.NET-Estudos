using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskFormaCorreta
    {
        public static Task<int> ObterResultado()
        {
            return ObterResultado1();
        }

        private static Task<int> ObterResultado1()
        {
            return ObterResultado2();
        }

        private static Task<int> ObterResultado2()
        {
            return ObterResultado3();
        }

        private static Task<int> ObterResultado3()
        {
            return Task.Run(() => 10);
        }
    }
}