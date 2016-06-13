using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                Console.WriteLine("You must provide 1 parameter");

            var client = new TcpRequestClient(args[0]);
            var response = client.SendRequest();

            Console.WriteLine(response);
            Console.Read();
        }
    }
}
