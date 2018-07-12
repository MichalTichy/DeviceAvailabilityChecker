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
                    sb.AppendLine(
                        $"In group {deviceGroup.Key} was detected that {Devices.Count} {Devices.GetCorrectNoun()} {Devices.GetCorrectVerb()} unavailable.");
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

            var lengthOfLongestName = devices.Select(t => t.Name.Length).Max();
            var lengthOfLongestAddress = devices.Select(t => t.Address.Length).Max();
            var lengthOfLongestGroup = devices.Select(t => t.Group.Length).Max();


            sb.AppendLine(
                $"{"ADDRESS".PadRight(lengthOfLongestAddress)} | {"NAME".PadRight(lengthOfLongestName)} | {"GROUP".PadRight(lengthOfLongestGroup)}| LAST SEEN");

            foreach (var offlineDevice in devices)
            {
                var paddedName = offlineDevice.Name.ToString().PadRight(lengthOfLongestName);
                var paddedAddress = offlineDevice.Address.ToString().PadRight(lengthOfLongestAddress);
                var paddedGroup = offlineDevice.Group.ToString().PadRight(lengthOfLongestGroup);

                sb.AppendLine(
                    $"{paddedAddress} | {paddedName} | {paddedGroup} |{offlineDevice.LastSeen?.ToString("g") ?? "Never seen"}");
            }

            return sb.ToString();
        }
    }
}