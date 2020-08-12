using System;
using System.Reflection;
using Estudos.RemoteConfigurationProvider.Configurations.RemoteFile;
using Estudos.RemoteConfigurationProvider.Contracts;
using Estudos.RemoteConfigurationProvider.Tests.Fixture;
using FluentAssertions;
using Xunit;

namespace Estudos.RemoteConfigurationProvider.Tests.Tests.Configuration.RemoteFile
{
    public class RemoteFileInfoTest
    {
        [Theory]
        [InlineData(default)]
        [InlineData("")]
        [InlineData("                     ")]
        public void Should_Thrown_When_Path_Invalid(string path)
        {
            Action act = () => new RemoteFileInfoDefault(path);
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().Be(nameof(IRemoteFileInfo.Path));
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Thrown_When_ReloadTimeMinutes_Invalid(int reloadTimeMinutes)
        {
            Action act = () => new RemoteFileInfoDefault("Path", reloadTimeMinutes: reloadTimeMinutes);
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().Be(nameof(IRemoteFileInfo.ReloadTimeMinutes));
        }

        [Fact]
        public void Should_Validate_Watch()
        {
            var remoteFileInfo = new RemoteFileInfoDefault("Path");
            typeof(RemoteFileInfo)
                .GetField("IntervalInMiliseconds", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(remoteFileInfo, 2000);
            remoteFileInfo.Watch(AsyncProcessWaitHandle.SetCompleted);
            AsyncProcessWaitHandle.Wait(new TimeSpan(0,1,0));
        }
        
    }
}