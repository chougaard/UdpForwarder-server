using System;
using System.Net;
using System.Net.Sockets;

namespace udpForwarder
{
    public class Subscriber
    {
        // private IPEndPoint ep;
        private TcpClient _subscriberClient;
        private NetworkStream _stream;

        public IPublisher Publisher { get; set; }

        // private UdpClient subscriberSocket { get; set; }

        public Subscriber(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void Setup(TcpClient client)
        {
            this._subscriberClient = client;
            this.Publisher.Handler += OnPublish;
        }

        // public void Setup(Socket newConnectionSocket, System.Net.IPEndPoint groupEP)
        // {
        //     this.subscriberSocket = new UdpClient();
        //     this.subscriberSocket.Client = newConnectionSocket;
        //     this.ep = groupEP;

        //     this.Publisher.Handler += OnPublish;
        // }

        private void OnPublish(object sender, Message msg)
        {
            try
            {
                // var t = this.subscriberSocket.SendAsync(msg.Content, msg.ContentLength, ep);
                this._stream = this._subscriberClient.GetStream();

                _stream.Write(msg.Content, 0, msg.ContentLength);
            }
            catch (System.Exception ex)
            {
                // subscriberSocket.Close();
                this._stream.Close();
                this._subscriberClient.Close();
                throw ex;
            }

        }
    }
}