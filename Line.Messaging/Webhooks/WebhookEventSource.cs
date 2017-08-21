using System;

namespace Line.Messaging.Webhooks
{
    public abstract class WebhookEventSource
    {
        public EventSourceType Type { get; }

        public string UserId { get; }

        public WebhookEventSource(EventSourceType type, string userId)
        {
            Type = type;
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
            switch (sourceType)
            {
                case EventSourceType.User:
                    return new SourceUser(EventSourceType.User, (string)source.userId);
                case EventSourceType.Group:
                    return new SourceGroup(EventSourceType.Group, (string)source.groupId, (string)source.userId);
                case EventSourceType.Room:
                    return new SourceRoom(EventSourceType.Room, (string)source.roomId, (string)source.userId);
                default:
                    return null;
            }
        }
    }

    public class SourceUser : WebhookEventSource
    {
        public SourceUser(EventSourceType type, string userId) : base(type, userId)
        {
        }
    }

    public class SourceGroup : WebhookEventSource
    {
        public string GroupId { get; }

        public SourceGroup(EventSourceType type, string groupId, string userId) : base(type, userId)
        {
            GroupId = groupId;
        }
    }

    public class SourceRoom : WebhookEventSource
    {
        public string RoomId { get; }

        public SourceRoom(EventSourceType type, string roomId, string userId) : base(type, userId)
        {
            RoomId = roomId;
        }
    }
}
