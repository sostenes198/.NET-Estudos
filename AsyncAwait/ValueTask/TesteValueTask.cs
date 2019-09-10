using System.Threading.Tasks;

namespace AsyncAwait.ValueTask
{
    public class TesteValueTask
    {
        public static ValueTask<int> ObterResultado()
        {
            return ObterResultado1();
        }

        private static ValueTask<int> ObterResultado1()
        {
            return ObterResultado2();
        }

        private static ValueTask<int> ObterResultado2()
        {
            return new ValueTask<int>(Task.Run(() => 10));
        }
    }
}