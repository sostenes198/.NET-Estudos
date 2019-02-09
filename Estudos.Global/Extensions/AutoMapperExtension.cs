using AutoMapper;
using System;

namespace Estudos.Global.Extensions
{
    public static class AutoMapperExtension
    {

        /// <summary>
        /// Método para transforma uma entidade fonte em uma entidade Destino utilizando AutoMapper
        /// </summary>
        /// <typeparam name="T"> Entidade Destino </typeparam>
        /// <param name="entidadeFonte"></param>
        /// <returns></returns>
        public static TDestino Transformar<TDestino>(this object entidadeFonte)
        {
            try
            {
                if (entidadeFonte == null)
                    return default(TDestino);

                return Mapper.Map<TDestino>(entidadeFonte);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
