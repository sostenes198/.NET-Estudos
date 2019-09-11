using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskFormaIncorreta
    {
        public static async Task<int> ObterResultado() => await ObterResultado1();

        private static async Task<int> ObterResultado1() => await ObterResultado2();

        private static async Task<int> ObterResultado2() => await ObterResultado3();

        private static async Task<int> ObterResultado3() => await Task.Run(() => 10);
    }
}