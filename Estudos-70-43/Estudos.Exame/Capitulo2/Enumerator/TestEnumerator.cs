using System;

namespace Estudos.Exame.Capitulo2.Enumerator
{
    public class TestEnumerator
    {
        public static void TestEnumeratorThing()
        {
            var enumeratorThing = new EnumeratorThing(2);
            while (enumeratorThing.MoveNext())
            {
                Console.WriteLine(enumeratorThing.Current);
            }

            foreach (var thing in enumeratorThing)
                Console.WriteLine(thing);
        }
        
        public static void TestEnumeratorThingYield()
        {
            var enumeratorThingYield = new EnumeratorThingYield(10);
            foreach (var enumeratorThing in enumeratorThingYield)
            {
                Console.WriteLine(enumeratorThing);
            }
        }
    }
}