using System;

namespace Line.Messaging.Webhooks
{

    public class EventMessage
    {
        public string Id { get; }

        public EventMessageType Type { get; }

        public EventMessage(EventMessageType type, string id)
        {
            Type = type;
            Id = id;
        }

        public static EventMessage CreateFrom(dynamic dynamicObject)
        {
            var message = dynamicObject?.message;
            if (message == null) { return null; }
            if (!Enum.TryParse<EventMessageType>((string)message.type, true, out EventMessageType messageType))
            {
                return null;
            }
            switch (messageType)
            {
                case EventMessageType.Text:
                    return new TextEventMessage((string)message.id, (string)message.text);
                case EventMessageType.Image:
                case EventMessageType.Audio:
                case EventMessageType.Video:
                    return new EventMessage(messageType, (string)message.id);
                case EventMessageType.Location:
                    return new LocationEventMessage((string)message.id, (string)message.title,
                        (decimal)message.latitude, (decimal)message.longitude);
                case EventMessageType.Sticker:
                    return new StickerEventMessage((string)message.id, (string)message.packageId, (string)message.stickerId);
                case EventMessageType.File:
                    return new FileEventMessage((string)message.id, (string)message.fileName, (long)message.fileSize);
                default:
                    return null;
            }

        }
    }

    public class TextEventMessage : EventMessage
    {
        public string Text { get; }

        public TextEventMessage(string id, string text) : base(EventMessageType.Text, id)
        {
            Text = text;
        }
    }


    public class FileEventMessage : EventMessage
    {
        public string FileName { get; }

        public long FileSize { get; }

        public FileEventMessage(string id, string fileName, long fileSize) : base(EventMessageType.File, id)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }

    public class LocationEventMessage : EventMessage
    {
        public string Title { get; }

        public decimal Latitude { get; }

        public decimal Longitude { get; }

        public LocationEventMessage(string id, string title, decimal latitude, decimal longitude) : base(EventMessageType.Location, id)
        {
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public class StickerEventMessage : EventMessage
    {
        public string PacageId { get; }

        public string StickerId { get; }

        public StickerEventMessage(string id, string packageId, string stickerId) : base(EventMessageType.Sticker, id)
        {
            PacageId = packageId;
            StickerId = stickerId;
        }
    }

}
