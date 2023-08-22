using Estudos.CleanArchitecture.Modular.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
#pragma warning disable CS0618 // Type or member is obsolete

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace Estudos.CleanArchitecture.Modular.IntegratedTests
{
    public class BaseTest : IDisposable
    {
        public const string Enviroment = "IntegrationTest";

        public const string JsonFile = "appsettings.json";

        public IDictionary<string, string> InMemoryCollectionConfiguration = new Dictionary<string, string>();

        public Action<WebHostBuilderContext, IServiceCollection> ConfigureServices { get; set; } = (_, _) => { };

        private WebApplicationFactory<Program>? _factory;

        private WebApplicationFactory<Program> Factory => _factory ??= CreateWebApplicationFactory();

        public IServiceProvider ServiceProvider => Factory.Services;

        public IConfiguration Configuration => ServiceProvider.GetRequiredService<IConfiguration>();

        protected BaseTest()
        {
            //ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-br");    
        }

        public virtual HttpClient Client
        {
            get
            {
                var client = Factory.CreateClient();

                return client;
            }
        }

        private WebApplicationFactory<Program> CreateWebApplicationFactory()
        {
            var application = new WebApplicationFactory<Program>()
               .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment(Enviroment);
                    builder.ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddJsonFile(JsonFile).AddInMemoryCollection(OverrideConfigsAppSettingsJsonToTest()));
                    builder.ConfigureServices(ConfigureServices);
                    builder.UseTestServer(t => { t.PreserveExecutionContext = true; });
                    builder.UseSerilog((hostingContext, loggerConfiguration) => { loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration); });
                 });

            return application;
        }

        public void Dispose()
        {
            DisposeBase();
            _factory?.Dispose();
        }

        protected virtual void DisposeBase()
        {
        }

        private IDictionary<string, string> OverrideConfigsAppSettingsJsonToTest()
        {
            return InMemoryCollectionConfiguration;
        }
    }
}