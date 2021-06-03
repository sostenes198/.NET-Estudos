using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Estudos.Graphana
{
    public class StatsDClient
    {
        private readonly string _host;

        public StatsDClient(string host)

        {
            _host = host;
        }

        public void Send(string dataGram)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                var serverAddress = Dns.GetHostEntry(_host);
                var endPoint = new IPEndPoint(serverAddress.AddressList[0], 8125);
                var bytes = Encoding.UTF8.GetBytes(dataGram);
                socket.SendTo(bytes, endPoint);
            }
        }
    }
}