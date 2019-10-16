using System.Collections;

namespace WhereDynamic.Extensions
{
    public static class IEnumerableExtensions
    {
        public static object ObterPrimeiroElemento(this IEnumerable ts)
        {
            object resultado = null;
            foreach (var item in ts)
            {
                resultado = item;
                break;
            }

            return resultado;
        }
    }
}