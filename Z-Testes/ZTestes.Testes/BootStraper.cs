using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZTestes.Testes.Cliente;
using ZTestes.Testes.Interceptors;
using ZTestes.Testes.Log;

namespace ZTestes.Testes
{
    public static class BootStraper
    {
        public static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.TryAddSingleton<ILogExample, LogExample>();
            DynamicProxyExtensions.AddDynamicProxy(serviceCollection, typeof(LogInterceptor), typeof(LogAInterceptor), typeof(LogBInterceptor));
            serviceCollection.TryAddProxiedScoped<IClienteService, ClienteService>();
            return serviceCollection.BuildServiceProvider();
        }
    }
}