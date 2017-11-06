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

        public ButtonsTemplate(string thumbnailImageUrl, string title, string text, IList<ITemplateAction> actions)
        {
            ThumbnailImageUrl = thumbnailImageUrl;
            Title = title;
            Text = text;
            Actions = actions;
        }
    }
}
