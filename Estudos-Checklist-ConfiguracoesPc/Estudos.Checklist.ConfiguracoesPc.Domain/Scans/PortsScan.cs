using System.Net.Sockets;
using System.Threading.Tasks;
using Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces;
using Estudos.Checklist.ConfiguracoesPc.Domain.ScansResult.Port;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.Scans
{
    public class PortsScan : IScan
    {
        private readonly string _host;
        private readonly int[] _ports;
        private readonly PortsScanResult _resultPortScan;

        public PortsScan(string host, params int[] ports)
        {
            _host = host;
            _ports = ports;
            _resultPortScan = new PortsScanResult();
        }


        public Task<IScanResult> ScanAsync()
        {
            return ScanTcpPorts();
        }

        private async Task<IScanResult> ScanTcpPorts()
        {
            var protocolType = ProtocolType.Tcp;
            foreach (var port in _ports)
            {
                var portIsOpen = await IsPortOpen(port, protocolType);
                _resultPortScan.AddPortScanResult(port, portIsOpen, protocolType);
            }

            return _resultPortScan;
        }

        private async Task<bool> IsPortOpen(int port, ProtocolType protocolType)
        {
            Socket socket = null;
            bool result = false;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
                await socket.ConnectAsync(_host, port);
                result = true;
            }
            catch (SocketException exception)
            {
                if (exception.SocketErrorCode == SocketError.ConnectionRefused)
                    result = false;
            }

            CloseConnectionSocket(socket);
            
            return result;
        }

        private void CloseConnectionSocket(Socket socket)
        {
            if (socket?.Connected ?? false)
                socket.Disconnect(false);
            
            socket?.Close();
        }
    }
}