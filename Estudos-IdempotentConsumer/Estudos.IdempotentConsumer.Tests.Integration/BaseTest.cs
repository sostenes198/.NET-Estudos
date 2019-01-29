using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.IdempotentConsumer.Tests.Integration;

public class BaseTest: IDisposable
{
    public string Enviroment { get; set; } = "IntegrationTest";

    public string JsonFile { get; set; } = "appsettings.test.json";

    public Type? Startup { get; set; } /*= typeof(Startup);*/

    public IDictionary<string, string> InMemoryCollectionConfiguration { get; set; } = new Dictionary<string, string>();

    public Action<WebHostBuilderContext, IServiceCollection> ConfigureServices { get; set; } = (_, _) => { };

    public Action<IApplicationBuilder> Configure { get; set; } = _ => { };

    private TestServer? _testServer;

    protected TestServer TestServer => _testServer ??= new TestServer(CreateWebHost())
    {
        PreserveExecutionContext = true
    };

    public IServiceProvider ServiceProvider => TestServer.Services;

    public virtual HttpClient Client
    {
        get
        {
            var client = TestServer.CreateClient();
            return client;
        }
    }

    private IWebHostBuilder CreateWebHost()
    {
        var builder = new WebHostBuilder();
        builder.UseEnvironment(Enviroment);
        builder.ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddJsonFile(JsonFile).AddInMemoryCollection(InMemoryCollectionConfiguration));
        builder.ConfigureServices(ConfigureServices);
        builder.Configure(Configure);
        if (Startup != null)
            builder.UseStartup(Startup);
        return builder;
    }

    public void Dispose()
    {
        DisposeBase();
        _testServer?.Dispose();
    }

    protected virtual void DisposeBase()
    {
    }
}