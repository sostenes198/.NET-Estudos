using Estudos.CleanArchitecture.Modular.API;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Estudos.CleanArchitecture.Modular.IntegratedTests.Tests
{
    public class DependencyInjectionTest 
    {
        private WebApplicationFactory<Program>? _webApplicationFactory;

        [Fact(DisplayName = "Deve validar injeção de dependencias da aplicação")]
        public void ShouldValidateDependencyInjectionApplication()
        {
            // arrange
            const string JsonFile = BaseTest.JsonFile;

            // act 
            _webApplicationFactory = new WebApplicationFactory<Program>();

            _webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(BaseTest.Enviroment);
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
}