using System;
using System.Collections.Generic;
using System.Linq;
using Estudos.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Estudos.Repositorio.EntityFrameworkCore.Extensoes
{
    public static class EntityFrameworkCoreExtension
    {
        #region Métodos Extensivos

        public static T Find<T>(this EntityContext context, T entidade)
            where T : AEntidade
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (entidade == null) throw new ArgumentNullException(nameof(entidade));

            return context.Set<T>().Find(context.FindPrimaryKeyValues(entidade));
        }

        public static IEnumerable<string> FindPrimaryNames<T>(this EntityContext context, T entidade)
            where T : AEntidade
        {
            return context.FindPrimaryKeyProperties<T>()
                .Select(lnq => lnq.Name);
        }

        public static object[] FindPrimaryKeyValues<T>(this EntityContext context, T entidade)
            where T : AEntidade
        {
            return context.FindPrimaryKeyProperties<T>()
                .Select(lnq => entidade.GetPropertyValue(lnq.Name)).ToArray();
        }

        #endregion

        #region Métodos Privados

        private static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this EntityContext context)
            where T : AEntidade
        {
            return context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
        }

        private static object GetPropertyValue<T>(this T entity, string name)
        {
            return entity.GetType().GetProperty(name).GetValue(entity, null);
        }

        //public static object[] FindPrimaryNames<T>(this EntityContext<T> context, T entidade)
        //    where T : AEntidade
        //{

        //    var entry = context.Entry(entidade);
        //    object[] valoresPrimaryKey = entry.Metadata
        //        .FindPrimaryKey()
        //        .Properties
        //        .Select(lnq => entry.Property(lnq.Name).CurrentValue)
        //        .ToArray();

        //    return valoresPrimaryKey;
        //}

        #endregion
    }
}