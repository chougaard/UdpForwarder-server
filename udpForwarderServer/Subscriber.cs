using System;
using System.Net.Sockets;

namespace udpForwarder
{
    public class Subscriber
    {
        public IPublisher Publisher { get; set; }

        private UdpClient subscriberSocket { get; set; }

        public Subscriber(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Setup(UdpClient newConnectionSocket)
        {
            this.subscriberSocket = newConnectionSocket;

            this.Publisher.Handler += OnPublish;
        }

        private void OnPublish(object sender, Message msg)
        {
            try
            {
                Console.WriteLine($"Send to socket {this.subscriberSocket.ToString()}");
                // var args = new SocketAsyncEventArgs();
                // args.SetBuffer(msg.Content);

                var t = this.subscriberSocket.SendAsync(msg.Content, msg.ContentLength);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
    }
}