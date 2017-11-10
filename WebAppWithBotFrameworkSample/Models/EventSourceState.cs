using Microsoft.WindowsAzure.Storage.Table;

namespace WebAppWithBotFrameworkSample.Models
{
    public class EventSourceState : TableEntity
    {
        [IgnoreProperty]
        public string SourceType { get { return PartitionKey; } set { PartitionKey = value; } }
        [IgnoreProperty]
        public string SourceId { get { return RowKey; } set { RowKey = value; } }
        
        public EventSourceState() { }
    }
}
