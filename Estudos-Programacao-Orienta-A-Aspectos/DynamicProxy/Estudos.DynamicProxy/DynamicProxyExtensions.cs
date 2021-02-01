using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Estudos.DynamicProxy.Interceptors.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.DynamicProxy
{
    public static class DynamicProxyExtensions
    {
        public static void AddDynamicProxy(IServiceCollection services, params Type[] interceptors)
        {
            ValidarInterceptors<Interceptor>(interceptors);
            services.TryAddSingleton(new ProxyGenerator());
            foreach (var interceptor in interceptors)
                services.AddScoped(typeof(Interceptor), interceptor);
        }
        
        public static void AddGenericDynamicProxy(IServiceCollection services, params Type[] interceptors)
        {
            ValidarInterceptors<GenericInterceptor>(interceptors);
            services.TryAddSingleton(new ProxyGenerator());
            foreach (var interceptor in interceptors)
                services.AddScoped(typeof(GenericInterceptor), interceptor);
        }

        public static void TryAddProxiedScopedInService<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.TryAddScoped<TImplementation>();
            services.TryAddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService<TImplementation>();
                var interceptorsToApply = ListInterceptorsToApply<TInterface, TImplementation>();
                IInterceptor[] interceptors = serviceProvider.GetServices<GenericInterceptor>().Where(lnq => interceptorsToApply.Any(t => t.Name == lnq.GetType().Name)).ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
            });
        }

        public static void TryAddProxiedScopedInService<TInterface, TImplementation>(this IServiceCollection services, Type[] interceptorsToApply)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            ValidarInterceptors<Interceptor>(interceptorsToApply);

            services.TryAddScoped<TImplementation>();
            services.TryAddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService<TImplementation>();
                IInterceptor[] interceptors = serviceProvider.GetServices<Interceptor>().Where(lnq => interceptorsToApply.Any(t => t == lnq.GetType())).ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
            });
        }

        private static void ValidarInterceptors<TInterceptor>(Type[] interceptors)
        {
            if (interceptors.Any(lnq => typeof(TInterceptor).IsAssignableFrom(lnq) == false))
                throw new Exception("Tipo passado não é um interceptador");
        }

        private static Type[] ListInterceptorsToApply<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            var interceptorsInterface = ListTypesInterceptors(typeof(TInterface));
            var interceptorsImplementation = ListTypesInterceptors(typeof(TImplementation));
            return interceptorsInterface.Concat(interceptorsImplementation).ToArray();
        }

        private static IEnumerable<Type> ListTypesInterceptors(Type type) =>
            type.GetMethods().Select(lnq => lnq.GetCustomAttributes(typeof(InterceptorAttribute))).SelectMany(lnq => lnq).Cast<InterceptorAttribute>().Select(lnq => lnq.Interceptor);
    }
}