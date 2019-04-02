using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WhereDynamic.Atributo;

namespace WhereDynamic.Extensions
{
    public static class WhereDynamicExtension
    {
        public static IEnumerable<TSource> WhereDynamic<TSource, TFilter>(this IEnumerable<TSource> source, TFilter filtro)
        {
            BinaryExpression expressao = null;

            var propriedadesFiltro = ListarPropriedades(filtro);

            var expressaoParametro = Expression.Parameter(typeof(TSource), "lnq");

            expressao = ConstruirExpressao(expressao, expressaoParametro, propriedadesFiltro, filtro, source.First());

            var lambda = Expression.Lambda<Func<TSource, bool>>(expressao, expressaoParametro).Compile();

            return source.Where(lambda);
        }

        private static BinaryExpression ConstruirExpressao<TSource, TFilter>(BinaryExpression expressao, ParameterExpression expressaoParametro,
            IEnumerable<PropertyInfo> propriedades, TFilter filtro, TSource entidade)
        {
            MemberExpression NomePropriedade = null;
            ConstantExpression ValorPropriedade = null;

            foreach (var item in propriedades)
            {
                if (!TryGetAtributo(entidade.GetType().GetProperties(), item, out WhereDynamicAttribute atributo))
                    continue;


                if (ObjectIsSimple(item.PropertyType))
                {
                    NomePropriedade = Expression.Property(expressaoParametro, atributo.NomeCampo);
                    ValorPropriedade = Expression.Constant(item.GetValue(filtro, null));

                    if (expressao == null)
                        expressao = Expression.Equal(NomePropriedade, ValorPropriedade);
                    else
                    {
                        var expressaoTemp = Expression.Equal(NomePropriedade, ValorPropriedade);

                        expressao = Expression.And(expressao, expressaoTemp);
                    }
                }
                else
                {
                    var objetoComplexo = item.GetValue(filtro, null);                    


                    foreach (var resultadoPropriedadeComplexa in ObterNomeEValorPropriedadeComplexa(objetoComplexo, item, atributo.NomeCampo))
                    {
                        foreach (var itemPropriedadeCompelxa in resultadoPropriedadeComplexa.Item1.Split('.'))
                        {
                            NomePropriedade = Expression.PropertyOrField(expressaoParametro, itemPropriedadeCompelxa);
                        }

                        ValorPropriedade = Expression.Constant(resultadoPropriedadeComplexa.Item2);

                        if (expressao == null)
                            expressao = Expression.Equal(NomePropriedade, ValorPropriedade);
                        else
                        {
                            var expressaoTemp = Expression.Equal(NomePropriedade, ValorPropriedade);

                            expressao = Expression.And(expressao, expressaoTemp);
                        }
                    }                    
                }

                //if (expressao == null)
                //    expressao = Expression.Equal(NomePropriedade, ValorPropriedade);
                //else
                //{
                //    var expressaoTemp = Expression.Equal(NomePropriedade, ValorPropriedade);

                //    expressao = Expression.And(expressao, expressaoTemp);
                //}
            }

            return expressao;
        }

        private static IEnumerable<Tuple<string, object>> ObterNomeEValorPropriedadeComplexa<TEntidade>(TEntidade entidade, PropertyInfo propriedade, string nomePropriedade)
        {
            var propriedadesEntidade = entidade.GetType().GetProperties().Where(lnq => lnq.GetCustomAttributes(typeof(WhereDynamicAttribute), false).Any());

            foreach (var item in propriedadesEntidade)
            {

                if (ObjectIsSimple(item.PropertyType))
                {
                    string nomeCampo = ((WhereDynamicAttribute)item.GetCustomAttribute(typeof(WhereDynamicAttribute), false)).NomeCampo;
                    yield return new Tuple<string, object>($"{nomePropriedade}.{nomeCampo}", item.GetValue(entidade, null));
                }
                else
                {
                    var objetoComplexo = item.GetValue(entidade, null);

                    foreach (var resultado in ObterNomeEValorPropriedadeComplexa(objetoComplexo, item, nomePropriedade))
                        yield return resultado;
                }
            }
        }

        private static bool TryGetAtributo(IEnumerable<PropertyInfo> propriedadesEntidade, PropertyInfo propriedade, out WhereDynamicAttribute atributo)
        {
            var x = ((WhereDynamicAttribute)propriedade.GetCustomAttribute(typeof(WhereDynamicAttribute), false)).NomeCampo;
            var resultado = propriedadesEntidade
                .FirstOrDefault(lnq => lnq.Name == ((WhereDynamicAttribute)propriedade.GetCustomAttribute(typeof(WhereDynamicAttribute), false)).NomeCampo);

            atributo = (WhereDynamicAttribute)propriedade.GetCustomAttribute(typeof(WhereDynamicAttribute), false);

            return resultado != null;
        }

        private static IEnumerable<PropertyInfo> ListarPropriedades<TEntidade>(TEntidade entidade)
        {
            var atributos = entidade.GetType().GetProperties();

            return atributos.Where(lnq => lnq.GetCustomAttributes(typeof(WhereDynamicAttribute), false).Any());
        }

        private static bool ObjectIsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return ObjectIsSimple(typeInfo.GetGenericArguments()[0]);
            }

            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || typeInfo.Equals(typeof(string))
              || typeInfo.Equals(typeof(decimal))
              || typeInfo.Equals(typeof(Int16))
              || typeInfo.Equals(typeof(Int32))
              || typeInfo.Equals(typeof(Int64))
              || typeInfo.Equals(typeof(int));
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        //&& !lnq.GetValue(filtro, null).Equals(GetDefault(lnq.PropertyType))

        //private static IEnumerable<PropertyInfo> ListarPropriedadesComValor(FiltroPessoa filtro, IEnumerable<PropertyInfo> properties)
        //{
        //    var retorno = new List<PropertyInfo>();

        //    foreach (var item in properties)
        //    {
        //        var valorPropriedade = item.GetValue(filtro, null);
        //        var valorDefaultPropriedade = GetDefault(item.PropertyType);

        //        if (!valorPropriedade.Equals(valorDefaultPropriedade))
        //            retorno.Add(item);
        //    }

        //    return retorno;
        //}


    }
}
