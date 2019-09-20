using System;

namespace Thread
{
    class Program
    {
        static void Main(string[] args)
        {
//            System.Threading.Thread t1 = new System.Threading.Thread(RecursoCompartilhadoLock.IncrementCountWithoutLock);
//            System.Threading.Thread t2 = new System.Threading.Thread(RecursoCompartilhadoLock.IncrementCountWithoutLock);
//            System.Threading.Thread t3 = new System.Threading.Thread(RecursoCompartilhadoLock.IncrementCountWithoutLock);
//            t1.Start();
//            t2.Start();
//            t3.Start();
//            Console.Read();

            System.Threading.Thread[] Threads = new System.Threading.Thread [3];
            for (int i = 0; i < 3; i++)
            {
                Threads[i] = new System.Threading.Thread(RecursoCompartilhadoMonitor.PrintNumbersWithTrue);
                Threads[i].Name = "Child Thread " + i;
            }

            foreach (System.Threading.Thread t in Threads)
            {
                t.Start();
            }

            Console.ReadLine();
        }
    }
}