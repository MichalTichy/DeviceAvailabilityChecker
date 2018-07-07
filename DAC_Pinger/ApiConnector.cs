using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DAC_Common;
using Newtonsoft.Json;

namespace DAC_Pinger
{
    public class ApiConnector
    {
        private readonly Uri baseUrl;

        public ApiConnector(string baseUrl)
        {
            this.baseUrl = new Uri(baseUrl);
        }
        public ApiConnector()
        {
            this.baseUrl = new Uri(EnvironmentVariables.ApiUrl);
        }

        public async Task<ICollection<DeviceReport>> GetAll()
        {
            var targetUri = new Uri(baseUrl, "api/devices");

            using (var client = new HttpClient())
            using (HttpResponseMessage res = await client.GetAsync(targetUri))
            using (HttpContent content = res.Content)
            {
                var data = await content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ICollection<DeviceReport>>(data);
            }
        }

        public async Task ReportStatus(DeviceReport device)
      {
            var targetUri = new Uri(baseUrl, $"api/spot/{device.Group}/{device.Address}");
            using (var client = new HttpClient())

            using (HttpResponseMessage res = await client.GetAsync(targetUri))
            using (HttpContent content = res.Content)
            {
                var data = await content.ReadAsStringAsync();
            }
        }
    }
}