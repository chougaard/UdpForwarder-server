using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace udpForwarder
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello ForwarderServer!");

            var pub = new Publisher();
            var forwarderServer = new ForwarderServer(pub);

            var t = new Thread(() =>
            {
                forwarderServer.StartServerAsync("127.0.0.1", 8881, pub);
            });
            t.Start();

            var t2 = new Thread(() =>
            {
                UDPSocket ACCRelayServer = new UDPSocket();
                ACCRelayServer.Server("127.0.0.1", 9996, pub);
            });
            t2.Start();

            Console.Read();
        }
    }
}
