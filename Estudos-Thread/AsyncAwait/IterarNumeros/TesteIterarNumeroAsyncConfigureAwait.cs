using System;
using System.Threading.Tasks;

namespace AsyncAwait.IterarNumeros
{
    public class TesteIterarNumeroAsyncConfigureAwait
    {
        public static void InterarNumero()
        {
            var task = IterarAsync();

            for (int i = 0; i < 10; i++)
            {
                var x = i;
                Console.WriteLine(x);
            }

            task.ConfigureAwait(false);
        }

        private static Task IterarAsync() =>
            Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var x = i;
                    Console.WriteLine(x);
                }
            });
    }
}