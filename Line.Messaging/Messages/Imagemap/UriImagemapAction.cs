namespace Line.Messaging
{
    /// <summary>
    /// Object which specifies the actions and tappable regions of an imagemap.
    /// When a region is tapped, the user is redirected to the URI specified in uri.
    /// https://developers.line.me/en/docs/messaging-api/reference/#imagemap-action-objects
    /// </summary>
    public class UriImagemapAction : IImagemapAction
    {
        public ImagemapActionType Type { get; } = ImagemapActionType.Uri;

        /// <summary>
        /// Defined tappable area
        /// </summary>
        public ImagemapArea Area { get; }

        /// <summary>
        /// Webpage URL
        /// Max: 1000 characters
        /// </summary>
        public string LinkUri { get; }

        public UriImagemapAction(ImagemapArea area, string linkUri)
        {
            Area = area;
            LinkUri = linkUri;
        }
    }    
}
