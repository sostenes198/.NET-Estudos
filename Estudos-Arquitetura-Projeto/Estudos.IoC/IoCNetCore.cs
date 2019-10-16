using System;
using System.Collections.Generic;
using System.Linq;
using Estudos.Api.GraphQL.GraphQL_Schema.Schema_Cardapio;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using Estudos.Global.Helpers;
using Estudos.IoC.Configuracao;
using Estudos.Repositorio.EntityFrameworkCore;
using Estudos.Repositorio.EntityFrameworkCore.Base;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.IoC
{
    public static class IoCNetCore
    {
        public static IServiceCollection IoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContext, EntityContext>();
            services.AddDbContext<EntityContext>(lnq => lnq.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.InjetarDependencias();

            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new CardapioCategoriaSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            return services;
        }

        private static void InjetarDependencias(this IServiceCollection services)
        {
            foreach (var nameSpace in NamesSpacesInjection.NamesSpaces)
                if (nameSpace.Implementacao is string)
                {
                    var abstracoes = AssemblyHelper.ObterEntidadesAssemblyAbstrato((string) nameSpace.Abstracao);
                    var implementacoes = AssemblyHelper.ObterEntidadeAssemblyImplementacao((string) nameSpace.Implementacao);

                    RegistrarDependencias(abstracoes, implementacoes, services);
                }
                else if (nameSpace.Abstracao is Type && nameSpace.Implementacao is Type)
                {
                    RegistrarDependencia((Type) nameSpace.Abstracao, (Type) nameSpace.Implementacao, services);
                }
        }

        private static void RegistrarDependencia(Type abstracao, Type implementacao, IServiceCollection services)
        {
            var atributo = (LifeStyleAttribute) implementacao.GetCustomAttributes(true)
                .FirstOrDefault(lnq => lnq.GetType() == typeof(LifeStyleAttribute));

            if (abstracao != null)
                RegistrarDependencia(services, atributo.LifeStyleIoCEnum, abstracao, implementacao);
            else
                RegistrarDependencia(services, atributo.LifeStyleIoCEnum, implementacao);
        }

        private static void RegistrarDependencias(IEnumerable<Type> abstracoes, IEnumerable<Type> implemetacoes, IServiceCollection services)
        {
            foreach (var implementacao in implemetacoes)
            {
                var atributo = (IoCAttribute) implementacao.GetCustomAttributes(true)
                    .FirstOrDefault(lnq => lnq.GetType() == typeof(IoCAttribute));
                var abstracao = abstracoes
                    .LastOrDefault(lnq => lnq.Name == "I" + implementacao.Name);

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
                case LifeStyleIoCEnum.Scoped:
                    services.AddScoped(abstracao, implementacao);
                    break;
                case LifeStyleIoCEnum.Singleton:
                    services.AddSingleton(abstracao, implementacao);
                    break;
                default:
                    RegistrarDependencia(services, abstracao, implementacao);
                    break;
            }
        }

        private static void RegistrarDependencia(IServiceCollection services, LifeStyleIoCEnum lifeStyleIoCEnum, Type implementacao)
        {
            switch (lifeStyleIoCEnum)
            {
                case LifeStyleIoCEnum.Scoped:
                    services.AddScoped(implementacao);
                    break;
                case LifeStyleIoCEnum.Singleton:
                    services.AddSingleton(implementacao);
                    break;
                default:
                    RegistrarDependencia(services, implementacao);
                    break;
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