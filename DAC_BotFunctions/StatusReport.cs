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
        public static async Task Run([TimerTrigger("*/5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            using (BotService.Initialize())
            {
                var dataSource = new DataSource();
                await dataSource.Init();
                var unavailableDevices = await dataSource.GetUnavailableDevices();

                foreach (var deviceGroup in unavailableDevices.GroupBy(t=>t.Group))
                {
                    var message=new FoundUnavailableDevicesInGroupMessage(deviceGroup.Key,deviceGroup.ToList());
                    await message.Send();
                }
            }
        }
    }
}
