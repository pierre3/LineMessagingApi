using System;

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

        /// <summary>
        /// Label for the action. Spoken when the accessibility feature is enabled on the client device. 
        /// Max: 50 characters
        /// Supported on LINE iOS version 8.2.0 and later.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="area">
        /// Defined tappable area
        /// </param>
        /// <param name="linkUri">
        /// Label for the action. Spoken when the accessibility feature is enabled on the client device. 
        /// Max: 50 characters
        /// Supported on LINE iOS version 8.2.0 and later.
        /// </param>
        /// <param name="label">
        /// Label for the action. Spoken when the accessibility feature is enabled on the client device. 
        /// Max: 50 characters
        /// Supported on LINE iOS version 8.2.0 and later.
        /// </param>
        public UriImagemapAction(ImagemapArea area, string linkUri, string label = null)
        {
            Area = area;
            LinkUri = linkUri;
            Label = label?.Substring(Math.Min(label.Length, 50));
        }
    }
}
