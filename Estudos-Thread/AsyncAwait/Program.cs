using System.Threading.Tasks;

namespace AsyncAwait
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await RetornoTaskMetodo.TesteObterResultadoMetodoTaskBlocoTryCatchIncorreto.ObterResultado();
        }
    }
}