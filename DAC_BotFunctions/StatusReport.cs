using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAC_BotFunctions.Messages.ProactiveMessages;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Builder.Azure;

namespace DAC_BotFunctions
{
    public static class StatusReport
    {
        [FunctionName("StatusReport")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            using (BotService.Initialize())
            {
                string group = await req.Content.ReadAsAsync<string>();
                var message = new FoundUnavailableDevicesMessage(group);
                message.Send();
                return req.CreateResponse(HttpStatusCode.Accepted);
            }
        }
    }
}
