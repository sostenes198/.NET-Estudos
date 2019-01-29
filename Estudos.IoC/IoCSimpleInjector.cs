using SimpleInjector;
using System.Linq;
using Estudos.Abstract.Dominio.Entidades;
using System;
using System.Collections.Generic;
using Estudos.Global.Atributos;
using Estudos.Global.Enuns;
using Estudos.IoC.Helpers;
using Estudos.Dominio.Entidades;

namespace Estudos.IoC
{
    public static class IoCSimpleInjector
    {
        static Container _container;

        public static Container InjetarDependencias()
        {
            if (_container != null)
                return _container;

            _container = new Container();

            RegistrarDominio();
            _container.Verify();

            return _container;
        }

        private static void RegistrarDominio()
        {
            var dominioAbstrato = AssemblyHelper.ObterEntidadesAssemblyAbstrato<IEntidade>();
            var dominio = AssemblyHelper.ObterEntidadeAssemblyImplementacao<AEntidade>();

            RegistrarDependencias(dominioAbstrato, dominio);
        }      

        private static void RegistrarDependencias(IEnumerable<Type> abstracoes, IEnumerable<Type> implementacoes)
        {
            foreach (var implementacao in implementacoes)
            {
                var atributo = (IoCAttribute)implementacao.GetCustomAttributes(true)
                    .FirstOrDefault(lnq => lnq.GetType() == typeof(IoCAttribute));
                var abstracao = implementacao.GetInterfaces()
                    .FirstOrDefault(lnq => abstracoes.Any(o => o.Name == lnq.Name));

                Lifestyle lifestyle = RetornarTipoLyfeStyleInjecao(atributo.LifeStyleIoCEnum);
                if (abstracao != null)
                    _container.Register(abstracao, implementacao, lifestyle);
                else
                    _container.Register(implementacao);
            }
        }

        private static Lifestyle RetornarTipoLyfeStyleInjecao(LifeStyleIoCEnum lifeStyle)
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
