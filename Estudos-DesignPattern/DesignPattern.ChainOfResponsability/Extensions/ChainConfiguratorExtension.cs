using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: InternalsVisibleTo("DesignPattern.Tests")]
[assembly: InternalsVisibleTo("DesignPattern.Tests.ChainOfResponsibility.Fixture")]

namespace DesignPattern.ChainOfResponsability.Extensions
{
    public static class ChainConfiguratorExtension
    {
        private static readonly Type _typeChainOfResponsibility = typeof(IChainOfResponsibility);
        private static readonly Type _typeBaseStepChainOfResponsibility = typeof(IBaseStepChainOfResponsibility);

        internal static IChainConfiguratorStep AddChain<T>(this IServiceCollection services)
            where T : IChainOfResponsibility
        {
            var typeChain = typeof(T);
            ValidateTypeChainOfResponsibility(typeChain);
            return new ChainConfiguratorStepImplementation(services, typeChain);
        }

        private static void ValidateTypeChainOfResponsibility(Type typeChain)
        {
            if (typeChain == _typeChainOfResponsibility)
                throw new InvalidOperationException($"Parameter needs to inherit from {_typeChainOfResponsibility.Name}.");
            if (typeChain.IsClass == false || typeChain.IsClass && typeChain.IsAbstract)
                throw new InvalidOperationException($"Parameter {typeChain.Name} needs to be a class.");
        }

        internal interface IChainConfiguratorStep
        {
            IChainConfiguratorStep AddStep<TImplementation>()
                where TImplementation : IStepChainOfResponsibility;

            IChainConfigurator AddLastStep<TImplementation>()
                where TImplementation : ILastStepChainOfResponsibility;
        }

        internal interface IChainConfigurator
        {
            void Configure();
        }

        private readonly struct TypeChain
        {
            public Type Implementation { get; }

            public TypeChain(Type typeImplementation)
            {
                Implementation = typeImplementation;
            }
        }

        private class ChainConfiguratorStepImplementation : IChainConfiguratorStep
        {
            private readonly IServiceCollection _services;
            private readonly Type _typeChain;
            private readonly IList<TypeChain> _steps;

            public ChainConfiguratorStepImplementation(IServiceCollection services, Type typeChain)
            {
                _services = services;
                _typeChain = typeChain;
                _steps = new List<TypeChain>();
            }

            public IChainConfiguratorStep AddStep<TImplementation>()
                where TImplementation : IStepChainOfResponsibility
            {
                AddStep(typeof(TImplementation));
                return this;
            }

            public IChainConfigurator AddLastStep<TImplementation>() where TImplementation : ILastStepChainOfResponsibility
            {
                AddStep(typeof(TImplementation));
                return new ChainConfiguratorImplementation(_services, _steps, _typeChain);
            }

            private void AddStep(Type implementationStep)
            {
                _steps.Add(new TypeChain(implementationStep));
            }
        }

        private class ChainConfiguratorImplementation : IChainConfigurator
        {
            private readonly IServiceCollection _services;
            private readonly IList<TypeChain> _steps;
            private readonly Type _typeChain;
            private readonly TypeChain _firstElementChain;
            private readonly IList<(Type, object)> _implementationSteps;
            private IServiceProvider _provider;

            public ChainConfiguratorImplementation(IServiceCollection services, IList<TypeChain> steps, Type typeChain)
            {
                _services = services;
                _steps = steps;
                _typeChain = typeChain;
                _firstElementChain = _steps.First();
                _implementationSteps = new List<(Type, object)>();
            }

            public void Configure()
            {
                ValidateDuplicateSteps();
                _services.TryAddScoped(_typeChain, provider =>
                {
                    _provider = provider;
                    ConfigureSteps();
                    var constructorInfo = GetConstructorWithMoreParameters(_typeChain.GetConstructors());
                    var parameters = GetConstructorParameters(constructorInfo, _firstElementChain.Implementation);
                    return Activator.CreateInstance(_typeChain, parameters);
                });
            }

            private void ValidateDuplicateSteps()
            {
                if (_steps.GroupBy(lnq => lnq.Implementation).Any(lnq => lnq.Count() > 1))
                    throw new InvalidOperationException("There can be no duplicate steps in the chain.");
            }

            private void ConfigureSteps()
            {
                foreach (var typeChain in _steps.Reverse().ToList())
                {
                    var constructorInfo = GetConstructorWithMoreParameters(typeChain.Implementation.GetConstructors());
                    var nextStepType = GetNextType(typeChain.Implementation);
                    var parameters = GetConstructorParameters(constructorInfo, nextStepType);
                    _implementationSteps.Add((typeChain.Implementation, Activator.CreateInstance(typeChain.Implementation, parameters)));
                }
            }


            private ConstructorInfo GetConstructorWithMoreParameters(ConstructorInfo[] constructorInfos)
                => constructorInfos.OrderByDescending(lnq => lnq.GetParameters().Length).First();

            private Type GetNextType(Type typeImplementation) => _steps.SkipWhile(x => x.Implementation != typeImplementation)
                .SkipWhile(x => x.Implementation == typeImplementation).Select(lnq => lnq.Implementation).FirstOrDefault();

            private object[] GetConstructorParameters(ConstructorInfo constructorInfo, Type nextStepType)
            {
                var parameters = new List<object>();
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    var param = GetParameter(parameterInfo.ParameterType, nextStepType);
                    parameters.Add(param);
                }

                return parameters.ToArray();
            }

            private object GetParameter(Type parameterType, Type nextStepType) =>
                nextStepType != default && _typeBaseStepChainOfResponsibility.IsAssignableFrom(nextStepType) && _typeBaseStepChainOfResponsibility.IsAssignableFrom(parameterType)
                    ? _implementationSteps.First(lnq => lnq.Item1 == nextStepType).Item2
                    : _provider.GetRequiredService(parameterType);
        }
    }
}