using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace DAC_Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                PerformRemoteAvailabilityCheck();
                Thread.Sleep(TimeSpan.FromMinutes(Configuration.TimeBetweenPings));
            }
        }

        private static void PerformRemoteAvailabilityCheck()
        {
            Log("Checking status of devices");
            var connector = new ApiConnector(Configuration.ApiUrl);
            var devices = connector.GetAll().Result;

            Log($"Found {devices.Count} devices");

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
