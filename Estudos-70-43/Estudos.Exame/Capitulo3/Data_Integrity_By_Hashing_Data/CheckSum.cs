using System;

namespace Estudos.Exame.Capitulo3.Data_Integrity_By_Hashing_Data
{
    public class CheckSum
    {
        public static void Test()
        {
            ShowCheckSumObjectHashCode("Hello world");
            ShowCheckSumObjectHashCode("world Hello");
            ShowCheckSumObjectHashCode("Heem world");
        }

        private static void ShowCheckSumObjectHashCode(string source)
        {
            Console.WriteLine("Hash for {0} is {1:X}", source, source.GetHashCode());
        }

        private static void ShowCheckSum(string source)
        {
            Console.WriteLine("Checksum for {0} is {1}",
                source, CalculateCheckSum(source));
        }

        private static int CalculateCheckSum(string source)
        {
            int total = 0;
            foreach (var ch in source)
            {
                total += ch;
            }

            return total;
        }
    }
}