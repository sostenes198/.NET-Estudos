using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Estudos.Ioc.Ioc.Abstractions.DI;
using Estudos.Ioc.Ioc.Communs;
using Estudos.Ioc.Ioc.Communs.Windsor;

namespace Estudos.Ioc.Ioc.DI.Windsor
{
    public class WindsorDependencyInjector : ADependencyInjector
    {
        public readonly WindsorContainer Container = ContainerDependencyInjector<WindsorContainer>.Container;

        public override TClass Resolve<TClass>()
        {
            return Container.Resolve<TClass>();
        }

        public override void RegisterDependencyTransient<TInterface, TClass>()
        {
            Container.Register(Component.For<TInterface>().ImplementedBy<TClass>().LifestyleTransient());
        }

        public override void RegisterDependencyTransient<TClass>()
        {
            Container.Register(Component.For<TClass>().LifestyleTransient());
        }

        public override void TryRegisterDependencyTransient<TClass>()
        {
            Container.Register(Component.For<TClass>().LifestyleScoped().OverridesExistingRegistration());
        }

        public override void TryRegisterDependencyTransient<TInterface, TClass>()
        {
            Container.Register(Component.For<TInterface>().ImplementedBy<TClass>().LifestyleTransient().OverridesExistingRegistration());
        }

        public override void RegisterDependencyScoped<TInterface, TClass>()
        {
            Container.Register(Component.For<TInterface>().ImplementedBy<TClass>().LifestyleScoped());
        }

        public override void RegisterDependencyScoped<TClass>()
        {
            Container.Register(Component.For<TClass>().LifestyleScoped());
        }

        public override void TryRegisterDependencyScoped<TInterface, TClass>()
        {
            Container.Register(Component.For<TInterface>().ImplementedBy<TClass>().LifestyleScoped().OverridesExistingRegistration());
        }

        public override void TryRegisterDependencyScoped<TClass>()
        {
            Container.Register(Component.For<TClass>().LifestyleTransient().OverridesExistingRegistration());
        }

        public override void RegisterDepedencySingleton<TInterface, TClass>()
        {
            Container.Register(Component.For<TInterface>().ImplementedBy<TClass>().LifestyleSingleton());
        }

        public override void RegisterDepedencySingleton<TClass>()
        {
            Container.Register(Component.For<TClass>().LifestyleSingleton());
        }

        public override void TryRegisterDepedencySingleton<TInterface, TClass>()
        {
            Container.Register(Component.For<TInterface>().ImplementedBy<TClass>().LifestyleSingleton().OverridesExistingRegistration());
        }

        public override void TryRegisterDepedencySingleton<TClass>()
        {
            Container.Register(Component.For<TClass>().LifestyleSingleton().OverridesExistingRegistration());
        }
    }
}