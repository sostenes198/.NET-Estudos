using System;

namespace Thread
{
    public static class RecursoCompartilhadoLock
    {
        private static int Count;

        private static readonly object _lockObject = new object();

        private static readonly object _lockObjectCount = new object();

        public static void DisplayMessageWithouLock()
        {
            Console.Write("[Welcome to the ");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("world of dotnet!]");
        }

        public static void DisplayMessageWithLock()
        {
            lock (_lockObject)
            {
                Console.Write("[Welcome to the ");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("world of dotnet!]");
            }
        }

        public static void IncrementCountWithoutLock()
        {
            for (var i = 1; i <= 50000; i++)
            {
                Count++;
                Console.WriteLine(Count);
            }
        }

        public static void IncrementCountWithLock()
        {
            for (var i = 1; i <= 50000; i++)
                //Only protecting the shared Count variable
                lock (_lockObjectCount)
                {
                    Count++;
                    Console.WriteLine(Count);
                }
        }
    }
}