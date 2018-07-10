using System.Collections.Generic;
using System.Threading.Tasks;
using DAC_DAL.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL
{
    internal class DeviceDataSource : DataSourceBase
    {

        public TableQuery<DeviceEntity> GetAllDevicesQuery()
        {
            return new TableQuery<DeviceEntity>();
        }

       

        public async Task<IEnumerable<DeviceEntity>> GetAllDevices()
        {
            return await ExecuteQuery(devicesTable, GetAllDevicesQuery());
        }

       
        

        public async Task RegisterDevice(DeviceEntity device)
        {
            var operation = TableOperation.Insert(device);
            await devicesTable.ExecuteAsync(operation);
        }
        
        public async Task UnregisterDevice(string @group, string address)
        {
            var operation = TableOperation.Delete(new DeviceEntity()
            {
                Group = group,
                Address = address
            });

            await devicesTable.ExecuteAsync(operation);
        }
    }
}