namespace Line.Messaging
{
    public class ExternalLink
    {
        /// <summary>
        /// Webpage URL. Called when the label displayed after the video is tapped.
        /// / Max: 1000 characters
        /// </summary>
        public string LinkUri { get; }

        /// <summary>
        /// Label.Displayed after the video is finished.
        /// / Max: 30 characters
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="linkUri">
        /// Webpage URL. Called when the label displayed after the video is tapped.
        /// / Max: 1000 characters
        /// </param>
        /// <param name="label">
        /// Label.Displayed after the video is finished.
        /// / Max: 30 characters
        /// </param>
        public ExternalLink(string linkUri, string label)
        {
            LinkUri = linkUri;
            Label = label;
        }
    }
}
