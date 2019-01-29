using System;
using System.IO;

namespace Estudos.Exame.Capitulo2.ManipulateStringByUsingThe_StringBuilderStringWriterAndStringReader
{
    public class StringWriterStudy
    {
        public static void Test()
        {
            var writer = new StringWriter();
            writer.WriteLine("Hellor world");
            writer.Close();
            Console.WriteLine(writer.ToString());
        }
    }
}