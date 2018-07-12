using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL
{
    internal abstract class DataSourceBase
    {
        protected static CloudTable devicesTable;
        protected static CloudTable spotsTable;
        protected static CloudTable subscriptionsTable;

        protected static CloudStorageAccount cloudStorageAccount;
        internal DataSourceBase()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(DalEnvironmentVariables.StorageAccount);

            if (devicesTable != null && spotsTable != null) return;


            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            if (devicesTable == null)
            {

                devicesTable = tableClient.GetTableReference("Devices");
                devicesTable.CreateIfNotExistsAsync().RunSynchronously();

            }

            if (spotsTable == null)
            {
                spotsTable = tableClient.GetTableReference("Spots"); 
                spotsTable.CreateIfNotExistsAsync().RunSynchronously();
            }

            if (subscriptionsTable == null)
            {
                subscriptionsTable = tableClient.GetTableReference("Subscriptions");
                subscriptionsTable.CreateIfNotExistsAsync().RunSynchronously();
            }
        }

        internal virtual async Task<IEnumerable<T>> ExecuteQuery<T>(CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
        {
            TableContinuationToken token = null;
            var entities = new List<T>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }
    }
}