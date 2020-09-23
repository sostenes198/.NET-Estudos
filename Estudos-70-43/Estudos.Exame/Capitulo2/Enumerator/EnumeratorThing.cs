using System.Collections;
using System.Collections.Generic;

namespace Estudos.Exame.Capitulo2.Enumerator
{
    public class EnumeratorThing : IEnumerator<int>, IEnumerable
    {
        private int _count;
        private readonly int _limit;

        public EnumeratorThing(int limit)
        {
            _count = 0;
            _limit = limit;
        }

        public int Current => _count;

        object IEnumerator.Current => Current;

        public bool MoveNext() => ++_count <= _limit;

        public void Reset() => _count = 0;

        public IEnumerator GetEnumerator() => this;

        public void Dispose()
        {
        }
    }
}