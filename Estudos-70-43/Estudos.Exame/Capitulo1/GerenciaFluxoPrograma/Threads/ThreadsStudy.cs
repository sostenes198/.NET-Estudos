using System;
using System.Threading;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads
{
    public class ThreadsStudy
    {
        public static void StartThread()
        {
            var thread = new Thread(HelloWorlThread);
            thread.Start();
        }

        public static void LambdaThread()
        {
            var thread = new Thread(() =>
            {
                Console.WriteLine("Hello World");
                //Thread.Sleep(2000);
            });
            thread.Start();
        }

        public static void ThreadWithParameterizedThreadStart()
        {
            var ps = new ParameterizedThreadStart(WOrkOnData);
            var thread = new Thread(ps);
            thread.Start(99);
        }

        public static void ThreadWithParams()
        {
            var thread = new Thread(WOrkOnData);
            thread.Start(99);
        }

        public static void AbortThread()
        {
            var tickThread = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("TICK");
                    Thread.Sleep(1000);
                }
            });
            tickThread.Start();

            Console.WriteLine("Press any key to leaver");
            Console.ReadKey();
            tickThread.Abort();
        }

        public static void AbortThreadBetter()
        {
            var tickRunning = true;
            var tickThread = new Thread(() =>
            {
                while (tickRunning)
                {
                    Console.WriteLine("TICK");
                    Thread.Sleep(1000);
                }
            });
            tickThread.Start();

            Console.WriteLine("Press any key to leaver");
            Console.ReadKey();
            tickRunning = false;
        }

        public static void ThreadJoinMethod()
        {
            var threadWaitForJoin = new Thread(_ =>
            {
                Console.WriteLine("Thread starting");
                Thread.Sleep(2000);
                Console.WriteLine("Thread done");
            });

            threadWaitForJoin.Start();
            Console.WriteLine("Wait for joined Thread");
            threadWaitForJoin.Join();
            Console.WriteLine("FOI");
        }


        public static void ThreadDataStorage()
        {
            var t1 = new Thread(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Console.WriteLine($"t1: {RangomGenerator.Value.Next(10)}");
                    Thread.Sleep(500);
                }
            });

            var t2 = new Thread(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Console.WriteLine($"t2: {RangomGenerator.Value.Next(10)}");
                    Thread.Sleep(500);
                }
            });
            t1.Start();
            t2.Start();
        }

        static ThreadLocal<Random> RangomGenerator = new ThreadLocal<Random>(() => new Random(2));


        public static void DisplayThread(Thread t)
        {
            Console.WriteLine($"Name: {t.Name}");
            Console.WriteLine($"CurrentCulture: {t.CurrentCulture}");
            Console.WriteLine($"Priority: {t.Priority}");
            Console.WriteLine($"ExecutionContext: {t.ExecutionContext}");
            Console.WriteLine($"IsBackground: {t.IsBackground}");
            Console.WriteLine($"IsThreadPoolThread: {t.IsThreadPoolThread}");
        }

        public static void DoWorkThreadPools()
        {
            for (var i = 0; i < 5; i++)
            {
                int stateNumber = i;
                ThreadPool.QueueUserWorkItem(state => WOrkOnData(stateNumber));
            }
        }

        private static void WOrkOnData(object data)
        {
            Console.WriteLine($"Working on {data}");
            Thread.Sleep(5000);
            Console.WriteLine("Finish work");
        }


        private static void HelloWorlThread()
        {
            Console.WriteLine("Hello World");
            Thread.Sleep(2000);
        }
    }
}