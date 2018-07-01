using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL.Entities
{
    public class DeviceSpot : TableEntity
    {
        public DeviceEntity DeviceEntity { get; set; }
        public DateTime Date { get; set; }

        public static string GetPartitionKey(string group, string address)
        {
            return $"{group}|{address}";
        }
        internal static string GetPartitionKey(DeviceEntity device)
        {
            return $"{device.Group}|{device.Address}";
        }

        internal static string GetRowKey(DateTime dateTime)
        {
            return string.Format("{0:D19}", DateTime.MaxValue.Ticks - dateTime.Ticks);
        }

        public void UpdateTableKeys()
        {
            this.PartitionKey = GetPartitionKey(DeviceEntity);
            this.RowKey = GetRowKey(Date);
        }
    }
}
