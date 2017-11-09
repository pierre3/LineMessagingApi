using Microsoft.WindowsAzure.Storage.Table;

namespace WebAppWithBotFrameworkSample.Models
{
    public class EventSourceLocation : EventSourceState
    {
        public string Location { get; set; }

        public EventSourceLocation() { }
    }
}