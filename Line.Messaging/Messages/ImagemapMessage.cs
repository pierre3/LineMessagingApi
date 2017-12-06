using System;
using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Imagemaps are images with one or more links. You can assign one link for the entire image or multiple links which correspond to different regions of the image.
    /// https://developers.line.me/en/docs/messaging-api/reference/#imagemap-message
    /// </summary>
    public class ImagemapMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Imagemap;

        /// <summary>
        /// Base URL of image (Max: 1000 characters)
        /// HTTPS
        /// </summary>
        public string BaseUrl { get; }

        /// <summary>
        /// Alternative text
        /// Max: 400 characters
        /// </summary>
        public string AltText { get; }

        /// <summary>
        /// Width of base image (set to 1040px）
        /// Height of base image（set to the height that corresponds to a width of 1040px）
        /// </summary>
        public ImagemapSize BaseSize { get; }

        /// <summary>
        /// Action when tapped.
        /// Max: 50
        /// </summary>
        public IList<IImagemapAction> Actions { get; }

        public ImagemapMessage(string baseUrl, string altText, ImagemapSize baseSize, IList<IImagemapAction> actions)
        {
            BaseUrl = baseUrl;
            AltText = altText.Substring(0, Math.Min(altText.Length, 400)); ;
            BaseSize = baseSize;
            Actions = actions;
        }
    }
}
