using System;
using System.Threading;

namespace Thread
{
    public static class RecursoCompartilhadoMonitor
    {
        private static readonly object lockObject = new object();

        private static readonly object lockObjectWithTrue = new object();

        public static void PrintNumbers()
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " Trying to enter into the critical section");
            Monitor.Enter(lockObject);
            try
            {
                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " Entered into the critical section");
                for (var i = 0; i < 5; i++)
                {
                    System.Threading.Thread.Sleep(100);
                    Console.Write(i + ",");
                }

                Console.WriteLine();
            }
            finally
            {
                Monitor.Exit(lockObject);
                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " Exit from critical section");
            }
        }

        public static void PrintNumbersWithTrue()
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " Trying to enter into the critical section");
            var IsLockTaken = false;
            Monitor.Enter(lockObjectWithTrue, ref IsLockTaken);
            try
            {
                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " Entered into the critical section");
                for (var i = 0; i < 5; i++)
                {
                    System.Threading.Thread.Sleep(100);
                    Console.Write(i + ",");
                }

                Console.WriteLine();
            }
            finally
            {
                Monitor.Exit(lockObjectWithTrue);
                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " Exit from critical section");
            }
        }
    }
}