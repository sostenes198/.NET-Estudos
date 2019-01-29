using System;

namespace Estudos.IndicesIntervalos
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words = new string[]
            {
                // index from start    index from end
                "The",      // 0                   ^9
                "quick",    // 1                   ^8
                "brown",    // 2                   ^7
                "fox",      // 3                   ^6
                "jumped",   // 4                   ^5
                "over",     // 5                   ^4
                "the",      // 6                   ^3
                "lazy",     // 7                   ^2
                "dog"       // 8                   ^1
            };  
            
            Console.WriteLine($"The last word is {words[^1]}");
            string[] quickBrownFox = words[..^3];
            foreach (var word in quickBrownFox)
                Console.Write($"< {word} >");
            Console.WriteLine();
            
            Index the = ^3;
            Console.WriteLine(words[the]);
            Range phrase = 1..4;
            string[] text = words[phrase];
            foreach (var word in text)
                Console.Write($"< {word} >");
            Console.WriteLine();
        }
    }
}