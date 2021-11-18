using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace udpForwarder
{
    public class ConfigModel
    {
        public string telemetryRelayIP { get; set; }
        public int telemetryRelayPort { get; set; }
        public string serverIP { get; set; }
        public int serverPort { get; set; }
        public bool IsInitialized { get; set; } = false;
    }

    public class Program
    {
        private static string configFilePath = "./config.json";

        static void Main(string[] args)
        {
            Console.WriteLine("Server Running...");
            Console.WriteLine("Press Any key to quit");

            var config = new ConfigModel();

            try
            {
                //Get Config File
                var content = File.ReadAllText(configFilePath);

                config = JsonSerializer.Deserialize<ConfigModel>(content);

                config.IsInitialized = true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("No Configfile found");

                Console.Read();
            }

            var pub = new Publisher();
            var forwarderServer = new ForwarderServer(pub);

            try
            {
                var t = new Thread(() =>
                {
                    forwarderServer.StartServerAsync(config.serverIP, config.serverPort, pub);
                });
                t.Start();

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Server failed");
                Console.Read();
            }

            try
            {
                var t2 = new Thread(() =>
                {

                    UDPSocket ACCRelayServer = new UDPSocket();
                    ACCRelayServer.Server(config.telemetryRelayIP, config.telemetryRelayPort, pub);


                });
                t2.Start();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Relay connection failed");
                Console.Read();
            }

            Console.Read();
        }
    }
}
