using Microsoft.WindowsAzure.Storage.Table;

namespace DAC_DAL.Entities
{
    public class DeviceEntity : TableEntity

    {
        
        public string Name { get; set; }

        [IgnoreProperty]
        public string Group
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        [IgnoreProperty]
        public string Address
        {
            get => RowKey;
            set => RowKey = value;
        }
    }
}