using Estudos.Checklist.ConfiguracoesPc.Domain.Resources;
using Estudos.Checklist.ConfiguracoesPc.Tests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Estudos.Checklist.ConfiguracoesPc.Tests
{
    public class ConfigurationTests : IClassFixture<BaseFixture>
    {
        private readonly BaseFixture _fixture;

        public ConfigurationTests(BaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Should_Get_Configuration()
        {
            var resultExpected = new Configuration
            {
                Host = "127.0.0.1",
                Ports = new[] {80, 8080, 5939},
                RegistriesWindows = new[]
                {
                    "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                    "NotExist"
                },
                Directories = new[]
                {
                    "D:\\Teste",
                    "D:\\Teste\\A",
                    "D:\\Teste\\B",
                    "D:\\NotFound"
                }
            };

            var result = _fixture.ServiceProvider.GetService<IOptions<Configuration>>().Value;
            
            result.Should().BeEquivalentTo(resultExpected);
        }
    }
}