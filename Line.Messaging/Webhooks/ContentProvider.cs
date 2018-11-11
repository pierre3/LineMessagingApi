namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Content provider of media message
    /// </summary>
    public class ContentProvider
    {
        /// <summary>
        /// Content Provider Type
        /// </summary>
        public ContentProviderType Type { get; }

        /// <summary>
        /// URL of the media file. Only included when contentProvider.type is external.
        /// </summary>
        public string OriginalContentUrl { get; }


        /// <summary>
        /// URL of the preview image. Only included when contentProvider.type is external.
        /// </summary>
        public string PreviewImageUrl { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">
        /// Provider of the media file.
        /// / line: LINE.The binary media data can be retrieved from the content endpoint.
        /// / external: Provider other than LINE
        /// </param>
        /// <param name="originalContentUrl">
        /// URL of the media file. Only included when contentProvider.type is external.
        /// </param>
        /// <param name="previewImageUrl">
        /// URL of the preview image. Only included when contentProvider.type is external.
        /// </param>
        public ContentProvider(ContentProviderType type, string originalContentUrl, string previewImageUrl)
        {
            Type = type;
            OriginalContentUrl = originalContentUrl;
            PreviewImageUrl = previewImageUrl;
        }
    }
}
