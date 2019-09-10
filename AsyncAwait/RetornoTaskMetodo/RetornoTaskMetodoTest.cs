using System;
using System.Threading.Tasks;
using AsyncAwait.GetAwaitGetResult;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class RetornoTaskMetodoTest
    {
        public static async Task TestarRetornoTaskMetodo()
        {
            Console.Write(await TesteObterResultadoMetodoTaskFormaIncorreta.ObterResultado());
            Console.Write("\n\n\n");
            Console.Write(await TesteObterResultadoMetodoTaskFormaCorreta.ObterResultado());
            Console.Write("\n\n\n");
            TesteObterResultadoMetodoTaskBlocoTryCatchIncorreto.ObterResultado();
            Console.WriteLine("AWE");
            Console.WriteLine("AWE");
            Console.Write("\n");
            TesteObterResultadoMetodoTaskBlocoTryCatchCorreto.ObterResultado();
            Console.WriteLine("AWE");
            Console.WriteLine("AWE");
            await Task.Delay(1000);
        }
    }
}