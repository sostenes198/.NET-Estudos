using MessagePack;
using MessagePack.Resolvers;

namespace Estudos.Redis.Domain.Serializer
{
    public class CacheSerializer : ICacheSerializer
    {
        public T Deserialize<T>(byte[] value) where T : class
        {
            if (value == default)
                return default;

            return MessagePackSerializer.Deserialize<T>(value, ContractlessStandardResolver.Options);

        }

        public byte[] Serialize<T>(T value) where T : class
        {
            if (value == default)
                return default;

            return MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Options);
        }
    }
}