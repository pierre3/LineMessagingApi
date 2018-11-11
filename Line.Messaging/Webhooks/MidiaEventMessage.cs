namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Media event message (Image, Video or Audio)
    /// </summary>
    public class MediaEventMessage : EventMessage
    {
        /// <summary>
        /// ContentProvider
        /// </summary>
        public ContentProvider ContentProvider { get; }

        /// <summary>
        /// Length of media file (milliseconds)
        /// </summary>
        public int? Duration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Event Message Type</param>
        /// <param name="id">Message ID</param>
        /// <param name="contentProvider">ContentProvider Object</param>
        /// <param name="duration">Duration</param>
        public MediaEventMessage(EventMessageType type, string id, ContentProvider contentProvider = null, int? duration = null) : base(type, id)
        {
            ContentProvider = contentProvider;
            Duration = duration;
        }
    }
}
