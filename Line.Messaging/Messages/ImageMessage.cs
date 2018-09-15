namespace Line.Messaging
{
    /// <summary>
    /// Image
    /// https://developers.line.me/en/docs/messaging-api/reference/#image
    /// </summary>
    public class ImageMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Image;

        /// <summary>
        /// These properties are used for the quick reply feature
        /// </summary>
        public QuickReply QuickReply { get; set; }

        /// <summary>
        /// Image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG
        /// Max: 1024 x 1024
        /// Max: 1 MB
        /// </summary>
        public string OriginalContentUrl { get; }

        /// <summary>
        /// Preview image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG
        /// Max: 240 x 240
        /// Max: 1 MB
        /// </summary>
        public string PreviewImageUrl { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalContentUrl">
        /// Image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG
        /// Max: 1024 x 1024
        /// Max: 1 MB
        /// </param>
        /// <param name="previerImageUrl">
        /// Preview image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG
        /// Max: 240 x 240
        /// Max: 1 MB
        /// </param>
        /// <param name="quickReply">
        /// QuickReply
        /// </param>
        public ImageMessage(string originalContentUrl, string previerImageUrl, QuickReply quickReply = null)
        {
            OriginalContentUrl = originalContentUrl;
            PreviewImageUrl = previerImageUrl;
            QuickReply = quickReply;
        }
    }
}
