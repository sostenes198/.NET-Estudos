using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.ExpressionThree;

public static class ConsumerFilterExtensions
{
    private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, object>> s_expressionInstances = new ConcurrentDictionary<Type, Func<IServiceProvider, object>>();

    private static readonly ParameterExpression s_providerArg = Expression.Parameter(typeof(IServiceProvider), "serviceProvider");

    private static readonly MethodInfo s_getServiceInfo = typeof(ConsumerFilterExtensions).GetMethod(nameof(GetService), BindingFlags.NonPublic | BindingFlags.Static)!;

    /// <summary>
    /// Add filter to the consumer.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
    /// <param name="envelopeType">The <see cref="IConsumerFilter{TEnvelope}"/> EnvelopType.</param>
    /// <param name="filters">The <see cref="IConsumerFilter{TEnvelope}"/> instances.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when filter class contains Multiple constructors or not contain required parameter in constructor See <see cref="consumerFilterTypeParameterConstructorRequired"/>.
    /// </exception>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddConsumerFilters(this IServiceCollection services, Type envelopeType, IList<Type> filters)
    {
        var consumerFilterInitializeType = typeof(ConsumerFilterInitialize<>).MakeGenericType(envelopeType);

        services.TryAddScoped(consumerFilterInitializeType,
                              provider =>
                              {
                                  if (s_expressionInstances.ContainsKey(consumerFilterInitializeType))
                                  {
                                      return s_expressionInstances[consumerFilterInitializeType].Invoke(provider);
                                  }

                                  s_expressionInstances.TryAdd(consumerFilterInitializeType, MakeExpressionConsumerFilterInstance(filters, envelopeType));

                                  return s_expressionInstances[consumerFilterInitializeType].Invoke(provider);
                              });

        return services;
    }

    private static Func<IServiceProvider, object> MakeExpressionConsumerFilterInstance(IList<Type> filters, Type envelopeType)
    {
        var consumerFilterTypeParameterConstructorRequired = typeof(IConsumerFilter<>);

        var lastFilterType = filters.LastOrDefault();

        var lastFilterInstanceExpression = MakeLastFilterInstanceExpression(envelopeType);

        Expression expressionInstance = filters
           .Reverse()
           .Aggregate<Type, Expression>(null, (current, filterType) => MakeExpressionFilter(filterType, consumerFilterTypeParameterConstructorRequired, lastFilterType == filterType ? lastFilterInstanceExpression : current));

        var expressionConsumerFilter = MakeExpressionFilter(typeof(ConsumerFilterInitialize<>).MakeGenericType(envelopeType), consumerFilterTypeParameterConstructorRequired, expressionInstance ?? lastFilterInstanceExpression);

        return Expression.Lambda<Func<IServiceProvider, object>>(expressionConsumerFilter, s_providerArg).Compile();
    }

    private static Expression MakeLastFilterInstanceExpression(Type envelopeType)
    {
        var expressionConsumerBase = MakeExpressionServiceProvider(typeof(IConsumerBase<>).MakeGenericType(envelopeType));

        return MakeExpressionFilter(typeof(LastConsumerFilter<>).MakeGenericType(envelopeType), typeof(IConsumerBase<>), expressionConsumerBase);
    }

    private static Expression MakeExpressionFilter(Type filterType, Type constructorParameterRequired, Expression expression)
    {
        var constructorInfo = GetConstructor(filterType);
        var parametersInfo = GetParametersConstructorInfo(constructorInfo, constructorParameterRequired);
        int indexConstructorParameterRequired = GetIndexConstructorParameterRequited(parametersInfo, constructorParameterRequired);

        var methodArguments = new Expression[parametersInfo.Length];
        methodArguments[indexConstructorParameterRequired] = expression;

        for (int i = 0; i < parametersInfo.Length; i++)
        {
            if (i != indexConstructorParameterRequired)
            {
                methodArguments[i] = MakeExpressionServiceProvider(parametersInfo[i].ParameterType);
            }
        }

        return Expression.New(constructorInfo, methodArguments);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ConstructorInfo GetConstructor(Type filterType)
    {
        var constructorInfos = filterType.GetConstructors();

        if (constructorInfos.Length > 1)
        {
            throw new InvalidOperationException($"Failed to create filter {filterType}\nMultiple constructors, must contain only 1");
        }

        return constructorInfos[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ParameterInfo[] GetParametersConstructorInfo(ConstructorInfo constructorInfo, Type constructorParameterRequired)
    {
        var parameterInfos = constructorInfo.GetParameters();

        string typeName = constructorParameterRequired.GetGenericTypeDefinition().ToString();

        if (parameterInfos.Count(lnq => lnq.ParameterType.IsGenericType && constructorParameterRequired.IsAssignableFrom(lnq.ParameterType.GetGenericTypeDefinition())) != 1)
        {
            throw new InvalidOperationException($"Failed to create filter {constructorInfo.DeclaringType}\nConstructor must contain one single parameter: '{typeName}'");
        }

        return parameterInfos.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetIndexConstructorParameterRequited(ParameterInfo[] parameterInfos, Type constructorParameterRequired)
    {
        return parameterInfos.Single(lnq => lnq.ParameterType.IsGenericType && lnq.ParameterType.GetGenericTypeDefinition() == constructorParameterRequired).Position;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static object GetService(IServiceProvider sp, Type type) => sp.GetRequiredService(type);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Expression MakeExpressionServiceProvider(Type parameterType)
    {
        var parameterTypeExpression = new Expression[] {s_providerArg, Expression.Constant(parameterType, typeof(Type))};
        var getServiceCall = Expression.Call(s_getServiceInfo, parameterTypeExpression);

        return Expression.Convert(getServiceCall, parameterType);
    }
}