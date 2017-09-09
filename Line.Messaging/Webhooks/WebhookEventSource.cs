using System;

namespace Line.Messaging.Webhooks
{
    public class WebhookEventSource
    {
        public EventSourceType Type { get; }

        public string Id { get; }

        public string UserId { get; }

        public WebhookEventSource(EventSourceType type, string sourceId, string userId)
        {
            Type = type;
            Id = sourceId;
            UserId = userId;
        }

        public static WebhookEventSource CreateFrom(dynamic dynamicObject)
        {
            var source = dynamicObject?.source;
            if (source == null) { return null; }
            if (!Enum.TryParse((string)source.type, true, out EventSourceType sourceType))
            {
                return null;
            }
            var sourceId = "";
            switch (sourceType)
            {
                case EventSourceType.User:
                    sourceId = (string)source.userId;
                    break;
                case EventSourceType.Group:
                    sourceId = (string)source.groupId;
                    break;
                case EventSourceType.Room:
                    sourceId = (string)source.roomId;
                    break;
                default:
                    return null;
            }
            return new WebhookEventSource(sourceType, sourceId, (string)source.userId);
        }
    }

}
