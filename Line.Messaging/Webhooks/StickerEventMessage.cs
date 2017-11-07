namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Message object which contains the sticker data sent from the source. For a list of basic LINE stickers and sticker IDs, see sticker list.
    /// </summary>
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
