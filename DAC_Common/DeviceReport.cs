using System;
using System.Linq;
using System.Text;

namespace DAC_Common
{
    public class DeviceReport : Device
    {
        public DateTime? LastSeen { get; set; }

        
        public static string GetDevicesReport(DeviceReport[] offlineDevices)
        {
            StringBuilder sb = new StringBuilder();

            var lengthOfLongestName = offlineDevices.Select(t => t.Name.Length).Max();
            var lengthOfLongestAddress = offlineDevices.Select(t => t.Address.Length).Max();
            var lengthOfLongestGroup = offlineDevices.Select(t => t.Group.Length).Max();


            sb.AppendLine(
                $"{"ADDRESS".PadRight(lengthOfLongestAddress)} | {"NAME".PadRight(lengthOfLongestName)} | {"GROUP".PadRight(lengthOfLongestGroup)}| LAST SEEN");

            foreach (var offlineDevice in offlineDevices)
            {
                var paddedName = offlineDevice.Name.ToString().PadRight(lengthOfLongestName);
                var paddedAddress = offlineDevice.Address.ToString().PadRight(lengthOfLongestAddress);
                var paddedGroup = offlineDevice.Group.ToString().PadRight(lengthOfLongestGroup);

                sb.AppendLine(
                    $"{paddedAddress} | {paddedName} | {paddedGroup} |{offlineDevice.LastSeen?.ToString("g") ?? ""}");
            }

            return sb.ToString();
        }
    }
}