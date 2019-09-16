using System;
using System.Threading.Tasks;

namespace AsyncAwait.IterarNumeros
{
    public class TesteIterNumeroAsyncSemCriarNovaTask
    {
        public static Task InterarNumero()
        {
            var task = IterarAsync();

            for (int i = 0; i < 10; i++)
            {
                var x = i;
                Console.WriteLine(x);
            }

            return task;
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