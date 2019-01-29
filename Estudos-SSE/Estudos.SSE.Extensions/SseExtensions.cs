using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Estudos.SSE.Core.Authorizations.Contracts;
using Estudos.SSE.Core.BackgroundServices;
using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Middlewares;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services;
using Estudos.SSE.Core.Services.Contracts;
// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Extensions
{
    public static class SseExtensions
    {
        public static IServiceCollection AddSse(this IServiceCollection services, Action<SseBuilder>? buildSse = null, Action<SseOptions>? buildSseOptions = null)
        {
            var sseBuilder = new SseBuilder();
            buildSse?.Invoke(sseBuilder);

            ConfigureOptions(services, buildSseOptions);
            AddSseService(services);
            AddAuthorization(services, sseBuilder);
            AddClientIdProvider(services, sseBuilder);
            AddClientSseStorage(services, sseBuilder);
            AddCloseExpiresConnectionSse(services);

            return services;
        }

        public static IServiceCollection AddKeepAliveSseConnection(this IServiceCollection services)
        {
            services.TryAddSingleton<KeepAliveSseConnectionBackgroundService>();
            services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<KeepAliveSseConnectionBackgroundService>());
            
            return services;
        }

        private static void ConfigureOptions(IServiceCollection services, Action<SseOptions>? buildSseOptions = null)
        {
            var sseOptions = new SseOptions();
            buildSseOptions?.Invoke(sseOptions);

            services.Configure<CloseExpiresConnectionOptions>(
                opt => { opt.CloseConnectionsInSecondsInterval = sseOptions.CloseConnectionsInSecondsInterval.GetValueOrDefault(); });

            services.Configure<DistributedCacheClientSseStorageOptions>(
                opt => { opt.MaxTimeCacheInMinutes = sseOptions.MaxTimeCacheInMinutes.GetValueOrDefault(); });

            services.Configure<SseMiddlewareOptions>(
                opt => { opt.OnPrepareAccept = sseOptions.OnPrepareAccept; });

            services.Configure<SseServiceOptions>(
                opt =>
                {
                    opt.OnClientConnected = sseOptions.OnClientConnected;
                    opt.OnClientDisconnected = sseOptions.OnClientDisconnected;
                });
        }

        private static void AddSseService(IServiceCollection services)
        {
            services.TryAddSingleton<SseService>();
            services.TryAddSingleton<ISseService>(provider => provider.GetRequiredService<SseService>());
            services.TryAddSingleton<ISseServiceClientManager>(provider => provider.GetRequiredService<SseService>());
            services.TryAddSingleton<ISseServiceSendEvent>(provider => provider.GetRequiredService<SseService>());
        }

        private static void AddAuthorization(this IServiceCollection services, SseBuilder sseBuilder)
        {
            services.TryAddSingleton(typeof(IAuthorizationSse), sseBuilder.AuthorizationType);
        }

        private static void AddClientIdProvider(this IServiceCollection services, SseBuilder sseBuilder)
        {
            services.TryAddSingleton(typeof(IClientIdProvider), sseBuilder.ClientIdProviderType);
        }

        private static void AddClientSseStorage(this IServiceCollection services, SseBuilder sseBuilder)
        {
            services.TryAddSingleton(typeof(IClientSseStorage), sseBuilder.ClientSseStorageType);
        }

        private static void AddCloseExpiresConnectionSse(this IServiceCollection services)
        {
            services.TryAddSingleton<CloseExpiresConnectionSseBackgroundService>();
            services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<CloseExpiresConnectionSseBackgroundService>());
        }

        public static IApplicationBuilder UseSse(this IApplicationBuilder app, PathString path)
        {
            app.Map(path, builder => builder.UseMiddleware<SseMiddleware>());

            return app;
        }
    }
}