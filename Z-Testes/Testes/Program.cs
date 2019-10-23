using System;

namespace Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            var stringTeste = new string('*', 1000000000);
            Console.WriteLine(TestesMetodosStrings.TesteStringIsNullOrEmpty(stringTeste));
            Console.WriteLine(TestesMetodosStrings.IsNullOrWhiteSpace(stringTeste));
            Console.WriteLine(TestesMetodosStrings.SmartStringIsNullOrEmpty(stringTeste));
        }
    }
}