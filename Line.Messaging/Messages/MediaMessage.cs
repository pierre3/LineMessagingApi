using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Line.Messaging
{
    public class MediaMessage : ISendMessage
    {
        public MessageType Type { get; }

        public string OriginalContentUrl { get; }
        
        public MediaMessage(MessageType type, string originalContentUrl)
        {
            Type = type;
            OriginalContentUrl = originalContentUrl;
        }
    }
}
