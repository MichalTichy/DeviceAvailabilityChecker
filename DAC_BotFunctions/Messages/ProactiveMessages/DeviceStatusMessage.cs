using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAC_Common;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public class DeviceStatusMessage : MessageBase
    {
        public IReadOnlyCollection<DeviceReport> Devices { get; }
        public DeviceStatusMessage(IEnumerable<DeviceReport> devices)
        {
            Devices = devices.ToList().AsReadOnly();
        }

        public override async Task<string> Build()
        {

            if (!Devices.Any())
                return null;

            var sb = new StringBuilder();
            foreach (var deviceGroup in Devices.GroupBy(t=>t.Group))
            {
                sb.AppendLine(deviceGroup.Key);

                if (deviceGroup.Any(t => t.IsUnavailable))
                {
                    var unavailableDevices = deviceGroup.Where(t => t.IsUnavailable).ToArray();
                    sb.AppendLine(
                        $"In group {deviceGroup.Key} was detected that {unavailableDevices.Count()} {unavailableDevices.GetCorrectNoun()} {unavailableDevices.GetCorrectVerb()} unavailable.");
                }
                else
                {

                    sb.AppendLine(
                        $"In group {deviceGroup.Key} all devices are available.");
                }


                sb.AppendLine(String.Empty);

                sb.Append(GetDevicesReport(deviceGroup));


                sb.AppendLine(String.Empty);
            }
            

            return sb.ToString();
        }

        protected static string GetDevicesReport(IEnumerable<DeviceReport> devices)
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendLine(
                "ADDRESS | NAME | GROUP | LAST SEEN");

            foreach (var offlineDevice in devices)
            {
                var paddedName = offlineDevice.Name;
                var paddedAddress = offlineDevice.Address;
                var paddedGroup = offlineDevice.Group;

                sb.AppendLine(
                    $"{paddedAddress} | {paddedName} | {paddedGroup} | {offlineDevice.LastSeen?.ToLocalTime().ToString("g") ?? "Never seen"}");
            }

            return sb.ToString();
        }
    }
}