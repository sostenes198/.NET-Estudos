using System;
using System.IO;
using System.IO.Compression;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class Storing_Compressed_Files
    {
        public static void Test()
        {
            using (FileStream writeFile = new FileStream("CompText.zip", FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (GZipStream writeFileZip = new GZipStream(writeFile, CompressionMode.Compress))
                {
                    using (StreamWriter writeFileText = new StreamWriter(writeFileZip))
                    {
                        writeFileText.WriteLine("Hello world");
                    }
                }
            }
            
            using (FileStream readFile = new FileStream("CompText.zip", FileMode.Open, FileAccess.Read))
            {
                using (GZipStream readFileZip = new GZipStream(readFile, CompressionMode.Decompress))
                {
                    using (StreamReader readFileText = new StreamReader(readFileZip))
                    {
                        var message = readFileText.ReadToEnd();
                        Console.WriteLine($"Read Text: {message}");
                    }
                }
            }
        }
    }
}