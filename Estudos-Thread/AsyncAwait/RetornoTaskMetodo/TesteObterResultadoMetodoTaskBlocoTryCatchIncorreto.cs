using System;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskBlocoTryCatchIncorreto
    {
        public static void ObterResultado()
        {
            try
            {
                ObterResultado1();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ObterResultado1() => Task.Run(ObterResultado2);

        private static Task<int> ObterResultado2() => throw new Exception("Mensagem de Erro");
    }
}