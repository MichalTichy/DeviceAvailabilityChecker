using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAC_DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace DAC_ApiFunctions
{
    public static class GetAll
    {
        [FunctionName("GetAll")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices")]HttpRequestMessage req, TraceWriter log)
        {
            var deviceFacade = new DeviceFacade();
            var devices = await deviceFacade.GetAllDevices();

            return req.CreateResponse(HttpStatusCode.OK, devices);

        }
    }
}
