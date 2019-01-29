// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Linq.Expressions;
// using System.Reflection;
// using HoursAutomation.Domain.Contracts.Base;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace HoursAutomation.Domain.Extensions
// {
//     internal static class ChainConfiguratorExtensions1
//     {
//         internal interface IChainConfiguratorStep<in T>
//             where T : IChainOfResponsibility
//         {
//             IChainConfiguratorStep<T> Add<TImplementation>() where TImplementation : IStepChainOfResponsibility;
//
//             IChainConfigurator AddLastStep<TImplementation>() where TImplementation : ILastStepChainOfResponsibility;
//         }
//
//         internal interface IChainConfigurator
//         {
//             IChainConfiguratorExecution<TExecutor> ConfigureChainOfResponsability<TExecutor>() where TExecutor : IExecutorChainOfResponsability;
//         }
//
//         internal interface IChainConfiguratorExecution<in T>
//             where T : IExecutorChainOfResponsability
//         {
//             void ConfigureExecution();
//         }
//
//         
//         public static IChainConfiguratorStep<T> AddChain<T>(this IServiceCollection services)
//             where T : class, IChainOfResponsibility
//         {
//             return new ChainConfiguratorStepImplementation<T>(services);
//         }
//
//         private class ExecutorChainOfResponsabilityImplementation : IExecutorChainOfResponsability
//         {
//             private readonly IStepChainOfResponsibility _firstStepChain;
//
//             public ExecutorChainOfResponsabilityImplementation(IStepChainOfResponsibility firstStepChain)
//             {
//                 _firstStepChain = firstStepChain;
//             }
//
//             public void Run()
//             {
//                 _firstStepChain.Handle();
//             }
//         }
//
//         private class ChainConfiguratorImplementation<T> : IChainConfigurator
//         {
//             private readonly IServiceCollection _services;
//             private readonly Type _interfaceType;
//             private readonly IList<Type> _types;
//             private Type _firstType;
//             private Type _lastType;
//
//             public ChainConfiguratorImplementation(IServiceCollection services, IList<Type> types)
//             {
//                 _services = services;
//                 _interfaceType = typeof(T);
//                 _types = types;
//             }
//
//             public IChainConfiguratorExecution<TExecutor> ConfigureChainOfResponsability<TExecutor>()
//                 where TExecutor : IExecutorChainOfResponsability
//             {
//                 VerifySteps();
//
//                 _firstType = _types.First();
//                 _lastType = _types.Last();
//                 foreach (var type in _types)
//                 {
//                     ConfigureType(type);
//                 }
//
//                 return new ChainConfiguratorExecutionImplementation<TExecutor>(_services, _firstType);
//             }
//
//             private void VerifySteps()
//             {
//                 if (_types.Count == 0)
//                     throw new InvalidOperationException($"No implementation defined for {_interfaceType.Name}");
//             }
//
//             private void ConfigureType(Type currentType)
//             {
//                 var nextType = GetNextType(currentType);
//                 var constructorInfo = GetConstructorInfo(currentType);
//                 var parameter = Expression.Parameter(typeof(IServiceProvider), "x");
//                 var ctorParameters = GetConstructorParameters(constructorInfo, nextType, parameter);
//                 var body = Expression.New(constructorInfo, ctorParameters.ToArray());
//                 InjectType(currentType, body, parameter);
//             }
//
//             private void InjectType(Type currentType, NewExpression body, ParameterExpression parameter)
//             {
//                 var resolveType = _firstType == currentType ? typeof(IStepChainOfResponsibility) : _lastType == currentType ? typeof(ILastStepChainOfResponsibility) : currentType;
//                 var expressionType = Expression.GetFuncType(typeof(IServiceProvider), resolveType);
//                 var expression = Expression.Lambda(expressionType, body, parameter);
//
//                 ResolveInjection(resolveType, expression);
//             }
//
//             private void ResolveInjection(Type serviceType, LambdaExpression expression)
//             {
//                 Func<IServiceProvider, object> expressionCompiled = (Func<IServiceProvider, object>) expression.Compile();
//                 _services.AddScoped(serviceType, expressionCompiled);
//             }
//
//             private Type GetNextType(Type currentType) => _types.SkipWhile(x => x != currentType).SkipWhile(x => x == currentType).FirstOrDefault();
//
//             private ConstructorInfo GetConstructorInfo(Type currentType) => currentType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();
//
//             private IEnumerable<Expression> GetConstructorParameters(ConstructorInfo constructorInfo, Type nextType, Expression parameter) =>
//                 constructorInfo.GetParameters().Select(parameterInfo =>
//                 {
//                     if (_interfaceType.IsAssignableFrom(parameterInfo.ParameterType))
//                     {
//                         if (nextType == null)
//                             return Expression.Constant(null, _interfaceType);
//
//                         return Expression.Call(typeof(ServiceProviderServiceExtensions), "GetRequiredService", new[] {nextType}, parameter);
//                     }
//
//                     return (Expression) Expression.Call(typeof(ServiceProviderServiceExtensions), "GetRequiredService", new[] {parameterInfo.ParameterType}, parameter);
//                 });
//         }
//
//         private class ChainConfiguratorStepImplementation<T> : IChainConfiguratorStep<T>
//             where T : class, IChainOfResponsibility
//         {
//             private readonly IServiceCollection _services;
//             private readonly IList<Type> _types;
//
//             public ChainConfiguratorStepImplementation(IServiceCollection services)
//             {
//                 _services = services;
//                 _types = new List<Type>();
//             }
//
//             public IChainConfiguratorStep<T> Add<TImplementation>() where TImplementation : IStepChainOfResponsibility
//             {
//                 _types.Add(typeof(TImplementation));
//                 return this;
//             }
//
//             public IChainConfigurator AddLastStep<TImplementation>() where TImplementation : ILastStepChainOfResponsibility
//             {
//                 _types.Add(typeof(TImplementation));
//                 return new ChainConfiguratorImplementation<T>(_services, _types);
//             }
//         }
//
//         private class ChainConfiguratorExecutionImplementation<T> : IChainConfiguratorExecution<T>
//             where T : IExecutorChainOfResponsability
//         {
//             private readonly IServiceCollection _services;
//             private readonly Type _firstElementChain;
//             private readonly Type _interfaceType;
//
//             public ChainConfiguratorExecutionImplementation(IServiceCollection services, Type firstElementChain)
//             {
//                 _services = services;
//                 _firstElementChain = firstElementChain;
//                 _interfaceType = typeof(T);
//             }
//
//             public void ConfigureExecution()
//             {
//                 if (_firstElementChain == default)
//                     throw new InvalidOperationException($"No implementation defined for {_interfaceType.Name}");
//
//                 _services.AddScoped<IExecutorChainOfResponsability>(provider =>
//                 {
//                     var firstElementChain = provider.GetRequiredService(typeof(IStepChainOfResponsibility));
//                     return new ExecutorChainOfResponsabilityImplementation(null);
//                 });
//             }
//         }
//     }
// }