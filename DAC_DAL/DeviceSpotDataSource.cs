using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_DAL.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL
{
    internal class DeviceSpotDataSource : DataSourceBase
    {
        public TableQuery<DeviceSpotEntity> GetLastSpotQuery(DeviceEntity deviceEntity)
        {
            var partitionKey = DeviceSpotEntity.GetPartitionKey(deviceEntity);

            return new TableQuery<DeviceSpotEntity>().Where(
                    TableQuery.GenerateFilterCondition(nameof(deviceEntity.PartitionKey), QueryComparisons.Equal,
                        partitionKey))
                .Take(1);
        }
        public async Task<DeviceSpotEntity> GetLastSpot(DeviceEntity device)
        {
            var deviceSpots = await ExecuteQuery(spotsTable, GetLastSpotQuery(device),1);
            return deviceSpots.SingleOrDefault();
        }

        private static DeviceSpotEntity CreateDeviceSpot(DeviceEntity device)
        {
            var deviceSpot = new DeviceSpotEntity()
            {
                DeviceEntity = device,
                Date = DateTime.Now
            };
            deviceSpot.UpdateTableKeys();
            return deviceSpot;
        }

        public async Task ReportDeviceSpot(DeviceEntity device)
        {
            var spot = CreateDeviceSpot(device);
            var operation = TableOperation.Insert(spot);
            await spotsTable.ExecuteAsync(operation);
        }
    }
}