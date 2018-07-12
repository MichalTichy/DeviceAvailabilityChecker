using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_BotFunctions.Messages.ProactiveMessages;
using DAC_DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Connector;

namespace DAC_BotFunctions
{
    public static class StatusReport
    {
        [FunctionName("StatusReport")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            using (BotService.Initialize())
            {
                var subscriptionFacade = new SubscriptionFacade();
                var reportsFacade = new ReportsFacade();
                var unavailableDevices = await reportsFacade.GetUnavailableDeviceReports();

                var subscriptions = (await subscriptionFacade.GetAllSubscriptions()).ToArray();

                foreach (var subscription in subscriptions.Where(t => DateTime.Now.Subtract(t.LastActivity.Created) > TimeSpan.FromDays(1)))
                {
                    var connector = new ConnectorClient(new Uri(subscription.ServiceUrl), Environment.GetEnvironmentVariable("MicrosoftAppId"), Environment.GetEnvironmentVariable("MicrosoftAppPassword"));
                    await connector.Conversations.DeleteActivityAsync(subscription.LastActivity.ConversationId, subscription.LastActivity.ActitityId);
                    subscription.LastActivity = null;
                    await subscriptionFacade.UpdateBotSubscription(subscription);
                }

                foreach (var deviceGroup in unavailableDevices.GroupBy(t=>t.Group))
                {
                    var message=new DeviceStatusMessage(deviceGroup.ToList());
                    var recipients = subscriptions.Where(t=>t.GroupName == deviceGroup.Key);
                    await message.SendMessageToChannel("Found unavailable devices!", recipients);
                }
            }
        }
    }
}
