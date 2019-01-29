using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Tasks
{
    public class TaskStudy
    {
        public static void StartAndRunTask()
        {
            var newTask = new Task(DoWork);
            newTask.Start();
            newTask.Wait();
        }

        public static void RunTask()
        {
            var newTask = Task.Run(DoWork);
            newTask.Wait();
        }

        public static void RunTaskWithReturn()
        {
            var newTask = Task.Run(CalculateResultDoWork);
            Console.WriteLine(newTask.Result);
        }

        public static void WaitAllTasks()
        {
            var tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                var numTask = i;
                tasks[i] = Task.Run(() => DoWork(numTask));
            }
            
            Task.WaitAll(tasks);
            
            Console.WriteLine("Finished processing");
        }

        public static void ContinueWithTask()
        {
            var task = Task.Run(HelloTask);
            task.ContinueWith(WorldTask, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(FailureTask, TaskContinuationOptions.OnlyOnFaulted);
        }
        
        public static void TaskParent()
        {
            var parent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent start");
                for (int i = 0; i < 10; i++)
                {
                    int taskNo = i;
                    Task.Factory.StartNew(DoChild,
                        taskNo,
                        TaskCreationOptions.AttachedToParent);
                }
            });
            parent.Wait();
        }

        private static void DoWork()
        {
            Console.WriteLine("Work starting");
            Thread.Sleep(2000);
            Console.WriteLine("Work Finished");
        }

        private static void DoWork(int i)
        {
            Console.WriteLine($"Work starting {i}");
            Thread.Sleep(2000);
            Console.WriteLine("Work Finished");
        }

        private static int CalculateResultDoWork()
        {
            Console.WriteLine("Work starting");
            Thread.Sleep(2000);
            Console.WriteLine("Work Finished");
            return 99;
        }
        
        private static void HelloTask()
        {
            Console.WriteLine("Hello");
            Thread.Sleep(2000);
        }
        
        private static void WorldTask(Task prevTask)
        {
            Console.WriteLine("World");
            Thread.Sleep(2000);
            //throw new Exception("FAIOOO");
        }

        private static void FailureTask(Task prevTask)
        {
            Console.WriteLine("Faio");
            Thread.Sleep(2000);
        }

        private static void DoChild(object state)
        {
            Console.WriteLine($"Child {state} start");
            Thread.Sleep(2000);
            Console.WriteLine($"Child {state} finished");
        }
    }
}