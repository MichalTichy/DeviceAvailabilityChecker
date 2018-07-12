using System.Linq;
using System.Threading.Tasks;
using DAC_BotFunctions.Messages.ProactiveMessages;
using DAC_DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Builder.Azure;

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

                foreach (var deviceGroup in unavailableDevices.GroupBy(t=>t.Group))
                {
                    var message=new DeviceStatusMessage(deviceGroup.ToList());
                    var recipients = await subscriptionFacade.GetAllSubscriptionsWithGroup(deviceGroup.Key);
                    await message.SendMessageToChannel("Found unavailable devices!", recipients);
                }
            }
        }
    }
}
