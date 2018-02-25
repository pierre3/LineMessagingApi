using System;

namespace Line.Messaging
{
    /// <summary>
    /// Object which specifies the actions and tappable regions of an imagemap.
    /// When a region is tapped, the message specified in message is sent.
    /// https://developers.line.me/en/docs/messaging-api/reference/#imagemap-action-objects
    /// </summary>
    public class MessageImagemapAction : IImagemapAction
    {
        public ImagemapActionType Type { get; } = ImagemapActionType.Message;

        /// <summary>
        /// Defined tappable area
        /// </summary>
        public ImagemapArea Area { get; }

        /// <summary>
        /// Message to send
        /// Max: 400 characters
        /// </summary>
        public string Text { get; }

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
        /// <param name="text">
        /// Message to send
        /// Max: 400 characters
        /// </param>
        /// <param name="label">
        /// Label for the action. Spoken when the accessibility feature is enabled on the client device. 
        /// Max: 50 characters
        /// Supported on LINE iOS version 8.2.0 and later.
        /// </param>
        public MessageImagemapAction(ImagemapArea area, string text, string label = null)
        {
            Area = area;
            Text = text.Substring(0, Math.Min(text.Length, 400));
            Label = label?.Substring(0, Math.Min(label.Length, 50));
        }
    }
}
