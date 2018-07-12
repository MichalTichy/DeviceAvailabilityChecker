using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAC_Common;
using DAC_DAL.Entities;

namespace DAC_DAL
{
    public class SubscriptionFacade
    {

        internal SubscriptionsDataSource SubscriptionDataSource = new SubscriptionsDataSource();

        public async Task RegisterBotSubscription(BotSubscription subscription)
        {
            var entity = CreateBotSubscription(subscription);
            await SubscriptionDataSource.RegisterSubscription(entity);
        }

        public async Task UpdateBotSubscription(BotSubscription subscription)
        {
            var entity = CreateBotSubscription(subscription);
            await SubscriptionDataSource.UpdateSubscription(entity);
        }

        public async Task UnregisterBotSubscription(string channelId, string @group)
        {
            await SubscriptionDataSource.UnregisterSubscription(@group,channelId);
        }

        public async Task<IEnumerable<BotSubscription>> GetAllSubscriptions()
        {
            var botSubscriptionEntities = await SubscriptionDataSource.GetAllSubscriptions();
            return botSubscriptionEntities.Select(CreateBotSubscription);
        }
        public async Task<IEnumerable<BotSubscription>> GetAllSubscriptionsWithGroup(string group)
        {
            var botSubscriptionEntities = await SubscriptionDataSource.GetAllSubscriptionsInGroup(group);
            return botSubscriptionEntities.Select(CreateBotSubscription);
        }
        public async Task<IEnumerable<BotSubscription>> GetAllSubscriptionsWithChannelId(string channelId)
        {
            var botSubscriptionEntities = await SubscriptionDataSource.GetAllSubscriptionsInChannel(channelId);
            return botSubscriptionEntities.Select(CreateBotSubscription);
        }

        private BotSubscriptionEntity CreateBotSubscription(BotSubscription subscription)
        {
            return new BotSubscriptionEntity()
            {
                ChannelId = subscription.ChannelId,
                GroupName = subscription.GroupName,
                ServiceUrl = subscription.ServiceUrl,
                TeamId = subscription.TeamId,
                TenantId = subscription.TeamId,
                LastActivity = subscription.LastActivity
            };
        }

        protected BotSubscription CreateBotSubscription(BotSubscriptionEntity entity)
        {
            return new BotSubscription()
            {
                ChannelId = entity.ChannelId,
                GroupName = entity.GroupName,
                ServiceUrl = entity.ServiceUrl,
                TeamId = entity.TeamId,
                TenantId = entity.TenantId,
                LastActivity = entity.LastActivity
            };
        }
    }
}