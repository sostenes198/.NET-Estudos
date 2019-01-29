using System.Net.Sockets;
using System.Threading.Tasks;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.Scans;
using Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Port;
using FluentAssertions;
using Xunit;

namespace Estudos.Checklist.ConfiguracoesPc.Tests.Scans
{
    public class PortsScanTests
    {
        private readonly IScan _scan;

        public PortsScanTests()
        {
            _scan = new PortsScan("127.0.0.1", new[] {80, 8080, 5939});
        }
        
        
        [Fact]
        public async Task Should_Validate_Ports_Open_And_Close()
        {
            var expectedResult = new PortsScanResult();
            expectedResult.AddPortScanResult(80, true, ProtocolType.Tcp);
            expectedResult.AddPortScanResult(8080, false, ProtocolType.Tcp);
            expectedResult.AddPortScanResult(5939, true, ProtocolType.Tcp);
            
            var result = (PortsScanResult) await _scan.ScanAsync();

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}