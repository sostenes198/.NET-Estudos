namespace Estudos.Ioc.Ioc.Abstractions.DI
{
    public abstract class ADependencyInjector
    {
        public abstract TClass Resolve<TClass>() where TClass : class;

        public abstract void RegisterDependencyTransient<TInterface, TClass>() where TInterface : class where TClass : TInterface;

        public abstract void RegisterDependencyTransient<TClass>() where TClass : class;

        public abstract void TryRegisterDependencyTransient<TInterface, TClass>() where TInterface : class where TClass : TInterface;

        public abstract void TryRegisterDependencyTransient<TClass>() where TClass : class;

        public abstract void RegisterDependencyScoped<TInterface, TClass>() where TInterface : class where TClass : TInterface;

        public abstract void RegisterDependencyScoped<TClass>() where TClass : class;

        public abstract void TryRegisterDependencyScoped<TClass>() where TClass : class;

        public abstract void TryRegisterDependencyScoped<TInterface, TClass>() where TInterface : class where TClass : TInterface;

        public abstract void RegisterDepedencySingleton<TInterface, TClass>() where TInterface : class where TClass : TInterface;

        public abstract void RegisterDepedencySingleton<TClass>() where TClass : class;

        public abstract void TryRegisterDepedencySingleton<TInterface, TClass>() where TInterface : class where TClass : TInterface;

        public abstract void TryRegisterDepedencySingleton<TClass>() where TClass : class;
    }
}