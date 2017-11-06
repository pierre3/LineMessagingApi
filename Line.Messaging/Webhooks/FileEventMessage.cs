namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Message object which contains the file sent from the source. The binary data can be retrieved from the content endpoint.
    /// </summary>
    public class FileEventMessage : EventMessage
    {
        /// <summary>
        /// file name
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; }

        public FileEventMessage(string id, string fileName, long fileSize) : base(EventMessageType.File, id)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
