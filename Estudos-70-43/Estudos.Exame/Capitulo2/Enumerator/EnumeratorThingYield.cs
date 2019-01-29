using System.Collections;
using System.Collections.Generic;

namespace Estudos.Exame.Capitulo2.Enumerator
{
    public class EnumeratorThingYield : IEnumerable<int>
    {
        private readonly int _limit;

        public EnumeratorThingYield(int limit)
        {
            _limit = limit;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 1; i <= _limit; i++)
                yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}