namespace Line.Messaging
{
    public class StickerMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Sticker;

        public string PackageId { get; }

        public string StickerId { get; }

        public StickerMessage(string packageId, string stickerId)

        {
            PackageId = packageId;
            StickerId = stickerId;
        }
    }
}
