namespace Line.Messaging
{
    public class VideoMessage : MediaMessage
    {
        public string PreviewImageUrl { get; set; }

        public VideoMessage(string originalContentUrl, string previerImageUrl)
            : base(MessageType.Video, originalContentUrl)
        {
            PreviewImageUrl = previerImageUrl;
        }
    }
}
