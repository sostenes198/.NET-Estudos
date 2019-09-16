using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskFormaCorreta
    {
        public static Task<int> ObterResultado() => ObterResultado1();

        private static Task<int> ObterResultado1() => ObterResultado2();

        private static Task<int> ObterResultado2() => ObterResultado3();

        private static Task<int> ObterResultado3() => Task.Run(() => 10);
    }
}