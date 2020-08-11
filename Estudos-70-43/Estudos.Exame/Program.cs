using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates;
using Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates.Action;
using Estudos.Exame.Capitulo1.ImplementProgramFlow;

namespace Estudos.Exame
{
    class Program
    {
        static void Main(string[] args)
        {
            ClosureStudy.Run();
            Console.WriteLine($"Value of localInt {ClosureStudy.getLocalInt()}");
        }
    }
}