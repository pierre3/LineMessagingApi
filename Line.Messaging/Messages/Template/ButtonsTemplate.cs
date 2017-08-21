using System.Collections.Generic;

namespace Line.Messaging
{
    public class ButtonsTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Buttons;

        public string ThumbnailImageUrl { get; }

        public string Title { get; }

        public string Text { get; }

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
