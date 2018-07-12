using System.Collections.Generic;
using System.Threading.Tasks;
using DAC_DAL.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL
{
    internal class SubscriptionsDataSource : DataSourceBase
    {

        protected virtual  TableQuery<BotSubscriptionEntity> GetAllSubscriptionsQuery()
        {
            return new TableQuery<BotSubscriptionEntity>();
        }

        protected virtual TableQuery<BotSubscriptionEntity> GetAllSubscriptionsInGroupQuery(string group)
        {
            return new TableQuery<BotSubscriptionEntity>().Where(TableQuery.GenerateFilterCondition(nameof(BotSubscriptionEntity.PartitionKey),QueryComparisons.Equal,group));
        }
        protected virtual TableQuery<BotSubscriptionEntity> GetAllSubscriptionsInChannelQuery(string channelId)
        {
            return new TableQuery<BotSubscriptionEntity>().Where(TableQuery.GenerateFilterCondition(nameof(BotSubscriptionEntity.RowKey), QueryComparisons.Equal, channelId));
        }



        public async Task<IEnumerable<BotSubscriptionEntity>> GetAllSubscriptions()
        {
            return await ExecuteQuery(subscriptionsTable, GetAllSubscriptionsQuery());
        }

       
        

        public async Task RegisterSubscription(BotSubscriptionEntity device)
        {
            var operation = TableOperation.Insert(device);
            await subscriptionsTable.ExecuteAsync(operation);
        }

        public async Task UpdateSubscription(BotSubscriptionEntity device)
        {
            var operation = TableOperation.InsertOrReplace(device);
            await subscriptionsTable.ExecuteAsync(operation);
        }
        
        public async Task UnregisterSubscription(string group, string channelId)
        {
            var operation = TableOperation.Delete(new BotSubscriptionEntity()
            {
                GroupName = group,
                ChannelId = channelId
            });

            await subscriptionsTable.ExecuteAsync(operation);
        }

        public async Task<IEnumerable<BotSubscriptionEntity>> GetAllSubscriptionsInGroup(string group)
        {
            return await ExecuteQuery(subscriptionsTable, GetAllSubscriptionsInGroupQuery(@group));
        }
        public async Task<IEnumerable<BotSubscriptionEntity>> GetAllSubscriptionsInChannel(string channelId)
        {
            return await ExecuteQuery(subscriptionsTable, GetAllSubscriptionsInChannelQuery(channelId));
        }

    }
}