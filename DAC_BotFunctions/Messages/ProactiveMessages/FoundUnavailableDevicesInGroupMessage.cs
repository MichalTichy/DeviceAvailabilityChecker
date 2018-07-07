using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAC_BotFunctions.Subscription;
using DAC_Common;
using DAC_DAL;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public class FoundUnavailableDevicesInGroupMessage : MessageBase
    {
        public IReadOnlyCollection<DeviceReport> UnavailableDevices { get; }
        public string Group { get; }

        public FoundUnavailableDevicesInGroupMessage(string deviceGroup,List<DeviceReport> unavailableDevices)
        {
            UnavailableDevices = unavailableDevices.AsReadOnly();
            Group = deviceGroup;

            if(UnavailableDevices.Any(t=>t.Group!=Group))
                throw new ArgumentException("Not all given devices are in correct group!");
        }

        public override async Task<string> Build()
        {

            if (!UnavailableDevices.Any())
                return null;

            var sb = new StringBuilder();

            sb.AppendLine($"In group {Group} was detected that {UnavailableDevices.Count} {UnavailableDevices.GetCorrectVerb(usePastTense:true)} unavailable.");
            sb.AppendLine(String.Empty);

            GetDevicesReport(UnavailableDevices);

            return sb.ToString();
        }

        protected override async Task<IEnumerable<BotSubscription>> GetRecipients()
        {
            var store = 
                await new BotConfigurationStore(DalEnvironmentVariables.StorageAccount,"config").Load();

            return store.Subscriptions.Where(s => s.GroupName == Group);
        }

        protected static string GetDevicesReport(IEnumerable<DeviceReport> offlineDevices)
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