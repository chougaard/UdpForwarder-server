using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace udpForwarderClient
{
    public class ConfigModel
    {
        public string telemetryClientIP {get;set;} 
        public int telemetryClientPort {get;set;}
        public string serverIP {get;set;}
        public int serverPort{get;set;}
        public bool IsInitialized { get; set; } = false;
    }

    class Program
    {
        private static string configFilePath = "./config.json";

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                Console.WriteLine("Runnig...");
                Console.WriteLine("Press Any key to quit");

                var config = new ConfigModel();

                try
                {
                    //Get Config File
                    var content = await File.ReadAllTextAsync(configFilePath);

                    config = JsonSerializer.Deserialize<ConfigModel>(content);

                    config.IsInitialized = true;
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("No Configfile found");
                    Console.Read();

                    return;
                }

                try
                {
                    Console.WriteLine("Forwarder Client started!");

                    var client = new TcpClient(config.serverIP, config.serverPort);

                    //Prepare to send data to telemetry software
                    var telemetryClient = new UdpClient();
                    telemetryClient.Connect(config.telemetryClientIP, config.telemetryClientPort);

                    var stream = client.GetStream();

                    Console.WriteLine("Connected to server");

                    while (true)
                    {
                        var buffer = new byte[2024];

                        var numbytes = stream.Read(buffer, 0, buffer.Length);

                        var packet = buffer;
                        var sentbytes = await telemetryClient.SendAsync(packet, numbytes);
                    }

                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Something went very wrong somewhere!");
                    Console.Read();
                    throw e;
                }

            });


            Console.Read();
        }
    }
}
