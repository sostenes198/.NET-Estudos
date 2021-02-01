using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class Using_Path
    {
        public static void Test()
        {
            var fullPathAbsolute = @"D:\teste.txt";
            var dirName = Path.GetDirectoryName(fullPathAbsolute);
            var fileName = Path.GetFileName(fullPathAbsolute);
            var fileExension = Path.GetExtension(fullPathAbsolute);
            var lisName = Path.ChangeExtension(fullPathAbsolute, ".lis");
            var newTest = Path.Combine(fullPathAbsolute, "newtest.txt");
            
            Console.WriteLine($"Full name: {fullPathAbsolute}");
            Console.WriteLine($"File directory: {dirName}");
            Console.WriteLine($"File name: {fileName}");
            Console.WriteLine($"File extensions: {fileExension}");
            Console.WriteLine($"File with lis extension: {lisName}");
            Console.WriteLine($"New test: {newTest}");
        }
    }
}