using System;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.Resources;
using Estudos.Checklist.ConfiguracoesPc.Domain.Scans;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Estudos.Checklist.ConfiguracoesPc.Domain
{
    public static class Bootstrap
    {
        public static void RegisterApplication(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureOptions(services, configuration);

            services
                .AddTransient<IScan, PortsScan>(provider =>
                {
                    var config = GetConfiguration(provider);
                    return new PortsScan(config.Host, config.Ports);
                })
                .AddTransient<IScan, RegistryScan>(provider => new RegistryScan(GetConfiguration(provider).RegistriesWindows))
                .AddTransient<IScan, DirectoryScan>(provider => new DirectoryScan(GetConfiguration(provider).Directories));
        }

        private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Configuration>(configuration.GetSection(nameof(Configuration)));
        }

        private static Configuration GetConfiguration(IServiceProvider provider) => provider.GetService<IOptions<Configuration>>().Value;
    }
}