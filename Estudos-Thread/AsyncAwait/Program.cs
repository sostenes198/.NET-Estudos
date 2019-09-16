using System.Threading.Tasks;
using AsyncAwait.IterarNumeros;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await IterarNumerosTeste.TesteITerarNumeros();
        }
    }
}