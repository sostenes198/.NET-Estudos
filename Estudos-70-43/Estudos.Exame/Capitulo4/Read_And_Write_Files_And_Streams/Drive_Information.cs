using System;
using System.IO;

namespace Estudos.Exame.Capitulo4.Read_And_Write_Files_And_Streams
{
    public class Drive_Information
    {
        public static void Test()
        {
            var drivers = DriveInfo.GetDrives();
            foreach (var driver in drivers)
            {
                Console.WriteLine($"Name: {driver.Name}");
                if (driver.IsReady)
                {
                    Console.WriteLine($"Type: {driver.DriveType}");
                    Console.WriteLine($"Format: {driver.DriveFormat}");
                    Console.WriteLine($"Free space: {driver.TotalFreeSpace}");
                }
                else
                {
                    Console.WriteLine("Driver not ready");
                }
                Console.WriteLine();
            }
        }
    }
}