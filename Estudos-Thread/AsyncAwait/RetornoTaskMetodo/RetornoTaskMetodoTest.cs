using System;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class RetornoTaskMetodoTest
    {
        public static async Task TestarRetornoTaskMetodo()
        {
            var resultadoFormaIncorreta = await TesteObterResultadoMetodoTaskFormaIncorreta.ObterResultado();
            Console.Write(resultadoFormaIncorreta);

            var resultadoFormaCorreta = await TesteObterResultadoMetodoTaskFormaCorreta.ObterResultado();
            Console.Write(resultadoFormaCorreta);
        }
    }
}