using System;
using System.Linq;
using System.Threading.Tasks;
using DAC_DAL.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL
{
    internal class DeviceSpotDataSource : DataSourceBase
    {
        public TableQuery<DeviceSpot> GetLastSpotQuery(DeviceEntity deviceEntity)
        {
            var partitionKey = DeviceSpot.GetPartitionKey(deviceEntity);

            return new TableQuery<DeviceSpot>().Where(
                    TableQuery.GenerateFilterCondition(nameof(deviceEntity.PartitionKey), QueryComparisons.Equal,
                        partitionKey))
                .Take(1);
        }
        public async Task<DeviceSpot> GetLastSpot(DeviceEntity device)
        {
            var deviceSpots = await ExecuteQuery(spotsTable, GetLastSpotQuery(device));
            return deviceSpots.SingleOrDefault();
        }

        private static DeviceSpot CreateDeviceSpot(DeviceEntity device)
        {
            var deviceSpot = new DeviceSpot()
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