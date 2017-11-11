using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppSample
{
    public class EventSourceLocation : EventSourceState
    {
        public string Location { get; set; }

        public EventSourceLocation() { }
    }
}