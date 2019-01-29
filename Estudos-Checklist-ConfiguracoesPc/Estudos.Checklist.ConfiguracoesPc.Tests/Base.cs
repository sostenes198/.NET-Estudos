using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.Checklist.ConfiguracoesPc.Tests
{
    public class Base
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        
        public Action<IServiceCollection> AddServices { get; set; }
        public Func<string> ConfigureAppSetingsFile { get; set; }
        
        public IServiceProvider ServiceProvider => _serviceProvider ??= CreateDiContainer();
        public IConfiguration Configuration => _configuration ??= CreateConfiguration();
        
        public Base()
        {
            AddServices = (services) => { };
            ConfigureAppSetingsFile = () => "appsettings.tests.json";
        }
        
        private IServiceProvider CreateDiContainer()
        {
            IServiceCollection services = new ServiceCollection();
            
            ConfigureServices(services);

            return services.BuildServiceProvider();
        }
        
        private IConfiguration CreateConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigureAppSetingsFile())
                .Build();
        
        private void ConfigureServices(IServiceCollection services)
        {
            AddServices(services);
        }
    }
}