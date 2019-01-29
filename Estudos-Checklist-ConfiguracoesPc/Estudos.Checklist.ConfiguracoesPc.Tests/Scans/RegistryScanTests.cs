using System.Threading.Tasks;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.Scans;
using Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Registry;
using FluentAssertions;
using Xunit;

namespace Estudos.Checklist.ConfiguracoesPc.Tests.Scans
{
    public class RegistryScanTests
    {
        private readonly IScan _registryScan;

        public RegistryScanTests()
        {
            _registryScan = new RegistryScan(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", @"NotExist");
        }

        [Fact]
        public async Task Should_Scan_Registry()
        {
            var resultExpected = new RegistriesScanResult();
            resultExpected.AddRegistryScanResult(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true, true, true);
            resultExpected.AddRegistryScanResult(@"NotExist", false, false, false);

            var result = await _registryScan.ScanAsync();

            result.Should().BeEquivalentTo(resultExpected);
        }
    }
}