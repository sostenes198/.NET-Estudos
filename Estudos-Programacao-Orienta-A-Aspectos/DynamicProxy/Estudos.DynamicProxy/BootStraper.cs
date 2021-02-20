using System;
using Estudos.DynamicProxy.Contracts;
using Estudos.DynamicProxy.Entities;
using Estudos.DynamicProxy.Interceptors.LogA;
using Estudos.DynamicProxy.Interceptors.LogB;
using Estudos.DynamicProxy.Interceptors.LogC;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.DynamicProxy
{
    public static class BootStraper
    {
        public static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            DynamicProxyExtensions.AddGenericDynamicProxy(serviceCollection, typeof(LogAInterceptor), typeof(LogBInterceptor), typeof(LogCInterceptor));
            serviceCollection.TryAddProxiedScopedInService<IBlogService, BlogService>();
            return serviceCollection.BuildServiceProvider();
        }
    }
}