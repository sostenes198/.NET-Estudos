using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Port
{
    public class PortsScanResult : IEnumerable<PortScanResult>, IScanResult
    {
        private readonly ICollection<PortScanResult> _resultTcpPorts;
        
        public PortsScanResult()
        {
            _resultTcpPorts = new HashSet<PortScanResult>();
        }

        public void AddPortScanResult(int portNumber, bool isPortOpen, ProtocolType typePort) => _resultTcpPorts.Add(new PortScanResult(portNumber, isPortOpen, typePort));
        
        public IEnumerator<PortScanResult> GetEnumerator()
        {
            return _resultTcpPorts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}