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

        public MessageImagemapAction(ImagemapArea area, string text)
        {
            Area = area;
            Text = text.Substring(0, Math.Min(text.Length, 400));
        }
    }
}
