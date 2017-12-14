using System;
using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Template message with an image, title, text, and multiple action buttons.
    /// https://developers.line.me/en/docs/messaging-api/reference/#buttons
    /// </summary>
    public class ButtonsTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Buttons;

        /// <summary>
        /// Image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG or PNG
        /// Aspect ratio: 1:1.51
        /// Max width: 1024px
        /// Max: 1 MB
        /// </summary>
        public string ThumbnailImageUrl { get; }

        /// <summary>
        /// Aspect ratio of the image. Specify one of the following values:
        /// rectangle: 1.51:1 
        /// square: 1:1 
        /// The default value is rectangle.
        /// </summary>
        public ImageAspectRatioType ImageAspectRatio { get; }

        /// <summary>
        /// Size of the image. Specify one of the following values:
        /// cover: The image fills the entire image area.Parts of the image that do not fit in the area are not displayed.
        /// contain: The entire image is displayed in the image area.A background is displayed in the unused areas to the left and right of vertical images and in the areas above and below horizontal images.
        /// The default value is cover.
        /// </summary>
        public ImageSizeType ImageSize { get; }

        /// <summary>
        /// Background color of image. Specify a RGB color value. The default value is #FFFFFF (white).
        /// </summary>
        public string ImageBackgroundColor { get; }

        /// <summary>
        /// Title
        /// Max: 40 characters
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Message text
        /// Max: 160 characters(no image or title)
        /// Max: 60 characters(message with an image or title)
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Action when tapped
        /// Max: 4
        /// </summary>
        public IList<ITemplateAction> Actions { get; }

        public ButtonsTemplate(string text, string thumbnailImageUrl = null, string title = null, IList<ITemplateAction> actions = null,
             ImageAspectRatioType imageAspectRatio = ImageAspectRatioType.Rectangle, ImageSizeType imageSize = ImageSizeType.Cover, string imageBackgroundColor = "#FFFFFF")
        {
            ThumbnailImageUrl = thumbnailImageUrl;
            Title = title.Substring(0, Math.Min(title.Length, 40));
            Text = (string.IsNullOrEmpty(thumbnailImageUrl) || string.IsNullOrEmpty(title)) ? text.Substring(0, Math.Min(text.Length, 160)) : text.Substring(0, Math.Min(text.Length, 60));
            Actions = actions ?? new List<ITemplateAction>();
            ImageAspectRatio = imageAspectRatio;
            ImageSize = imageSize;
            ImageBackgroundColor = imageBackgroundColor;
        }
    }
}
