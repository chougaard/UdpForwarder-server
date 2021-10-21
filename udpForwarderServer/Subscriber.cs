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
                var t = this.subscriberSocket.SendAsync(msg.Content, msg.ContentLength, ep);
            }
            catch (System.Exception ex)
            {
                subscriberSocket.Close();
                throw ex;
            }

        }
    }
}