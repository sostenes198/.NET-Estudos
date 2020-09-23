using System;

namespace Estudos.Exame.Capitulo2.Enumerator
{
    public class EnumeratorStudy
    {
        public static void Test()
        {
            var enumerator = "Hello World".GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }
        }
    }
}