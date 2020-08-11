using System;

namespace Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates
{
    public class DelegateStudy
    {
        private delegate int IntOperation(int a, int b);

        private static int Add(int a, int b)
        {
            Console.WriteLine("Add Called");
            return a + b;
        }
        
        private static int Subtract(int a, int b)
        {
            Console.WriteLine("Subtract Called");
            return a - b;
        }

        public static void Run()
        {
            var op = new IntOperation(Add);
            Console.WriteLine(op(2,2));
            
            op = Subtract;
            Console.WriteLine(op(2,2));
        }
    }
}