using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Middlewares
{
    [ExcludeFromCodeCoverage]
    public static class ReactiveManagementAppConfigurationReconnectionMiddlewareExtension
    {
        public static IApplicationBuilder UseReactiveManagementAppConfigurationReconnection(this IApplicationBuilder app)
        {
            app.UseMiddleware<ReactiveManagementAppConfigurationReconnectionMiddleware>();
            return app;
        }
    }
}