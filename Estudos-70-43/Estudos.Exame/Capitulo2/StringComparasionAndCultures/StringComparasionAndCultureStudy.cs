using System;
using System.Globalization;
using System.Threading;

namespace Estudos.Exame.Capitulo2.StringComparasionAndCultures
{
    public class StringComparasionAndCultureStudy
    {
        public static void Test()
        {
            // Default comparision fails because the strings are different
            if (!"encyclopædia".Equals("encyclopaedia"))
                Console.WriteLine("Unicode encyclopaedias are not equals");

            // Set the current culture for this thread to EN-US
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            // Using the current culture the string are equal
            if ("encyclopædia".Equals("encyclopaedia", StringComparison.CurrentCulture))
                Console.WriteLine("Culture comparison encyclopaedias are equal");

            // We can use the IgnoreCase options to perform comparisions that ignore case
            if ("encyclopædia".Equals("ENCYCLOPAEDIA", StringComparison.CurrentCultureIgnoreCase))
                Console.WriteLine("Case culture comparison encyclopaedias are equal");

            if (!"encyclopædia".Equals("ENCYCLOPAEDIA", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine("Ordinal comparison encyclopaedia are not equal");
        }
    }
}