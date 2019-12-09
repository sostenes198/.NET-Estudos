using System;
using System.Diagnostics;

namespace Testes
{
    public static class ComandosCmd
    {
        public static void ExecutarComandosCMD()
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal, 
                FileName = "cmd.exe", 
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            process.StartInfo = startInfo;
            process.Start();
            
            process.StandardInput.WriteLine("ping globo.com");
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
            var resultado = process.StandardOutput.ReadToEnd();
            Console.WriteLine(resultado);
        }
    }
}