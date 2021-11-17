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
        private static string udpServerIP;
        private static int udpServerPort;

        static void Main(string[] args)
        {
            telemetryClientIP = "127.0.0.1";
            telemetryClientPort = 9995;

            udpServerIP = "127.0.0.1";
            udpServerPort = 8881;

            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Forwarder Client started!");
                    
                    var client = new TcpClient(udpServerIP, udpServerPort);
                    // client.Connect(udpServerIP, udpServerPort);
                    // await client.ConnectAsync();
                    
                    // var ready = Encoding.ASCII.GetBytes("READY");

                    // //Connect to Server and signal ready
                    // await client.SendAsync(ready, ready.Length);

                    //Prepare to send data to telemetry software
                    var telemetryClient = new UdpClient();
                    telemetryClient.Connect(telemetryClientIP, telemetryClientPort);

                    var stream = client.GetStream();

                    while (true)
                    {
                        

                        // var packet = await client.ReceiveAsync();

                        var buffer = new byte[2024];

                        stream.Read(buffer, 0, buffer.Length);

                        var packet = buffer;

                        await telemetryClient.SendAsync(packet, packet.Length);

                        Console.WriteLine($"Received Packet, sized {packet.Length}");
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
