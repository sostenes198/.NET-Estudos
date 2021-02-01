using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class Using_FileInfo
    {
        public static void Test()
        {
            var filePath = "TextFile.txt";
            File.WriteAllText(filePath, "This text goes in the file");
            var info = new FileInfo(filePath);
            Console.WriteLine($"Name: {info.Name}");
            Console.WriteLine($"Full path: {info.FullName}");
            Console.WriteLine($"Last Access: {info.LastAccessTime}");
            Console.WriteLine($"Lenght: {info.Length}");
            Console.WriteLine($"Attributes: {info.Attributes}");
            Console.WriteLine("Make the file read only");
            info.Attributes |= FileAttributes.ReadOnly;
            Console.WriteLine($"Attributes: {info.Attributes}");
            Console.WriteLine("Remove the read only attributes");
            info.Attributes &= ~FileAttributes.ReadOnly;
            Console.WriteLine($"Attributes: {info.Attributes}");
        }
    }
}