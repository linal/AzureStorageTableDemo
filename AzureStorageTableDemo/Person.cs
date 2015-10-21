using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageTableDemo
{
    public class Person : TableEntity
    {
        public Person()
        {
            
        }

        public Person(string id, string name)
        {
            this.PartitionKey = "Key";
            this.RowKey = id;
            this.Name = name;
        }

        public string Name { get; set; }
    }
}