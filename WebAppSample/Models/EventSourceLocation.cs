using Microsoft.WindowsAzure.Storage.Table;

namespace WebAppSample.Models
{
    public class EventSourceLocation : EventSourceState
    {
        public string Location { get; set; }

        public EventSourceLocation() { }
    }
}