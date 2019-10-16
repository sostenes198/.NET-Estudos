using AutoMapper;

namespace Estudos.Global.Extensions
{
    public static class AutoMapperExtension
    {
        /// <summary>
        ///     Método
        ///     para
        ///     transforma
        ///     uma
        ///     entidade
        ///     fonte
        ///     em
        ///     uma
        ///     entidade
        ///     Destino
        ///     utilizando
        ///     AutoMapper
        /// </summary>
        /// <param
        ///     name="entidadeFonte">
        /// </param>
        /// <returns></returns>
        public static TDestino Transformar<TDestino>(this object entidadeFonte)
        {
            if (entidadeFonte == null)
                return default;

            return Mapper.Map<TDestino>(entidadeFonte);
        }
    }
}