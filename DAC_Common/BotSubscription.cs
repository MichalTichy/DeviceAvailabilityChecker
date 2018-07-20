using System;

namespace DAC_Common
{
    public class BotSubscription
    {
        public string ChannelId { get; set; }
        public string TeamId { get; set; }

        public string ServiceUrl { get; set; }

        public string GroupName { get; set; }
        public string TenantId { get; set; }

        public LastActivity LastActivity { get; set; }
    }

    public class LastActivity
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string ConversationId { get; set; }
        public string ActitityId { get; set; }
    }
}