using System;

namespace Line.Messaging.Webhooks
{
    public class WebhookEventSource
    {
        public EventSourceType Type { get; }

        public string EntryId { get; }

        public string UserId { get; }

        public WebhookEventSource(EventSourceType type, string entryId, string userId)
        {
            Type = type;
            EntryId = entryId;
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
            var entryId = "";
            switch (sourceType)
            {
                case EventSourceType.User:
                    entryId = (string)source.userId;
                    break;
                case EventSourceType.Group:
                    entryId = (string)source.groupId;
                    break;
                case EventSourceType.Room:
                    entryId = (string)source.roomId;
                    break;
                default:
                    return null;
            }
            return new WebhookEventSource(sourceType, entryId, (string)source.userId);
        }
    }

}
