using System.Threading.Tasks;
using AsyncAwait.IterarNumeros;

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