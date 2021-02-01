using System.IO;
using Estudos.RemoteConfigurationProvider.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Estudos.RemoteConfigurationProvider.Tests
{
    public abstract class ConfigurationTest
    {
        private readonly IRemoteFileInfo _remoteFileInfo;
        private readonly IRemoteFileInfo _remoteFileInfoNotExistAndOptional;
        private readonly IRemoteFileInfo _remoteFileInfoNotExitAndNotOptional;
        private readonly string _resultRemotAppsettings;

        protected ConfigurationTest(
            IRemoteFileInfo remoteFileInfo, 
            IRemoteFileInfo remoteFileInfoNotExistAndOptional, 
            IRemoteFileInfo remoteFileInfoNotExitAndNotOptional, 
            string resultRemotAppsettings)
        {
            _remoteFileInfo = remoteFileInfo;
            _remoteFileInfoNotExistAndOptional = remoteFileInfoNotExistAndOptional;
            _remoteFileInfoNotExitAndNotOptional = remoteFileInfoNotExitAndNotOptional;
            _resultRemotAppsettings = resultRemotAppsettings;
        }
        
        [Fact]
        public void Should_Validate_Remote_File_Configuration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddRemoteJsonFile(_remoteFileInfo)
                .Build();
            
                configuration.GetSection("RESULT:REMOTE_APPSETTINGS").Get<string>().Should().Be(_resultRemotAppsettings);
            configuration.GetSection("RESULT:COMPLEX:ITEM1").Get<int>().Should().Be(1);
            configuration.GetSection("RESULT:COMPLEX:ITEM2").Get<string>().Should().Be("ITEM2");
            configuration.GetSection("RESULT:COMPLEX:ITEM3").Get<int[]>().Should().BeEquivalentTo(new[] {1, 2, 3});
            configuration.GetSection("RESULT:COMPLEX:ITEM4").Get<string[]>().Should().BeEquivalentTo("a", "b", "c");
            configuration.GetSection("RESULT:NUMBER").Get<int>().Should().Be(123546);
            configuration.GetSection("RESULT:BOOL").Get<bool>().Should().Be(true);
        }

        [Fact]
        public void Should_Be_Validate_Remote_File_When_Not_Exist_And_Is_Optional()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddRemoteJsonFile(_remoteFileInfoNotExistAndOptional)
                .Build();

            configuration.GetSection("RESULT:REMOTE_APPSETTINGS").Value.Should().BeNullOrEmpty();
            configuration.GetSection("RESULT:COMPLEX:ITEM1").Value.Should().BeNullOrEmpty();
            configuration.GetSection("RESULT:COMPLEX:ITEM2").Value.Should().BeNullOrEmpty();
            configuration.GetSection("RESULT:COMPLEX:ITEM3").Value.Should().BeNullOrEmpty();
            configuration.GetSection("RESULT:COMPLEX:ITEM4").Value.Should().BeNullOrEmpty();
            configuration.GetSection("RESULT:NUMBER").Value.Should().BeNullOrEmpty();
            configuration.GetSection("RESULT:BOOL").Value.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Should_Thrown_FileNotFoundException_When_File_Not_Found_And_Is_Not_Optional()
        {
            new ConfigurationBuilder()
                .Invoking(lnq =>
                    lnq.AddRemoteJsonFile(_remoteFileInfoNotExitAndNotOptional)
                        .Build())
                .Should()
                .Throw<FileNotFoundException>()
                .And
                .Message
                .Should().Be($"The configuration from remote file '{_remoteFileInfoNotExitAndNotOptional.Path}' was not found and is not optional.");
        }
    }
}