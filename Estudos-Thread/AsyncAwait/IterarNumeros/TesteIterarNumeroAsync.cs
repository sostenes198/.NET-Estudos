using System;
using System.Threading.Tasks;

namespace AsyncAwait.IterarNumeros
{
    public class TesteIterarNumeroAsync
    {
        public static async Task InterarNumeroAsync()
        {
            var task = IterarAsync();

            for (var i = 0; i < 10; i++)
            {
                var x = i;
                Console.WriteLine(x);
            }

            await task;
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