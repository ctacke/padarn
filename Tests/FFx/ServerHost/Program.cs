using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF.Web.Server;
using System.Net;
using System.Net.Sockets;

namespace ServerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new Host();
            host.Start();

            bool exit = false;

            while (!exit)
            {
                var command = Console.ReadLine();
                exit = (command == "exit");
            }

            host.Stop();
        }
    }

    class Host
    {
        private WebServer m_server;

        public Host()
        {
            m_server = new WebServer();
        }

        public void Start()
        {
            m_server.Start();

            var ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(
                a => a.AddressFamily == AddressFamily.InterNetwork
                && a.GetAddressBytes()[0] != 169);

            Console.WriteLine(string.Format("Listening at: {0}:{1}", ip, m_server.Configuration.Port));
        }

        public void Stop()
        {
            m_server.Stop();
        }
    }
}
