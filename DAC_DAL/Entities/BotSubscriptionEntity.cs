using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL.Entities
{
    public class BotSubscriptionEntity : TableEntity
    {
        [IgnoreProperty]
        public string ChannelId {
            get => RowKey;
            set => RowKey = value;
        }
        public string TeamId { get; set; }

        public string ServiceUrl { get; set; }

        [IgnoreProperty]
        public string GroupName
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string TenantId { get; set; }
    }
}