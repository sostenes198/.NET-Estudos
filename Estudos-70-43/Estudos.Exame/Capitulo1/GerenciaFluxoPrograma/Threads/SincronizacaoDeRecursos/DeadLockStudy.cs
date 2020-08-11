using System;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.SincronizacaoDeRecursos
{
    public class DeadLockStudy
    {
        static object lock1 = new object();
        static object lock2 = new object();


        public static void Metodo1()
        {
            lock (lock1)
            {
                Console.WriteLine("Metodo 1 lock 1");
                Console.WriteLine("Metodo 1 waiting for lock 2");
                lock (lock2)
                {
                    Console.WriteLine("Metodo 1 got lock 1");
                }

                Console.WriteLine("Metodo 1 released lock 2");
            }

            Console.WriteLine("Metodo 1 released lock 1");
        }
        
        public static void Metodo2()
        {
            lock (lock2)
            {
                Console.WriteLine("Metodo 2 lock 2");
                Console.WriteLine("Metodo 2 waiting for lock 1");
                lock (lock1)
                {
                    Console.WriteLine("Metodo 2 got lock 1");
                }

                Console.WriteLine("Metodo 2 released lock 1");
            }

            Console.WriteLine("Metodo 2 released lock 2");
        }
    }
}