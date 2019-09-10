using System;
using System.Threading.Tasks;

namespace AsyncAwait.IterarNumeros
{
    public class TesteIterarNumeroSync
    {
        public static void InterarNumero()
        {
            Iterar();
            
            for (int i = 0; i < 10; i++)
            {
                var x = i;
                Console.WriteLine(x);
            }
        }

        private static void Iterar()
        {
            for (int i = 0; i < 10; i++)
            {
                var x = i;
                Console.WriteLine(x);
            }
        }
    }
}