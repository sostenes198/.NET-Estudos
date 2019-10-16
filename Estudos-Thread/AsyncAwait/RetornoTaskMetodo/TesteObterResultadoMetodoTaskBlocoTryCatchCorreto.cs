using System;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskBlocoTryCatchCorreto
    {
        public static async Task ObterResultado()
        {
            try
            {
                await ObterResultado1();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Task<int> ObterResultado1()
        {
            return Task.Run(ObterResultado2);
        }

        private static Task<int> ObterResultado2()
        {
            throw new Exception("Mensagem de Erro");
        }
    }
}