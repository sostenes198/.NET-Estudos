namespace Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates
{
    public class ClosureStudy
    {
        public delegate int GetValue();

        public static GetValue getLocalInt;

        public static void Run()
        {
            int localInt = 99;
            getLocalInt = () => localInt;
        }
    }
}