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
        private UdpClient _listener;

        public void StartServerAsync(string address, int port, Publisher pub)
        {
            try
            {
                this._listener = new UdpClient(port);
                
                while (true)
                {
                    var groupEP = new IPEndPoint(IPAddress.Any, port);
                    var data = this._listener.Receive(ref groupEP);

                    var sub = new Subscriber(pub);
                    sub.Setup(this._listener.Client, groupEP);

                    subscribers.Add(sub);
                }
            }
            catch (System.Exception ex)
            {
                this._listener.Close();
                throw ex;
            }
        }
    }
}