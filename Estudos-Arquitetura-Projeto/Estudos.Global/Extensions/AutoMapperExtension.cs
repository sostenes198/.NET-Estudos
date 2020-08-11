using AutoMapper;

namespace Estudos.Global.Extensions
{
    public static class AutoMapperExtension
    {
        public static TDestino Transformar<TDestino>(this object entidadeFonte)
        {
            if (entidadeFonte == null)
                return default;

            return Mapper.Map<TDestino>(entidadeFonte);
        }
    }
}