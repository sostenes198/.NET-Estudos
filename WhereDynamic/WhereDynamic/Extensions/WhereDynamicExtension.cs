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

            var propriedadesFiltro = ListarPropertiesInfo(filtro);

            var expressaoParametro = ObterParameterExpression(typeof(TSource));

            var lambda = ConstruirLambdaExpression<TSource, TFilter>(filtro).Compile();

            return source.Where(lambda);
        }

        private static Expression<Func<TSource, bool>> ConstruirLambdaExpression<TSource, TFilter>(TFilter filtro)
        {
            BinaryExpression expressao = null;

            var propriedadesFiltro = ListarPropertiesInfo(filtro);

            var expressaoParametro = ObterParameterExpression(typeof(TSource));

            return ConstruirFiltro<TFilter, TSource>(expressao, expressaoParametro, propriedadesFiltro, filtro);
        }

        private static Expression<Func<TSource, bool>> ConstruirFiltro<TFilter, TSource>(BinaryExpression expressao, ParameterExpression expressaoParametro,
            IEnumerable<PropertyInfo> propriedades, TFilter filtro)
        {
            MemberExpression NomePropriedade = null;
            ConstantExpression ValorPropriedade = null;

            foreach (var item in propriedades)
            {
                var nomeCampo = ObterAtributoWhereDynamic(item).NomeCampo;

                if (!TryGetValueObjetoCompleto(filtro, item, out var obj))
                    continue;

                if (ObjectIsSimple(obj))
                {
                    NomePropriedade = Expression.Property(expressaoParametro, nomeCampo);
                    ValorPropriedade = Expression.Constant(item.GetValue(filtro, null));
                }
                else if (ObjectIsList(obj))
                {
                    var typeTFilter = item.PropertyType.GetGenericArguments()[0];
                    var valuesTSource = ((IEnumerable) obj).ObterPrimeiroElemento();
                    var typeTSource = typeof(TSource).GetProperty(nomeCampo).PropertyType.GetGenericArguments()[0];

                    var predicateToAny = (Expression) typeof(WhereDynamicExtension)
                        .GetMethod("ConstruirLambdaExpression", BindingFlags.NonPublic | BindingFlags.Static)
                        .MakeGenericMethod(typeTSource, typeTFilter)
                        .Invoke(null, new[] {valuesTSource});

                    NomePropriedade = Expression.Property(expressaoParametro, nomeCampo);

                    expressao = ConstruirExpressaoAny(expressao, NomePropriedade, predicateToAny);
                }
                else
                {
                    MemberExpression NomePropriedadeEntidadeComplexa = null;

                    foreach (var resultadoPropriedadeComplexa in ObterNomeEValorPropriedadeComplexa(obj, item, nomeCampo))
                    {
                        foreach (var itemPropriedadeCompelxa in resultadoPropriedadeComplexa.Item1.Split('.'))
                            if (NomePropriedadeEntidadeComplexa == null)
                                NomePropriedadeEntidadeComplexa = Expression.PropertyOrField(expressaoParametro, itemPropriedadeCompelxa);
                            else
                                NomePropriedadeEntidadeComplexa = Expression.PropertyOrField(NomePropriedadeEntidadeComplexa, itemPropriedadeCompelxa);

                        NomePropriedade = NomePropriedadeEntidadeComplexa;
                        ValorPropriedade = Expression.Constant(resultadoPropriedadeComplexa.Item2);

                        NomePropriedadeEntidadeComplexa = null;
                    }
                }

                expressao = ConstruirExpressao(expressao, NomePropriedade, ValorPropriedade);
            }

            return Expression.Lambda<Func<TSource, bool>>(expressao, expressaoParametro);
        }

        private static IEnumerable<Tuple<string, object>> ObterNomeEValorPropriedadeComplexa<TEntidade>(TEntidade entidade, PropertyInfo propriedade, string nomePropriedade)
        {
            var propriedadesEntidade = ListarPropertiesInfo(entidade);

            foreach (var item in propriedadesEntidade)
            {
                var nomeCampo = ObterAtributoWhereDynamic(item).NomeCampo;

                if (ObjectIsSimple(item.PropertyType))
                {
                    yield return new Tuple<string, object>($"{nomePropriedade}.{nomeCampo}", item.GetValue(entidade, null));
                }

                else
                {
                    if (!TryGetValueObjetoCompleto(entidade, item, out var objetoComplexo))
                        continue;

                    foreach (var resultado in ObterNomeEValorPropriedadeComplexa(objetoComplexo, item, $"{nomePropriedade}.{nomeCampo}")) yield return resultado;
                }
            }
        }

        private static BinaryExpression ConstruirExpressao(BinaryExpression expressao, MemberExpression nomePropriedade, ConstantExpression valorPropriedade)
        {
            if (expressao == null)
                return Expression.Equal(nomePropriedade, valorPropriedade);

            var expressaoTemp = Expression.Equal(nomePropriedade, valorPropriedade);

            return Expression.And(expressao, expressaoTemp);
        }

        private static BinaryExpression ConstruirExpressaoAny(BinaryExpression expressao, MemberExpression nomePropriedade, Expression valorPropriedade)
        {
            
//            var x = Expression.Call(
//                valorPropriedade,
//                typeof(IEnumerable).GetMethod("Any")
//            );
            
            var expressaoTemp = Expression.Equal(nomePropriedade, valorPropriedade);

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

        private static ParameterExpression ObterParameterExpression(Type type)
        {
            var expressaoParametro = Expression.Parameter(type, $"lnq{valorParameterExpression}");

            int.TryParse(valorParameterExpression, out var valor);

            valorParameterExpression = (++valor).ToString();

            return expressaoParametro;
        }

        private static WhereDynamicAttribute ObterAtributoWhereDynamic(PropertyInfo property)
        {
            return (WhereDynamicAttribute) property.GetCustomAttribute(typeof(WhereDynamicAttribute), false);
        }

        private static bool ObjectIsSimple(object obj)
        {
            if (obj == null)
                return false;

            var type = obj.GetType();

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                // nullable type, check if the nested type is simple.
                return ObjectIsSimple(typeInfo.GetGenericArguments()[0]);

            return typeInfo.IsPrimitive
                   || typeInfo.IsEnum
                   || typeInfo.Equals(typeof(string))
                   || typeInfo.Equals(typeof(decimal))
                   || typeInfo.Equals(typeof(short))
                   || typeInfo.Equals(typeof(int))
                   || typeInfo.Equals(typeof(long))
                   || typeInfo.Equals(typeof(int));
        }

        private static bool ObjectIsList(object obj)
        {
            return obj.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType) return Activator.CreateInstance(type);
            return null;
        }
    }
}