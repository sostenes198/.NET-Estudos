using System;
using System.Diagnostics;

namespace Estudos.Exame.Capitulo2.Attributes
{
    public class ConditionalAttributeTest
    {
        [Conditional("DEBUG"), Conditional("TRACE")]
        private static void ReportHeader()
        {
            Console.WriteLine("This is the header for the report");
        }

        [Conditional("DEBUG")]
        private static void VerboseReport()
        {
            Console.WriteLine("This is output from the verbose report");
        }

        [Conditional("TRACE")]
        private static void TerseReport()
        {
            Console.WriteLine("This is output from the terse report");
        }

        public static void Test()
        {
            ReportHeader();
            VerboseReport();
            TerseReport();
        }
    }
}