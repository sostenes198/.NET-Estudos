using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Scheduled.Message.Api;

namespace Scheduled.Message.Tests.Integration.Api;

public class DependencyInjectionTest : BaseTest
{
    private WebApplicationFactory<Program>? _webApplicationFactory;

    [Fact]
    public void Should_Validate_Dependency_Injection_Application()
    {
        // arrange - act 
        _webApplicationFactory = new WebApplicationFactory<Program>();

        _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment(Enviroment);
            builder.ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddJsonFile(JsonFile));

            builder.UseDefaultServiceProvider(opt =>
            {
                opt.ValidateOnBuild = true;
                opt.ValidateScopes = true;
            });
        });

        // assert
        var services = _webApplicationFactory.Services;

        services.Should().NotBeNull();

        DisposeFactory();
    }

    private void DisposeFactory()
    {
        try
        {
            _webApplicationFactory?.Dispose();
        }
        catch
        {
            // ignored
        }
    }
}