namespace Line.Messaging
{
    public class ImageMessage : MediaMessage
    {
        public string PreviewImageUrl { get; }

        public ImageMessage(string originalContentUrl, string previerImageUrl)
            : base(MessageType.Image, originalContentUrl)
        {
            PreviewImageUrl = previerImageUrl;
        }
    }
}
