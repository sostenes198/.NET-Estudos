using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class Searching_For_Files
    {
        public static void Test()
        {
            var startDir = new DirectoryInfo(@"..\..\..\..");
            var searchString = "*.cs";
            FindFiles(startDir, searchString);
        }
        private static void FindFiles(DirectoryInfo dir, string searchPattern)
        {
            foreach (var directory in dir.GetDirectories())
            {
                FindFiles(directory, searchPattern);
            }

            var matchingFiles = dir.GetFiles(searchPattern);
            foreach (var fileInfo in matchingFiles)
            {
                Console.WriteLine(fileInfo.Name);
            }
        }
    }
}