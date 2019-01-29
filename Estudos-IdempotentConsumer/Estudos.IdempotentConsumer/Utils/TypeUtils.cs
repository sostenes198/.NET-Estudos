namespace Estudos.IdempotentConsumer.Utils;

internal static class TypeUtils
{
    internal static Type? GetGenericBaseType<T>(Type baseType)
    {
        return GetGenericBaseType(baseType, typeof(T));
    } 
    
    internal static Type? GetGenericBaseType(Type baseType, Type? type)
    {
        while (type != null)
        {
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (baseType.IsAssignableFrom(genericType))
                    return genericType;
            }
            type = type.BaseType;
        }

        return null;
    } 
}