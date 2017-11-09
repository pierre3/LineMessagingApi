namespace Line.Messaging
{
    /// <summary>
    /// Audio
    /// https://developers.line.me/en/docs/messaging-api/reference/#audio
    /// </summary>
    public class AudioMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Audio;

        /// <summary>
        /// URL of audio file (Max: 1000 characters)
        /// HTTPS
        /// m4a
        /// Less than 1 minute
        /// Max 10 MB
        /// </summary>
        public string OriginalContentUrl { get; }

        /// <summary>
        /// Length of audio file (milliseconds)
        /// </summary>
        public long Duration { get; }

        public AudioMessage(string originalContentUrl, long duration)
        {
            OriginalContentUrl = originalContentUrl;
            Duration = duration;
        }
    }
}
