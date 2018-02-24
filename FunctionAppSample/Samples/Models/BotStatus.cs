using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppSample
{
    public class BotStatus : EventSourceState
    {
        public string Location { get; set; }
        public string CurrentApp { get; set; }

        public BotStatus() { }
    }
}
