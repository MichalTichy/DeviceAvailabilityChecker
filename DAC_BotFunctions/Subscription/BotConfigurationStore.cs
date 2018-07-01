using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace DAC_BotFunctions.Subscription
{
    public class BotConfigurationStore
    {
        private readonly CloudBlobContainer container;

        public BotConfigurationStore(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudBlobClient();
            container = client.GetContainerReference(containerName);
        }

        public async Task<BotConfiguration> Load()
        {
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference("bots.config");
            if (await blob.ExistsAsync())
            {
                var json = blob.DownloadText(Encoding.UTF8);
                return JsonConvert.DeserializeObject<BotConfiguration>(json);
            }
            else
            {
                return new BotConfiguration();
            }
        }

        public async Task Save(BotConfiguration config)
        {
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference("bots.config");
            var json = JsonConvert.SerializeObject(config);
            blob.UploadText(json, Encoding.UTF8);
        }

    }
}