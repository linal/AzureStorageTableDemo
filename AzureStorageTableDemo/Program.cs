using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageTableDemo
{
    class Program
    {
        private const string TableName = "People";

        static void Main(string[] args)
        {
            // create out storage account connection
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            // get a table client
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            // get the table that we want to do stuff with
            var tableReference = cloudTableClient.GetTableReference(TableName);

            // make sure we have some data to play with
            PopulateTableData(tableReference);

            string personId;
            do
            {
                Console.Write("Person Search Or Lookup:");
                personId = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(personId))
                {
                    int id;
                    if (Int32.TryParse(personId, out id))
                    {
                        var tableOperation = TableOperation.Retrieve<Person>(Person.Partition, personId);
                        var tableResult = tableReference.Execute(tableOperation);
                        var person = (Person)tableResult.Result;
                        if (person != null)
                        {
                            Console.WriteLine("{0}:\t{1}", person.RowKey, person.Name);
                        }
                    }
                    else
                    {
                        var tableQuery =
                            new TableQuery<Person>().Where(TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, personId));
                        var people = tableReference.ExecuteQuery(tableQuery);
                        foreach (var person in people)
                        {
                            Console.WriteLine("{0}:\t{1}", person.RowKey, person.Name);
                        }
                    }
                }
            } while (!string.IsNullOrWhiteSpace(personId));
        }

        private static void PopulateTableData(CloudTable cloudTable)
        {
            // if the table does not exist then create it and populate it wih some data
            if (!cloudTable.Exists())
            {
                cloudTable.CreateIfNotExists();

                var tableBatchOperation = new TableBatchOperation();
                for (int i = 0; i < 100; i++)
                {
                    tableBatchOperation.Add(
                        TableOperation.Insert(new Person(i.ToString(), string.Format("Person {0}", i))));
                }

                cloudTable.ExecuteBatch(tableBatchOperation);
            }
        }

    }
}
