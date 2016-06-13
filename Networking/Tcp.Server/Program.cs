using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tcp.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                    throw new ArgumentException("Two arguments should be provided. IP and port.");

                IPAddress ip = null;
                short port = 0;

                if (!(IPAddress.TryParse(args[0], out ip)))
                    throw new ArgumentException("Invalid IP Address.");
                if (!(short.TryParse(args[1], out port)))
                    throw new ArgumentException("Invalid Port number.");

                var server = new TcpListener(ip, port);
                server.Start();
                server.BeginAcceptTcpClient(Receive, server);
                Console.WriteLine(string.Format("Server is listening on IP: {0}, port: {1}", ip, port));
                Console.ReadKey();
                
                server.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static void Receive(IAsyncResult ar)
        {
            var server = (TcpListener)ar.AsyncState;
            var client = server.EndAcceptTcpClient(ar);
            Console.WriteLine("Client connected.");
            client.Close();
        }
    }
}
