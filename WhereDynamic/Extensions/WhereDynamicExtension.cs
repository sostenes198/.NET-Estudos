using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WhereDynamic.Atributo;

namespace WhereDynamic.Extensions
{
    public static class WhereDynamicExtension
    {

        private static string valorParameterExpression = "";

        public static IEnumerable<TSource> WhereDynamic<TSource, TFilter>(this IEnumerable<TSource> source, TFilter filtro)
        {
            valorParameterExpression = "";            

            IEnumerable<PropertyInfo> propriedadesFiltro = ListarPropertiesInfo(filtro);

            ParameterExpression expressaoParametro = ObterParameterExpression<TSource>();

            Func<TSource, bool> lambda = ConstruirLambdaExpression<TSource, TFilter>(filtro);

            return source.Where(lambda);
        }

        private static Func<TSource, bool> ConstruirLambdaExpression<TSource, TFilter>(TFilter filtro)
        {
            BinaryExpression expressao = null;

            IEnumerable<PropertyInfo> propriedadesFiltro = ListarPropertiesInfo(filtro);

            ParameterExpression expressaoParametro = ObterParameterExpression<TSource>();

            return ConstruirFiltro<TFilter, TSource>(expressao, expressaoParametro, propriedadesFiltro, filtro);
        }

        private static Func<TSource, bool> ConstruirFiltro<TFilter, TSource>(BinaryExpression expressao, ParameterExpression expressaoParametro,
            IEnumerable<PropertyInfo> propriedades, TFilter filtro)
        {
            MemberExpression NomePropriedade = null;
            ConstantExpression ValorPropriedade = null;

            foreach (PropertyInfo item in propriedades)
            {
                string nomeCampo = ((WhereDynamicAttribute)item.GetCustomAttribute(typeof(WhereDynamicAttribute), false)).NomeCampo;

                if (ObjectIsSimple(item.PropertyType))
                {
                    NomePropriedade = Expression.Property(expressaoParametro, nomeCampo);
                    ValorPropriedade = Expression.Constant(item.GetValue(filtro, null));

                    expressao = ConstruirExpressao(expressao, NomePropriedade, ValorPropriedade);
                }
                else
                {
                    if (!TryGetValueObjetoCompleto(filtro, item, out object objetoComplexo))
                        continue;

                    MemberExpression NomePropriedadeEntidadeComplexa = null;

                    foreach (Tuple<string, object> resultadoPropriedadeComplexa in ObterNomeEValorPropriedadeComplexa(objetoComplexo, item, nomeCampo))
                    {
                        foreach (string itemPropriedadeCompelxa in resultadoPropriedadeComplexa.Item1.Split('.'))
                        {
                            if (NomePropriedadeEntidadeComplexa == null)
                                NomePropriedadeEntidadeComplexa = Expression.PropertyOrField(expressaoParametro, itemPropriedadeCompelxa);
                            else
                                NomePropriedadeEntidadeComplexa = Expression.PropertyOrField(NomePropriedadeEntidadeComplexa, itemPropriedadeCompelxa);
                        }

                        NomePropriedade = NomePropriedadeEntidadeComplexa;
                        ValorPropriedade = Expression.Constant(resultadoPropriedadeComplexa.Item2);

                        expressao = ConstruirExpressao(expressao, NomePropriedade, ValorPropriedade);

                        NomePropriedadeEntidadeComplexa = null;
                    }
                }
            }

            return Expression.Lambda<Func<TSource, bool>>(expressao, expressaoParametro).Compile();
        }

        private static IEnumerable<Tuple<string, object>> ObterNomeEValorPropriedadeComplexa<TEntidade>(TEntidade entidade, PropertyInfo propriedade, string nomePropriedade)
        {
            IEnumerable<PropertyInfo> propriedadesEntidade = ListarPropertiesInfo(entidade);

            foreach (PropertyInfo item in propriedadesEntidade)
            {
                string nomeCampo = ((WhereDynamicAttribute)item.GetCustomAttribute(typeof(WhereDynamicAttribute), false)).NomeCampo;

                if (ObjectIsSimple(item.PropertyType))
                    yield return new Tuple<string, object>($"{nomePropriedade}.{nomeCampo}", item.GetValue(entidade, null));

                else
                {
                    if (!TryGetValueObjetoCompleto(entidade, item, out object objetoComplexo))
                        continue;

                    foreach (Tuple<string, object> resultado in ObterNomeEValorPropriedadeComplexa(objetoComplexo, item, $"{nomePropriedade}.{nomeCampo}"))
                    {
                        yield return resultado;
                    }
                }
            }
        }

        private static BinaryExpression ConstruirExpressao(BinaryExpression expressao, MemberExpression nomePropriedade, ConstantExpression valorPropriedade)
        {
            if (expressao == null)
                return Expression.Equal(nomePropriedade, valorPropriedade);

            BinaryExpression expressaoTemp = Expression.Equal(nomePropriedade, valorPropriedade);

            return Expression.And(expressao, expressaoTemp);

        }




        private static bool TryGetValueObjetoCompleto<TEntidade>(TEntidade entidade, PropertyInfo propriedade, out object objetoComplexo)
        {
            objetoComplexo = propriedade.GetValue(entidade, null);

            return objetoComplexo != null;
        }

        private static IEnumerable<PropertyInfo> ListarPropertiesInfo<TFilter>(TFilter filtro)
        {
            return filtro.GetType().GetProperties().Where(lnq => lnq.GetCustomAttributes(typeof(WhereDynamicAttribute), false).Any());
        }

        private static ParameterExpression ObterParameterExpression<TSource>()
        {
            ParameterExpression expressaoParametro = Expression.Parameter(typeof(TSource), $"lnq{valorParameterExpression}");

            int.TryParse(valorParameterExpression, out int valor);

            valorParameterExpression = (++valor).ToString();

            return expressaoParametro;
        }

        private static bool ObjectIsSimple(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return ObjectIsSimple(typeInfo.GetGenericArguments()[0]);
            }           

            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || typeInfo.Equals(typeof(string))
              || typeInfo.Equals(typeof(decimal))
              || typeInfo.Equals(typeof(short))
              || typeInfo.Equals(typeof(int))
              || typeInfo.Equals(typeof(long))
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
    }
}
