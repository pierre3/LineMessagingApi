namespace Line.Messaging
{
    public class AudioMessage : MediaMessage
    {
        public long Duration { get; }

        public AudioMessage(string originalContentUrl, long duration)
            : base( MessageType.Audio, originalContentUrl)
        {
            Duration = duration;
        }
    }
}
