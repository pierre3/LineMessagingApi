using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace WebAppWithBotFrameworkSample.CloudStorage
{

    public class TableStorage<T> where T : TableEntity, new()
    {
        private CloudTableClient _tableClient;
        private CloudTable _table;

        protected TableStorage(string connectionString)
        {
            var strageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = strageAccount.CreateCloudTableClient();
        }

        protected async Task InitializeAsync(string tableName)
        {
            _table = _tableClient.GetTableReference(tableName);
            await _table.CreateIfNotExistsAsync();
        }

        public static async Task<TableStorage<T>> CreateAsync(string connectionString,string tableName)
        {
            var result = new TableStorage<T>(connectionString);
            await result.InitializeAsync(tableName);
            return result;
        }

        public async Task AddAsync(string partitionKey, string rowKey)
        {
            if ((await FindAsync(partitionKey, rowKey)) != null) { return; }

            var newItem = new T() { PartitionKey = partitionKey, RowKey = rowKey };
            await _table.ExecuteAsync(TableOperation.Insert(newItem));
        }

        public async Task UpdateAsync(T item)
        {
            await _table.ExecuteAsync(TableOperation.InsertOrReplace(item));
        }

        public async Task<T> FindAsync(string partitionKey, string rowKey)
        {
            var ope = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var retrieveResult = await _table.ExecuteAsync(ope);
            if (retrieveResult.Result == null) { return null; }
            return (T)(retrieveResult.Result);
        }

        public async Task DeleteAsync(string partitionKey, string rowKey)
        {
            var item = await FindAsync(partitionKey, rowKey);
            if (item == null) { return; }

            var ope = TableOperation.Delete(item);
            await _table.ExecuteAsync(ope);
        }
    }
}
