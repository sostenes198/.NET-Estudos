using System;
using System.Threading.Tasks;

namespace AsyncAwait.IterarNumeros
{
    public class TesteIterarNumeroAsyncConfigureAwait
    {
        public static void InterarNumero()
        {
            var task = IterarAsync();

            for (var i = 0; i < 10; i++)
            {
                var x = i;
                Console.WriteLine(x);
            }

            task.ConfigureAwait(false);
        }

        private static Task IterarAsync()
        {
            return Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    var x = i;
                    Console.WriteLine(x);
                }
            });
        }
    }
}