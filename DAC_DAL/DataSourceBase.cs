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

        protected static CloudStorageAccount cloudStorageAccount;
        internal DataSourceBase()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(DalEnvironmentVariables.StorageAccount);

            if (devicesTable != null && spotsTable != null) return;


            var tableClient = cloudStorageAccount.CreateCloudTableClient();
            if (devicesTable == null)
            {

                devicesTable = tableClient.GetTableReference("Devices");
                devicesTable.CreateIfNotExistsAsync();

            }

            if (spotsTable == null)
            {
                spotsTable = tableClient.GetTableReference("Spots"); 
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