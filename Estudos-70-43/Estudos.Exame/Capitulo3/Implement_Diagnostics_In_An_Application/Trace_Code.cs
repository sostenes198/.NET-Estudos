using System.Diagnostics;

namespace Estudos.Exame.Capitulo3.Implement_Diagnostics_In_An_Application
{
    public class Trace_Code
    {
        public static void Test()
        {
            Trace.WriteLine("Staring the program");
            Trace.TraceInformation("This is an information message");
            Trace.TraceWarning("This is an warning message");
            Trace.TraceError("This is a error message");
        }
    }
}