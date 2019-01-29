namespace Estudos.Redis.Domain.Serializer
{
    public interface ICacheSerializer
    {
        T Deserialize<T>(byte[] value) where T : class;
        byte[] Serialize<T>(T value) where T : class;
    }
}