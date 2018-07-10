using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAC_DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DAC_ApiFunctions
{
    public static class ReportStatus
    {
        [FunctionName("ReportSpottedDevice")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "spot/{group}/{address}")]HttpRequestMessage req,string group, string address,TraceWriter log)
        {
            var reportsFacade = new ReportsFacade();

            await reportsFacade.ReportDeviceSpot(group, address);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
