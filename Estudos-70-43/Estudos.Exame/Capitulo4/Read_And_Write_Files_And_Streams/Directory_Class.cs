using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class Directory_Class
    {
        public static void Test()
        {
            var pathDirecotory = "TestDir";
            
            Directory.CreateDirectory(pathDirecotory);
            if(Directory.Exists(pathDirecotory))
                Console.WriteLine("Directory exist");
            Directory.Delete(pathDirecotory);
            Console.WriteLine("Directory deleted");
        }
    }
}