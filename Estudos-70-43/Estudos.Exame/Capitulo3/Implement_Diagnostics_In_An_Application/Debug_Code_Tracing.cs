using System.Diagnostics;

namespace Estudos.Exame.Capitulo3.Implement_Diagnostics_In_An_Application
{
    public class Debug_Code_Tracing
    {
        public static void Test()
        {
            Debug.WriteLine("Starting the program");
            Debug.Indent();
            Debug.WriteLine("Inside a function");
            Debug.Indent();
            Debug.WriteLine("Outside a funtion");
            var customerName = "Rob";
            Debug.WriteLineIf(string.IsNullOrEmpty(customerName), "The name is empty");
        }
    }
}