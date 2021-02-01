using System;
using System.Security.Cryptography;
using System.Text;

namespace Estudos.Exame.Capitulo3.Data_Integrity_By_Hashing_Data
{
    public class SHA2_Hashing
    {
        private static byte[] CalculateHash(string source)
        {
            var converter = new ASCIIEncoding();
            var sourceBytes = converter.GetBytes(source);
            var hasher = SHA256.Create();
            var hash = hasher.ComputeHash(sourceBytes);
            return hash;
        }

        private static void ShowHash(string source)
        {
            Console.Write("Hash for {0} is: ", source);
            var hash = CalculateHash(source);
            foreach (var b in hash)
                Console.WriteLine("{0:X} ", b);
            Console.WriteLine();
        }

        public static void Test()
        {
            ShowHash("Hello World");
            ShowHash("world Hello");
            ShowHash("Hemmm world");
        }
    }
}