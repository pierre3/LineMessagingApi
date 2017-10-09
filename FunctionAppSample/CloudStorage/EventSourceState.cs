using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppSample
{
    public class EventSourceState : TableEntity
    {
        [IgnoreProperty]
        public string SourceType { get => PartitionKey; set => PartitionKey = value; }
        [IgnoreProperty]
        public string SourceId { get => RowKey; set => RowKey = value; }
        
        public EventSourceState() { }
    }
}
