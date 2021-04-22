namespace Estudos.Redis.Domain.Redis
{
    public class CacheEntry<T>
        where T : class
    {
        public bool Sucess { get; }
        public T Value { get; }

        private CacheEntry(bool sucess, T value)
        {
            Sucess = sucess;
            Value = value;
        }

        public static CacheEntry<T> Create(bool succes, T value) => new(succes, value);

        public static CacheEntry<T> NotFound => new(false, default);
    }
}