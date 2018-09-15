namespace Line.Messaging
{
    /// <summary>
    /// Sticker. For a list of the sticker IDs for stickers that can be sent with the Messaging API, see Sticker list.
    /// </summary>
    public class StickerMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Sticker;

        /// <summary>
        /// These properties are used for the quick reply feature
        /// </summary>
        public QuickReply QuickReply { get; set; }

        /// <summary>
        /// Package ID for a set of stickers. For information on package IDs, see the Sticker list.
        /// </summary>
        public string PackageId { get; }

        /// <summary>
        /// Sticker ID. For a list of sticker IDs for stickers that can be sent with the Messaging API, see the Sticker list.
        /// </summary>
        public string StickerId { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="packageId">
        /// Package ID for a set of stickers. For information on package IDs, see the Sticker list.
        /// </param>
        /// <param name="stickerId">
        /// Sticker ID. For a list of sticker IDs for stickers that can be sent with the Messaging API, see the Sticker list.
        /// </param>
        /// <param name="quickReply">
        /// QuickReply
        /// </param>
        public StickerMessage(string packageId, string stickerId, QuickReply quickReply = null)
        {
            PackageId = packageId;
            StickerId = stickerId;
            QuickReply = quickReply;
        }
    }
}
