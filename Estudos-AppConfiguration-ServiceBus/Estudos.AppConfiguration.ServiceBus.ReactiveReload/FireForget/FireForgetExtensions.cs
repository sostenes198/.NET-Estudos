using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.FireForget
{
    [ExcludeFromCodeCoverage]
    public static class FireForgetExtensions
    {
        public static IServiceCollection AddFireForgetHandler(this IServiceCollection services)
        {
            services.TryAddSingleton<IFireForgetHandler, FireForgetHandler>();
            return services;
        }
    }
}