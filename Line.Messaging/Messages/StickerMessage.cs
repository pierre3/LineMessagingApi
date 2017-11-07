namespace Line.Messaging
{
    /// <summary>
    /// Sticker. For a list of the sticker IDs for stickers that can be sent with the Messaging API, see Sticker list.
    /// </summary>
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
