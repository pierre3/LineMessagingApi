namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Provider of the media file.
    /// </summary>
    public enum ContentProviderType
    {
        /// <summary>
        /// LINE. The binary media data can be retrieved from the content endpoint.
        /// </summary>
        Line,
        /// <summary>
        /// Provider other than LINE
        /// </summary>
        External
    }
}
