using System;
using System.Diagnostics;

namespace Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = "reg ADD HKLM\\SOFTWARE\\asd",
                    //UseShellExecute = true,
                    //Verb = "runas"
                };
                process.Start();
                //ComandosCmd.ExecutarComandosCMD();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}