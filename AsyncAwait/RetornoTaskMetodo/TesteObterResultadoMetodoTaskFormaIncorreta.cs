using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskFormaIncorreta
    {
        public static async Task<int> ObterResultado()
        {
            Console.WriteLine($"Thread Id forma incorreta: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Task Id forma incorreta: {Task.CurrentId}");
            return await ObterResultado1();
        }
        
        private static async Task<int> ObterResultado1()
        {
            Console.WriteLine($"Thread Id forma incorreta: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Task Id forma incorreta: {Task.CurrentId}");
            return await ObterResultado2();
        }   
        
        private static async Task<int> ObterResultado2()
        {
            Console.WriteLine($"Thread Id forma incorreta: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Task Id forma incorreta: {Task.CurrentId}");
            return await ObterResultado3();
        }
        
        private static async Task<int> ObterResultado3()
        {
            return await Task.Run(() =>{
                Console.WriteLine($"Thread Id forma incorreta: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Task Id forma incorreta: {Task.CurrentId}");
                return 10;
            });
        }
    }
}