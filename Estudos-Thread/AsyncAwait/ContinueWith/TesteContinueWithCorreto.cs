using System.Threading.Tasks;

namespace AsyncAwait.ContinueWith
{
    public class TesteContinueWithCorreto
    {
        public static Task<bool> ObterResultado() => ObterResultado1().ContinueWith(result => result.GetAwaiter().GetResult() == 10);

        private static Task<int> ObterResultado1() => Task.Run(() => 10);
    }
}