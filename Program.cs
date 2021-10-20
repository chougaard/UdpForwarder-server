using System;
using System.Text;
using System.Threading;

namespace udpForwarder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var pub = new Publisher();
            var sub1 = new Subscriber(pub);

            sub1.Publisher.Handler += Execute;
   
            UDPSocket ACCRelayServer = new UDPSocket();
            ACCRelayServer.Server("127.0.0.1", 9996, pub);

            Console.Read();

         
            Console.Read();
        }

        private static void Execute(object sender, Message msg)
        {
            string decoded = Encoding.ASCII.GetString(msg.Content, 0, msg.ContentLength);

            Console.WriteLine($"PUBLISH: {decoded}");
        }
    }
}
