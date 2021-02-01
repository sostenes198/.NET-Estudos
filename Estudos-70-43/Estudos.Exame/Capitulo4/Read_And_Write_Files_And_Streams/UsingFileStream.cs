using System;
using System.IO;
using System.Text;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class UsingFileStream
    {
        public static void Test()
        {
            var filePath = "OutputText.txt";
            // Writing to a File
            var outputStream = new FileStream(filePath, FileMode.OpenOrCreate,
                FileAccess.Write);
            var outputMessageString = "Hello world";
            var outputMessageBytes = Encoding.UTF8.GetBytes(outputMessageString);
            outputStream.Write(outputMessageBytes);
            outputStream.Close();
            
            var inputStream = new FileStream(filePath, FileMode.Open,
                FileAccess.Read);
            var fileLength = inputStream.Length;
            var readBytes = new byte[fileLength];
            inputStream.Read(readBytes, 0, (int) fileLength);
            var readString = Encoding.UTF8.GetString(readBytes);
            inputStream.Close();
            Console.WriteLine($"Read message: {readString}");
        }
    }
}