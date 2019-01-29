using System.Net.Sockets;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Port
{
    public class PortScanResult
    {
        public int PortNumber { get; }
        public bool IsPortOpen { get; }
        public ProtocolType TypePort { get; }

        public PortScanResult(int portNumber, bool isPortOpen, ProtocolType typePort)
        {
            PortNumber = portNumber;
            IsPortOpen = isPortOpen;
            TypePort = typePort;
        }
    }
}