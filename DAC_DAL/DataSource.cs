using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAC_Common;
using DAC_DAL.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL
{
    public class DataSource
    {
        protected CloudTable devicesTable;
        protected CloudTable spotsTable;

        protected static CloudStorageAccount cloudStorageAccount;



        static DataSource()
        {
            cloudStorageAccount= CloudStorageAccount.Parse(EnvironmentVariables.StorageAccount);

        }

        public async Task Init()
        {
            var tableClient = cloudStorageAccount.CreateCloudTableClient();

            devicesTable = tableClient.GetTableReference("Devices");
            await devicesTable.CreateIfNotExistsAsync();

            spotsTable = tableClient.GetTableReference("Spots");
            await spotsTable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            var deviceEntities = await GetAllDeviceEntities();
            return deviceEntities.Select(ConvertToDevice);
        }

        public Device ConvertToDevice(DeviceEntity device)
        {
            return new Device()
            {
                Address = device.Address,
                Group = device.Group,
                Name = device.Name
            };
        }

        protected async Task<IEnumerable<DeviceEntity>> GetAllDeviceEntities()
        {
            var query=new TableQuery<DeviceEntity>();
            TableContinuationToken token = null;
            var entities=new List<DeviceEntity>();
            do
            {
                var queryResult = await devicesTable.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token!=null);

            return entities;
        }
        
        public async Task ReportDeviceSpot(string group, string address)
        {
            var device = await GetDevice(group, address);
            var spot = CreateDeviceSpot(device);

            var operation = TableOperation.Insert(spot);
            await spotsTable.ExecuteAsync(operation);
        }

        protected async Task<DeviceEntity> GetDevice(string group, string address)
        {
            var operation = TableOperation.Retrieve<DeviceEntity>(group, address);
            var result = await devicesTable.ExecuteAsync(operation);
            return (DeviceEntity)result.Result;
        }
        protected async Task<DeviceSpot> GetLastSpot(DeviceEntity deviceEntity)
        {

            var partitionKey = DeviceSpot.GetPartitionKey(deviceEntity);

            var query = new TableQuery<DeviceSpot>().Where(
                    TableQuery.GenerateFilterCondition(nameof(deviceEntity.Group), QueryComparisons.Equal,
                        partitionKey))
                .Take(1);

            var result = await spotsTable.ExecuteQuerySegmentedAsync(query,new TableContinuationToken());
            return result.Results.Single();
        }


        private static DeviceSpot CreateDeviceSpot(DeviceEntity deviceEntity)
        {
            var deviceSpot = new DeviceSpot()
            {
                DeviceEntity = deviceEntity,
                Date = DateTime.Now
            };
            deviceSpot.UpdateTableKeys();
            return deviceSpot;
        }

        private static DeviceEntity CreateDevice(DeviceReport deviceModel)
        {
            var deviceEntity = new DeviceEntity()
            {
                Address = deviceModel.Address,
                Group = deviceModel.Group,
                Name = deviceModel.Name
            };
            return deviceEntity;
        }

        private async Task<DeviceReport> GetDeviceModel(DeviceEntity deviceEntity)
        {
            if (deviceEntity == null)
                return null;

            var lastSpot = await GetLastSpot(deviceEntity);

            return new DeviceReport()
            {
                Address = deviceEntity.Address,
                Name = deviceEntity.Name,
                Group = deviceEntity.Group,
                LastSeen = lastSpot?.Date
            };

        }

        public async Task<IEnumerable<DeviceReport>> GetUnavailableDevices()
        {
            var devices = await GetAllDeviceEntities();

            var unavailableDevices=new List<DeviceReport>();
            var expectedTimeBetweenReports = DAC_Common.EnvironmentVariables.DelayInMinutesBetweenContacts;
            var allowedMissedContacts = DAC_Common.EnvironmentVariables.NumberOfAllowedMissedContacts;
            foreach (var device in devices)
            {
                var deviceModel = await GetDeviceModel(device);
                if (!deviceModel.LastSeen.HasValue || deviceModel.LastSeen.Value.AddMinutes(expectedTimeBetweenReports*allowedMissedContacts)<DateTime.Now)
                {
                    unavailableDevices.Add(deviceModel);
                }
            }

            return unavailableDevices;
        }

        public async Task RegisterDevice(DeviceReport device)
        {
            var deviceEntity = CreateDevice(device);
            

            var operation = TableOperation.Insert(deviceEntity);
            await devicesTable.ExecuteAsync(operation);

        }
    }
}
