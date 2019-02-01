using System;
using System.Collections.Generic;
using System.Linq;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using Estudos.Global.Helpers;
using Estudos.Global.NameSpace.Definicao;
using Estudos.IoC.Configuracao;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.IoC
{
    public static class IoCNetCore
    {
        public static void InjetarDependencias(this IServiceCollection services)
        {
            foreach (NameSpaceDefinition nameSpace in NamesSpacesInjection.NamesSpaces)
            {
                IEnumerable<Type> abstracoes = AssemblyHelper.ObterEntidadesAssemblyAbstrato(nameSpace.Abstracao);
                IEnumerable<Type> implementacoes = AssemblyHelper.ObterEntidadeAssemblyImplementacao(nameSpace.Implementacao);

                RegistrarDependencias(abstracoes, implementacoes, services);
            }
        }           

        private static void RegistrarDependencias(IEnumerable<Type> abstracoes, IEnumerable<Type> implemetacoes, IServiceCollection services)
        {
            foreach (var implementacao in implemetacoes)
            {
                var atributo = (IoCAttribute)implementacao.GetCustomAttributes(true)
                    .FirstOrDefault(lnq => lnq.GetType() == typeof(IoCAttribute));
                var abstracao = implementacao.GetInterfaces()
                    .FirstOrDefault(lnq => abstracoes.Any(o => o.Name == lnq.Name));

                if (abstracao != null)
                    RegistrarDependencia(services, atributo.LifeStyleIoCEnum, abstracao, implementacao);
                else
                    RegistrarDependencia(services, atributo.LifeStyleIoCEnum, implementacao);
            }
        }

        private static void RegistrarDependencia(IServiceCollection services, LifeStyleIoCEnum lifeStyleIoCEnum, Type abstracao, Type implementacao)
        {
            switch (lifeStyleIoCEnum)
            {
                case LifeStyleIoCEnum.Scoped: services.AddScoped(abstracao, implementacao); break;
                case LifeStyleIoCEnum.Singleton: services.AddSingleton(abstracao, implementacao); break;
                default: RegistrarDependencia(services, abstracao, implementacao); break;
            }
        }

        private static void RegistrarDependencia(IServiceCollection services, LifeStyleIoCEnum lifeStyleIoCEnum, Type implementacao)
        {
            switch (lifeStyleIoCEnum)
            {
                case LifeStyleIoCEnum.Scoped: services.AddScoped(implementacao); break;
                case LifeStyleIoCEnum.Singleton: services.AddSingleton(implementacao); break;
                default: RegistrarDependencia(services, implementacao); break;
            }
        }

        private static void RegistrarDependencia(IServiceCollection services, Type implementacao)
        {
            services.AddTransient(implementacao);
        }

        private static void RegistrarDependencia(IServiceCollection services, Type abstracao, Type implementacao)
        {
            services.AddTransient(abstracao, implementacao);
        }
    }
}
