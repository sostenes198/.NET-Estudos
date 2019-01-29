using System;
using System.Threading.Tasks;

namespace AsyncAwait.ContinueWith
{
    public class ContinueWithTest
    {
        public static async Task TestarContinueWith()
        {
            Console.WriteLine(await TesteContinueWithIncorreto.ObterResultado());
            Console.WriteLine(await TesteContinueWithCorreto.ObterResultado());
        }
    }
}