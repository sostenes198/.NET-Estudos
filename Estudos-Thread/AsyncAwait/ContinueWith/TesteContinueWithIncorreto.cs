using System.Threading.Tasks;

namespace AsyncAwait.ContinueWith
{
    public class TesteContinueWithIncorreto
    {
        public static async Task<bool> ObterResultado()
        {
            var result = await ObterResultado1();
            return result == 10;
        }

        private static Task<int> ObterResultado1()
        {
            return Task.Run(() => 9);
        }
    }
}