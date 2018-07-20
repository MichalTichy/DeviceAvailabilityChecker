using System;
using DAC_Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL.Entities
{
    public class BotSubscriptionEntity : TableEntity
    {
        public string ChannelId { get; set; }

        public string TeamId
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string ServiceUrl { get; set; }

        [IgnoreProperty]
        public string GroupName
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string TenantId { get; set; }

        public DateTime? LastActivityDate { get; set; }
        public string ConversationId { get; set; }
        public string ActitityId { get; set; }
    }
}