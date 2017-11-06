using System;

namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Webhook Event Source. Source could be User, Group or Room.
    /// </summary>
    public class WebhookEventSource
    {
        public EventSourceType Type { get; }

        /// <summary>
        /// User, Group or Room Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// UserId of the Group or Room
        /// </summary>
        public string UserId { get; }

        public WebhookEventSource(EventSourceType type, string sourceId, string userId)
        {
            Type = type;
            Id = sourceId;
            UserId = userId;
        }

        internal static WebhookEventSource CreateFrom(dynamic dynamicObject)
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
