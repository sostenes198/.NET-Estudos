using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class DirectoryInfo_Class
    {
        public static void Test()
        {
            var pathDirectory = "TestDirInfo";
            var localDir = new DirectoryInfo(pathDirectory);
            localDir.Create();
            if(localDir.Exists)
                Console.WriteLine("Directory created successfully");
            localDir.Delete();
            Console.WriteLine("Directory deleted successfully");
        }
    }
}