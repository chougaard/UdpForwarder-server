using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace udpForwarderClient
{
    class Program
    {

        private static string telemetryClientIP;
        private static int telemetryClientPort;
        private static string serverIP;
        private static int serverPort;

        static void Main(string[] args)
        {
            telemetryClientIP = "127.0.0.1";
            telemetryClientPort = 9995;

            serverIP = "192.168.1.95";
            serverPort = 8881;

            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Forwarder Client started!");
                    
                    var client = new TcpClient(serverIP, serverPort);

                    //Prepare to send data to telemetry software
                    var telemetryClient = new UdpClient();
                    telemetryClient.Connect(telemetryClientIP, telemetryClientPort);

                    var stream = client.GetStream();

                    while (true)
                    {
                        var buffer = new byte[2024];

                        var numbytes = stream.Read(buffer, 0, buffer.Length);

                        var packet = buffer;
                        var sentbytes = await telemetryClient.SendAsync(packet, numbytes);

                        Console.WriteLine($"Send Packet, sized {numbytes}");
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
}
