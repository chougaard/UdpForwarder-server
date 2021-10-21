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
            Task.Run(async () =>
            {
                Console.WriteLine("Hello World!");

                var pub = new Publisher();
                // var sub1 = new Subscriber(pub);

                // sub1.Publisher.Handler += Execute;

                var forwarderServer = new ForwarderServer(pub);

                var t = new Thread(async () =>
                {
                    await forwarderServer.StartServerAsync("127.0.0.1", 8881, pub);
                });
                t.Start();

                var t2 = new Thread(() =>
                {
                    UDPSocket ACCRelayServer = new UDPSocket();
                    ACCRelayServer.Server("127.0.0.1", 9996, pub);
                });
                t2.Start();


Console.Read();
            });

            Console.Read();
        }

        private static void Execute(object sender, Message msg)
        {
            string decoded = Encoding.ASCII.GetString(msg.Content, 0, msg.ContentLength);

            Console.WriteLine($"PUBLISH: {decoded}");
        }
    }
}
