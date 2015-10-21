using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageTableDemo
{
    public class Person : TableEntity
    {
        public const string Partition = "Key";
        public Person()
        {
            
        }

        public Person(string id, string name)
        {
            this.PartitionKey = Partition;
            this.RowKey = id;
            this.Name = name;
        }

        public string Name { get; set; }
    }
}