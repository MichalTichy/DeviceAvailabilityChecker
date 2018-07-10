using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAC_Common;
using DAC_DAL.Entities;

namespace DAC_DAL
{
    public class ReportsFacade
    {

        internal DeviceDataSource DeviceDataSource = new DeviceDataSource();
        internal DeviceSpotDataSource DeviceSpotDataSource = new DeviceSpotDataSource();

        public async Task ReportDeviceSpot(string group, string address)
        {
            await DeviceSpotDataSource.ReportDeviceSpot(new DeviceEntity()
            {
                Address = address,
                Group = group
            });
        }

        public async Task<IEnumerable<DeviceReport>> GetDeviceReports()
        {
            var devices = await DeviceDataSource.GetAllDevices();

            var deviceModels = new List<DeviceReport>();
            foreach (var device in devices)
            {
                var deviceModel = await GetDeviceModel(device);
                deviceModels.Add(deviceModel);

            }

            return deviceModels;
        }

        private async Task<DeviceReport> GetDeviceModel(DeviceEntity deviceEntity)
        {
            if (deviceEntity == null)
                return null;

            var lastSpot = await DeviceSpotDataSource.GetLastSpot(deviceEntity);

            return new DeviceReport()
            {
                Address = deviceEntity.Address,
                Name = deviceEntity.Name,
                Group = deviceEntity.Group,
                LastSeen = lastSpot?.Date
            };

        }
        public async Task<IEnumerable<DeviceReport>> GetUnavailableDeviceReports()
        {
            var deviceReports = await GetDeviceReports();
            return deviceReports.Where(t => t.IsUnavailable);
        }
    }
}