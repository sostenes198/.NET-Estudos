using System;
using System.Collections.Concurrent;

namespace Estudos.Colecoes.ThreadSafe.Implementacoes_Thread_Safe.Pool.de.objetos.usando.um.ConcurrentBag
{
    public class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> _objects;
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objectGenerator = objectGenerator;
            _objects = new ConcurrentBag<T>();
        }

        public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

        public void Return(T item) => _objects.Add(item);
    }
}