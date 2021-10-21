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
                    
                    var client = new UdpClient();
                    client.Connect("127.0.0.1", 8881);
                    
                    var ready = Encoding.ASCII.GetBytes("READY");

                    //Connect to Server and signal ready
                    await client.SendAsync(ready, ready.Length);

                    //Prepare to send data to telemetry software
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
}
