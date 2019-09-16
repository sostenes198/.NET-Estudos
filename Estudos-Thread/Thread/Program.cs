using System;

namespace Thread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");
            //Main Thread creating three child threads
            System.Threading.Thread thread1 = new System.Threading.Thread(Method1);
            System.Threading.Thread thread2 = new System.Threading.Thread(Method2);
            System.Threading.Thread thread3 = new System.Threading.Thread(Method3);
            
            thread1.Start();
            thread2.Start();
            thread3.Start();
            
            thread1.Join();
            thread2.Join();
            thread3.Join();
            
            Console.WriteLine("Main Thread Ended");
        }
        
        static void Method1()
        {
            Console.WriteLine("Method1 - Thread1 Started");
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("Method1 - Thread 1 Ended");
        }
        static void Method2()
        {
            Console.WriteLine("Method2 - Thread2 Started");
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Method2 - Thread2 Ended");
        }
        static void Method3()
        {
            Console.WriteLine("Method3 - Thread3 Started");
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine("Method3 - Thread3 Ended");
        }
    }
}