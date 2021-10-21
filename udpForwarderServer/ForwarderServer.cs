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
        private Socket _socket;
        private Publisher _pub;

        private UdpClient _client;

        public ForwarderServer(Publisher pub)
        {

            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this._pub = pub;
        }

        private List<Subscriber> subscribers = new List<Subscriber>();

        public async Task StartServerAsync(string address, int port, Publisher pub)
        {
            try
            {

                var ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);


                // _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
                // _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));

                var listener = new UdpClient(port);
                
                
                // Console.WriteLine(_client.Client.LocalEndPoint.ToString());

                while (true)
                {
                    // _socket.Listen(100);
                    // var newConnectionSocket = await _socket.AcceptAsync();
                    var groupEP = new IPEndPoint(IPAddress.Any, port);
                    var data = listener.Receive(ref groupEP);

                    // var sender = new IPEndPoint(IPAddress.Any, 0);
                    // var data = client.Receive(ref sender);

                    var sub = new Subscriber(pub);
                    sub.Setup(listener.Client, groupEP);

                    subscribers.Add(sub);
                }
            }
            catch (System.Exception ex)
            {

                _socket.Close();

                throw ex;
            }
        }
    }
}