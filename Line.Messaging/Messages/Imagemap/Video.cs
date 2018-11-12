namespace Line.Messaging
{
    public class Video
    {
        /// <summary>
        /// URL of the video file (Max: 1000 characters)
        /// HTTPS, mp4
        /// / Max: 1 minute
        /// / Max: 10 MB
        /// / Note: A very wide or tall video may be cropped when played in some environments.
        /// </summary>
        public string OriginalContentUrl { get; }

        /// <summary>
        /// URL of the preview image (Max: 1000 characters)
        /// HTTP, JPEG
        /// / Max: 240 x 240 pixels
        /// / Max: 1 MB
        /// </summary>
        public string PreviewImageUrl { get; }

        /// <summary>
        /// Imagemap Area
        /// </summary>
        public ImagemapArea Area { get; }

        /// <summary>
        /// Label. Displayed after the video is finished.
        /// And Webpage URL. Called when the label displayed after the video is tapped.
        /// </summary>
        public ExternalLink ExternalLink { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalContentUrl">
        /// URL of the video file (Max: 1000 characters)
        /// HTTPS, mp4
        /// / Max: 1 minute
        /// / Max: 10 MB
        /// / Note: A very wide or tall video may be cropped when played in some environments.
        /// </param>
        /// <param name="previewImageUrl">
        /// URL of the preview image (Max: 1000 characters)
        /// HTTP, JPEG
        /// / Max: 240 x 240 pixels
        /// / Max: 1 MB
        /// </param>
        /// <param name="area">
        /// Imagemap Area
        /// </param>
        /// <param name="externalLink">
        /// Label. Displayed after the video is finished.
        /// And Webpage URL. Called when the label displayed after the video is tapped.
        /// </param>
        public Video(string originalContentUrl, string previewImageUrl, ImagemapArea area, ExternalLink externalLink)
        {
            OriginalContentUrl = originalContentUrl;
            PreviewImageUrl = previewImageUrl;
            Area = area;
            ExternalLink = externalLink;
        }
    }
}
