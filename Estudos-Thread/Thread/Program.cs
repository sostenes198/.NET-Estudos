using System;

namespace Thread
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread t1 = new System.Threading.Thread(RecursoCompartilhado.IncrementCountWithoutLock);
            System.Threading.Thread t2 = new System.Threading.Thread(RecursoCompartilhado.IncrementCountWithoutLock);
            System.Threading.Thread t3 = new System.Threading.Thread(RecursoCompartilhado.IncrementCountWithoutLock);
            t1.Start();
            t2.Start();
            t3.Start();
            Console.Read();
        }
    }
}