using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.RetornoTaskMetodo
{
    public class TesteObterResultadoMetodoTaskFormaCorreta
    {
        public static Task<int> ObterResultado()
        {
            Console.WriteLine($"Thread Id forma correta: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Task Id forma correta: {Task.CurrentId}");
            return ObterResultado1();
        }
        
        private static Task<int> ObterResultado1()
        {
            Console.WriteLine($"Thread Id forma correta: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Task Id forma correta: {Task.CurrentId}");
            return ObterResultado2();
        }   
        
        private static Task<int> ObterResultado2()
        {
            Console.WriteLine($"Thread Id forma correta: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Task Id forma correta: {Task.CurrentId}");
            return ObterResultado3();
        }
        
        private static Task<int> ObterResultado3()
        {
            return Task.Run(() =>{
                Console.WriteLine($"Thread Id forma correta: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Task Id forma correta: {Task.CurrentId}");
                return 10;
            });
        }
    }
}