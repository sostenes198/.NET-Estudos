using System;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskBlocoTryCatchIncorreto
    {
        public static Task ObterResultado()
        {
            try
            {
                return ObterResultado1();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.CompletedTask;
            }
        }

        private static Task ObterResultado1()
        {
            return Task.Run(ObterResultado2);
        }

        private static Task<int> ObterResultado2()
        {
            throw new Exception("Mensagem de Erro");
        }
    }
}