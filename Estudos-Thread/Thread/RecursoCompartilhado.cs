using System;

namespace Thread
{
    public static class RecursoCompartilhado
    {
        static int Count = 0;

        public static void DisplayMessageWithouLock()
        {
            Console.Write("[Welcome to the ");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("world of dotnet!]");
        }

        private static object _lockObject = new object();

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
            for (int i = 1; i <= 50000; i++)
            {
                Count++;
                Console.WriteLine(Count);
            }
        }

        private static object _lockObjectCount = new object();

        public static void IncrementCountWithLock()
        {
            for (int i = 1; i <= 50000; i++)
            {
                //Only protecting the shared Count variable
                lock (_lockObjectCount)
                {
                    Count++;
                    Console.WriteLine(Count);
                }
            }
        }
    }
}