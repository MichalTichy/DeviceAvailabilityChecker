using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAC_BotFunctions;
using DAC_BotFunctions.Subscription;
using DAC_Common;
using DAC_DAL;

namespace DAC_BotFunctions.Messages.ProactiveMessages
{
    public class FoundUnavailableDevicesMessage : MessageBase
    {
        public string Group { get; }

        public FoundUnavailableDevicesMessage(string deviceGroup)
        {
            Group = deviceGroup;
        }

        public override async Task<string> Build()
        {
            var devices = await GetUnavailableDevices();
            var offlineDevices = devices.ToArray();

            if (!offlineDevices.Any())
                return null;

            var sb = new StringBuilder();

            sb.AppendLine($"In group {Group} was detected that {offlineDevices.Length} {offlineDevices.GetCorrectVerb(usePastTense:true)} unavailable.");
            sb.AppendLine(string.Empty);

            DeviceReport.GetDevicesReport(offlineDevices);

            return sb.ToString();
        }

        private async Task<IEnumerable<DeviceReport>> GetUnavailableDevices()
        {
            var dataSource = new DataSource();
            await dataSource.Init();

            return await dataSource.GetUnavailableDevices();
        }

        public override async Task<IEnumerable<BotSubscription>> GetRecipients()
        {
            var store = 
                await new BotConfigurationStore(Environment.GetEnvironmentVariable("AzureWebJobsStorage"),"config").Load();

            return store.Subscriptions.Where(s => s.GroupName == Group);
        }
    }
}