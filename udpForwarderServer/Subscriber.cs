using System;
using System.Net;
using System.Net.Sockets;

namespace udpForwarder
{
    public class Subscriber
    {
        private IPEndPoint ep;

        public IPublisher Publisher { get; set; }

        private UdpClient subscriberSocket { get; set; }

        public Subscriber(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Setup(Socket newConnectionSocket, System.Net.IPEndPoint groupEP)
        {
            this.subscriberSocket = new UdpClient();
            this.subscriberSocket.Client = newConnectionSocket;
            this.ep = groupEP;

            this.Publisher.Handler += OnPublish;
        }

        private void OnPublish(object sender, Message msg)
        {
            try
            {
                Console.WriteLine($"Send to socket {this.subscriberSocket.ToString()}");
                // var args = new SocketAsyncEventArgs();
                // args.SetBuffer(msg.Content);

                var t = this.subscriberSocket.SendAsync(msg.Content, msg.ContentLength, ep);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
    }
}