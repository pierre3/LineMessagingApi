using System;
using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Column object for carousel.
    /// Because of the height limitation for carousel template messages, the lower part of the text display area will get cut off if the height limitation is exceeded. For this reason, depending on the character width, the message text may not be fully displayed even when it is within the character limits.
    /// Keep the number of actions consistent for all columns.If you use an image or title for a column, make sure to do the same for all other columns.
    /// </summary>
    public class CarouselColumn
    {
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
        /// Max: 120 characters(no image or title)
        /// Max: 60 characters(message with an image or title)
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Action when tapped
        /// Max: 3
        /// </summary>
        public IList<ITemplateAction> Actions { get; }

        /// <summary>
        /// Action when image is tapped; set for the entire image, title, and text area
        /// </summary>
        public ITemplateAction DefaultAction { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">
        /// Message text
        /// Max: 120 characters(no image or title)
        /// Max: 60 characters(message with an image or title)
        /// </param>
        /// <param name="thumbnailImageUrl">
        /// Image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG or PNG
        /// Aspect ratio: 1:1.51
        /// Max width: 1024px
        /// Max: 1 MB
        /// </param>
        /// <param name="title">
        /// Title
        /// Max: 40 characters
        /// </param>
        /// <param name="actions">
        /// Action when tapped
        /// Max: 3
        /// </param>
        /// <param name="imageBackgroundColor">
        /// Background color of image. Specify a RGB color value. The default value is #FFFFFF (white).
        /// </param>
        /// <param name="defaultAction">
        /// Action when image is tapped; set for the entire image, title, and text area
        /// </param>
        public CarouselColumn(string text, string thumbnailImageUrl = null, string title = null,
            IList<ITemplateAction> actions = null, string imageBackgroundColor = "#FFFFFF",
            ITemplateAction defaultAction = null)
        {
            ThumbnailImageUrl = thumbnailImageUrl;
            Title = title?.Substring(0, Math.Min(title.Length, 40));
            Text = (string.IsNullOrEmpty(thumbnailImageUrl) && string.IsNullOrEmpty(title))
                ? text.Substring(0, Math.Min(text.Length, 120)) : text.Substring(0, Math.Min(text.Length, 60));
            Actions = actions ?? new List<ITemplateAction>();
            ImageBackgroundColor = imageBackgroundColor;
            DefaultAction = defaultAction;
        }
    }
}
