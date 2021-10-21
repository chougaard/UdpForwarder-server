using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udpForwarderClient
{
    class Program
    {


        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                try
                {


                    Console.WriteLine("Forwarder Client started!");

                    var ip = "127.0.0.1";

                    // var server = new UDPSocket();

                    // await server.Connect("127.0.0.1", 8881);

                    var client = new UdpClient();

                    client.Connect("127.0.0.1", 8881);

                    string welcome = "Welcome to my test server";
                    var data = Encoding.ASCII.GetBytes(welcome);

                    await client.SendAsync(data, data.Length);

                    var telemetryClient = new UdpClient();

                    telemetryClient.Connect("127.0.0.1", 9995);
                    while (true)
                    {
                        var packet = await client.ReceiveAsync();


                        await telemetryClient.SendAsync(packet.Buffer, packet.Buffer.Length);

                        Console.WriteLine($"Received Packet, sized {packet.Buffer.Length}");
                    }

                }
                catch (System.Exception e)
                {

                    throw e;
                }

            });

            Console.Read();
        }
    }


    public class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        private const int bufSize = 8 * 1024;

        public UDPSocket()
        {

        }

        public async Task Connect(string ip, int port)
        {
            await _socket.ConnectAsync(IPAddress.Parse(ip), port);

            var e = new SocketAsyncEventArgs();
            var buffer = new ArraySegment<byte>();

            var b = new byte[bufSize];

            await _socket.ReceiveAsync(b, SocketFlags.None);

            Console.WriteLine($"Received from socket size: {b.Length}");
        }
    }
}
