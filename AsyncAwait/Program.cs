using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncAwait.ConfigureAwait;
using AsyncAwait.ContinueWith;
using AsyncAwait.GetAwaitGetResult;
using AsyncAwait.IterarNumeros;
using AsyncAwait.RetornoTaskMetodo;
using AsyncAwait.ValueTask;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ContinueWithTest.TestarContinueWith();
        }
    }
}