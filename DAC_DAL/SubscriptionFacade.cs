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

        public async Task UnregisterBotSubscription(string teamId, string @group)
        {
            await SubscriptionDataSource.UnregisterSubscription(@group,teamId);
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
        public async Task<IEnumerable<BotSubscription>> GetAllSubscriptionsWithChannelId(string teamId)
        {
            var botSubscriptionEntities = await SubscriptionDataSource.GetAllSubscriptionsInChannel(teamId);
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
                TenantId = subscription.TenantId,
                LastActivityDate = subscription.LastActivity?.Created,
                ActitityId = subscription.LastActivity?.ActitityId,
                ConversationId = subscription.LastActivity?.ConversationId
            };
        }

        protected BotSubscription CreateBotSubscription(BotSubscriptionEntity entity)
        {

            LastActivity activity;
            if (entity.LastActivityDate == null)
            {
                activity = null;
            }
            else
            {
                activity = new LastActivity()
                {
                    ActitityId = entity.ActitityId,
                    ConversationId = entity.ConversationId,
                    Created = entity.LastActivityDate.Value,
                };
            }

            return new BotSubscription()
            {
                ChannelId = entity.ChannelId,
                GroupName = entity.GroupName,
                ServiceUrl = entity.ServiceUrl,
                TeamId = entity.TeamId,
                TenantId = entity.TeamId,
                LastActivity = activity
            };
        }
    }
}