using System;
using Estudos.Exame.Capitulo4.Serialize_And_Deserialize_Data_By_Using_Serializations;

namespace Estudos.Exame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            var assemblyName = typeof(Program).Assembly.FullName;
            Console.WriteLine(assemblyName);

            Binary_Serialization.Test();
        }
    }
}