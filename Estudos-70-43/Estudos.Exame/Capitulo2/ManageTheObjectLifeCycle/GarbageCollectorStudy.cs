using System.Threading;

namespace Estudos.Exame.Capitulo2.ManageTheObjectLifeCycle
{
    public class GarbageCollectorStudy
    {
        public static void Test()
        {
            for (int i = 0; i < 1_000_000_000_000; i++)
            {
                var p = new Person();
                Thread.Sleep(3);
            }
        }
    }
}