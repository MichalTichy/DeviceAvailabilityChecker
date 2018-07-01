using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAC_Common;

namespace DAC_Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                PerformRemoteAvailabilityCheck().RunSynchronously();
                Thread.Sleep(TimeSpan.FromMinutes(Configuration.TimeBetweenPings));
            }
        }

        private static async Task PerformRemoteAvailabilityCheck()
        {
            Log("Checking Device status");
            var connector = new ApiConnector(Configuration.ApiUrl);
            var devices = await connector.GetAll();
            foreach (var device in devices)
            {

                Log($"Checking {device.Address}");
                var ping = new Ping();
                
                PingOptions options = new PingOptions
                {
                    DontFragment = true
                };

                byte[] buffer = Encoding.ASCII.GetBytes(Configuration.Data);

                var result = ping.Send(device.Address, Configuration.Timeout,buffer,options);
                
                if (result.Status==IPStatus.Success)
                {
                    Log("OK");
                    connector.ReportStatus(device).Wait();
                }
                else
                {
                    Log("Unreachable");
                }

            }
        }

        private static void Log(string text)
        {
            Console.WriteLine($"{DateTime.Now.ToString("g")} || {text}");
        }
    }
}
