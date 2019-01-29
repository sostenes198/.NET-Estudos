using System;
using System.Collections.Generic;
using System.Linq;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using Estudos.Global.Helpers;
using Estudos.IoC.Configuracao;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Estudos.IoC
{
    public static class IoCSimpleInjector
    {
        private static Container _container;

        public static Container InjetarDependencias()
        {
            if (_container != null) return _container;

            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InjetarTodasDependencias();
            _container.Verify();

            return _container;
        }

        private static void InjetarTodasDependencias()
        {
            foreach (var nameSpace in NamesSpacesInjection.NamesSpaces)
                if (nameSpace.Abstracao is string && nameSpace.Implementacao is string)
                {
                    var abstracoes = AssemblyHelper.ObterEntidadesAssemblyAbstrato((string) nameSpace.Abstracao);
                    var implementacoes = AssemblyHelper.ObterEntidadeAssemblyImplementacao((string) nameSpace.Implementacao);

                    RegistrarDependencias(abstracoes, implementacoes);
                }
                else if (nameSpace.Abstracao is Type && nameSpace.Implementacao is Type)
                {
                    RegistrarDependencias((Type) nameSpace.Abstracao, (Type) nameSpace.Implementacao);
                }
        }

        private static void RegistrarDependencias(IEnumerable<Type> abstracoes, IEnumerable<Type> implementacoes)
        {
            foreach (var implementacao in implementacoes)
            {
                var atributo = (IoCAttribute) implementacao.GetCustomAttributes(true)
                    .FirstOrDefault(lnq => lnq.GetType() == typeof(IoCAttribute));
                var abstracao = implementacao.GetInterfaces()
                    .LastOrDefault(lnq => abstracoes.Any(o => o.Name == lnq.Name));

                RegistrarDependencias(abstracao, implementacao, atributo.LifeStyleIoCEnum);
            }
        }

        private static void RegistrarDependencias(Type abstracao, Type implementacao)
        {
            var atributo = (LifeStyleAttribute) implementacao.GetCustomAttributes(true)
                .FirstOrDefault(lnq => lnq.GetType() == typeof(LifeStyleAttribute));

            RegistrarDependencias(abstracao, implementacao, atributo.LifeStyleIoCEnum);
        }

        private static void RegistrarDependencias(Type abstracao, Type implementacao, LifeStyleIoCEnum lifeStyle)
        {
            var lifestyle = RetornarTipoLyfeStyleInjecao(lifeStyle);
            if (abstracao != null)
                _container.Register(abstracao, implementacao, lifestyle);
            else
                _container.Register(implementacao);
        }

        private static Lifestyle RetornarTipoLyfeStyleInjecao(LifeStyleIoCEnum? lifeStyle)
        {
            switch (lifeStyle)
            {
                case LifeStyleIoCEnum.Scoped: return Lifestyle.Scoped;
                case LifeStyleIoCEnum.Singleton: return Lifestyle.Singleton;
                default: return Lifestyle.Transient;
            }
        }
    }
}