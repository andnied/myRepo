using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class TcpRequestClient
    {
        private static int HttpPort = 80;

        public string Host { get; set; }
        public string Resource { get; set; }
        public IPEndPoint EndPoint { get; set; }

        public TcpRequestClient(string url)
        {
            var uri = new Uri(url);
            Host = uri.Host;
            Resource = uri.PathAndQuery;
            
            var hostEntry = Dns.GetHostEntry(Host);
            EndPoint = new IPEndPoint(hostEntry.AddressList[0], HttpPort);
        }

        public string SendRequest()
        {
            using (var socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(EndPoint);
                SendRequest(socket);
                return GetResponse(socket);
            }
        }

        private void SendRequest(Socket socket)
        {
            var requestMessage = string.Format(
                "GET {0} HTTP/1.1\r\n" +
                "Host: {1}\r\n\r\n",
                Resource, Host
                );

            var requestBytes = Encoding.ASCII.GetBytes(requestMessage);
            socket.Send(requestBytes);
        }

        private string GetResponse(Socket socket)
        {
            var bytes = 0;
            var buffer = new byte[255];
            var builder = new StringBuilder();

            do
            {
                bytes = socket.Receive(buffer);
                builder.Append(Encoding.ASCII.GetString(buffer));
            } while (bytes == 255);

            return builder.ToString();
        }
    }
}
