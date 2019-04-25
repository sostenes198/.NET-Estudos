using System.Collections;
using System.Linq;

namespace WhereDynamic.Extensions
{
    public static class IEnumerableExtensions
    {
        public static object ObterPrimeiroElemento(this IEnumerable ts)
        {

            object resultado = null;
            foreach (object item in ts)
            {
                resultado = item;
                break;
            }

            return resultado;
        }
    }
}
