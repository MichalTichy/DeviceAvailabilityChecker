using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAC_Common;
using DAC_DAL.Entities;

namespace DAC_DAL
{
    public class DeviceFacade
    {
        internal DeviceDataSource DeviceDataSource=new DeviceDataSource();
        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            var devices = await DeviceDataSource.GetAllDevices();
            return devices.Select(ConvertToDevice);
        }
        
        public async Task RegisterDevice(Device device)
        {
            var deviceEntity = CreateDevice(device);
            await DeviceDataSource.RegisterDevice(deviceEntity);
        }

        public async Task UnregisterDevice(string group, string address)
        {
            await DeviceDataSource.UnregisterDevice(group, address);
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

        private  DeviceEntity CreateDevice(Device deviceModel)
        {
            var deviceEntity = new DeviceEntity()
            {
                Address = deviceModel.Address,
                Group = deviceModel.Group,
                Name = deviceModel.Name
            };
            return deviceEntity;
        }
    }
}