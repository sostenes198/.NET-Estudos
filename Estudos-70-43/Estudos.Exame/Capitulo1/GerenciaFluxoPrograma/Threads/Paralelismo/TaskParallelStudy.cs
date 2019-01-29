using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Paralelismo
{
    public class TaskParallelStudy
    {
        public static void InvokeTasks()
        {
            Parallel.Invoke(Task1, Task2);
        }

        public static void InvokeForeach()
        {
            var itens = Enumerable.Range(0, 100);
            Parallel.ForEach(itens, item => WorkOnItem(item));
        }

        public static void InvokeFor()
        {
            var itens = Enumerable.Range(0, 100).ToArray();
            Parallel.For(0, itens.Count(), i => WorkOnItem(itens[0]));
        }

        public static void InvokeForLoopState()
        {
            var itens = Enumerable.Range(0, 500).ToArray();

            var result = Parallel.For(0, itens.Length, (i, loopState) =>
            {
                if (i == 200)
                    loopState.Stop();

                WorkOnItem(itens[i]);
            });
            
            Console.WriteLine($"Completed {result.IsCompleted}");
            Console.WriteLine($"Items: {result.LowestBreakIteration}");
        }

        private static void WorkOnItem(object item)
        {
            Console.WriteLine($"Started working on {item}");
            Thread.Sleep(100);
            Console.WriteLine($"Finished working on {item}");
        }

        private static void Task1()
        {
            Console.WriteLine("Task 1 Startging");
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 ending");
        }

        private static void Task2()
        {
            Console.WriteLine("Task 2 Startging");
            Thread.Sleep(1000);
            Console.WriteLine("Task 2 ending");
        }
    }
}