using System;

namespace DAC_BotFunctions.Subscription
{
    public class BotSubscription
    {
        public Guid Id { get; set; }

        public string ChannelId { get; set; }
        public string TeamId { get; set; }

        public string ServiceUrl { get; set; }

        public string GroupName { get; set; }
        public string TenantId { get; set; }
    }
}