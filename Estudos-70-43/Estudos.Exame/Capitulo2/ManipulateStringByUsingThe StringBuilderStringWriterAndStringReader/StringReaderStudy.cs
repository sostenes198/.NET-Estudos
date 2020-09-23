using System;
using System.IO;

namespace Estudos.Exame.Capitulo2.ManipulateStringByUsingThe_StringBuilderStringWriterAndStringReader
{
    public class StringReaderStudy
    {
        public static void Test()
        {
            var dataStringReader = new StringReader(@"Soso
            25");

            var name = dataStringReader.ReadLine();
            var age = int.Parse(dataStringReader.ReadLine());
            dataStringReader.Close();
            
            Console.WriteLine($"Name: {name} Age: {age}");
        }
    }
}