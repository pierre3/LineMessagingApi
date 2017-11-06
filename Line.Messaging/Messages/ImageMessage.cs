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

        public ImageMessage(string originalContentUrl, string previerImageUrl)
        {
            OriginalContentUrl = originalContentUrl;
            PreviewImageUrl = previerImageUrl;
        }
    }
}
