using System.Threading.Tasks;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.Scans;
using Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Directory;
using Estudos.Checklist.ConfiguracoesPc.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Estudos.Checklist.ConfiguracoesPc.Tests.Scans
{
    public class DirectoryScanTests : IClassFixture<BaseFixture>
    {
        private readonly IScan _scan;

        public DirectoryScanTests(BaseFixture fixture)
        {
            _scan = (IScan) fixture.ServiceProvider.GetService(typeof(DirectoryScan));
        }

        [Fact]
        public async Task Should_Scan_Directory()
        {
            var resultExpected = new DirectoriesScanResult();
            resultExpected.AddRegistryScanResult(@"D:\Teste", true, true, true);
            resultExpected.AddRegistryScanResult(@"D:\Teste\A", true, false, true);
            resultExpected.AddRegistryScanResult(@"D:\Teste\B", true, true, false);
            resultExpected.AddRegistryScanResult(@"D:\NotFound", false, false, false);

            var result = (DirectoriesScanResult) await _scan.ScanAsync();

            result.Should().BeEquivalentTo(resultExpected);
        }
    }
}