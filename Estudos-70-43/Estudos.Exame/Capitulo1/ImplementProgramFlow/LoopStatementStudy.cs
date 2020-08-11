using System;

namespace Estudos.Exame.Capitulo1.ImplementProgramFlow
{
    public class LoopStatementStudy
    {
        private static int counter;

        static void Initialize()
        {
            Console.WriteLine("Initialize called");
            counter = 0;
        }

        static void Update()
        {
            Console.WriteLine("Update Called");
            counter += 1;
        }

        static bool Test()
        {
            Console.WriteLine("Teste called");
            return counter < 5;
        }

        public static void Run()
        {
            for (Initialize(); Test(); Update())
            {
                Console.WriteLine($"Hello {counter}");
            }
        }
    }
}