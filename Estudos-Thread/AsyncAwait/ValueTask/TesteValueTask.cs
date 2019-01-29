using System.Threading.Tasks;

namespace AsyncAwait.ValueTask
{
    public class TesteValueTask
    {
        public static ValueTask<int> ObterResultado()
        {
            return new ValueTask<int>(Task.Run(() => 10));
        }
    }
}