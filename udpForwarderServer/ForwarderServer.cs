using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udpForwarder
{
    public class ForwarderServer
    {
        private Publisher _pub;

        public ForwarderServer(Publisher pub)
        {
            this._pub = pub;
        }

        private List<Subscriber> subscribers = new List<Subscriber>();
        private TcpListener _listener;

        private IPAddress _serverIp;

        public void StartServerAsync(string address, int port, Publisher pub)
        {
            this._serverIp = IPAddress.Parse(address);
            try
            {
                this._listener = new TcpListener(_serverIp, port);

                this._listener.Start();
                
                while (true)
                {
                    //var groupEP = new IPEndPoint(IPAddress.Any, port);
                    Console.WriteLine("Await connection");
                    var client = this._listener.AcceptTcpClient();
                    Console.WriteLine("Client Connected");

                    var sub = new Subscriber(pub);
                    sub.Setup(client);

                    subscribers.Add(sub);
                }
            }
            catch (System.Exception ex)
            {
                this._listener.Stop();
                throw ex;
            }
        }
    }
}