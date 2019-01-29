using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
#pragma warning disable CS8618

namespace Estudos.SSE.Tests.Integration;

public class BaseTest : IDisposable
{
    public string Enviroment { get; set; } = "IntegrationTest";

    public string JsonFile { get; set; } = "appsettings-integration-test.json";

    public IDictionary<string, string> InMemoryCollectionConfiguration = new Dictionary<string, string>();

    public Action<WebHostBuilderContext, IServiceCollection> ConfigureServices { get; set; } = (_, _) => { };
    
    public Action<IApplicationBuilder> Configure { get; set; } = _ => { };

    private WebApplicationFactory<Startup>? _factory;

    private WebApplicationFactory<Startup> Factory => _factory ??= CreateWebApplicationFactory();

    public IServiceProvider ServiceProvider => Factory.Services;

    public IConfiguration Configuration => ServiceProvider.GetRequiredService<IConfiguration>();

    public virtual HttpClient Client
    {
        get
        {
            var client = Factory.CreateClient();

            return client;
        }
    }

    private WebApplicationFactory<Startup> CreateWebApplicationFactory()
    {
        var application = new WebApplicationFactory<Startup>()
           .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(Enviroment);
                builder.ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddJsonFile(JsonFile).AddInMemoryCollection(InMemoryCollectionConfiguration));
                builder.ConfigureServices(ConfigureServices);
                builder.ConfigureServices(ConfigureDefaultService);

                builder.UseTestServer(t => { t.PreserveExecutionContext = true; });

                builder.Configure(Configure);
            });

        return application;
    }

    protected virtual void ConfigureDefaultService(WebHostBuilderContext webHostBuilderContext, IServiceCollection services)
    {
    }

    public void Dispose()
    {
        DisposeBase();
        _factory?.Dispose();
    }

    protected virtual void DisposeBase()
    {
    }

    protected static Task DefaultDelayToWaitFireForget(int delayInMilliseconds = 1000) => Task.Delay(TimeSpan.FromMilliseconds(delayInMilliseconds));
}