using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class StreamWriter_StreamReader
    {
        public static void Test()
        {
            using (var writeStream = new StreamWriter("OutputText.txt"))
            {
                writeStream.Write("Hello world");
            }

            using (var readStream = new StreamReader("OutputText.txt"))
            {
                var text = readStream.ReadToEnd();
                Console.WriteLine($"Text read: {text}");
            }
        }
    }
}