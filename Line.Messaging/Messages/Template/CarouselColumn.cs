using System.Collections.Generic;

namespace Line.Messaging
{
    public class CarouselColumn
    {
        public string ThumbnailImageUrl { get; }

        public string Title { get; }

        public string Text { get; }

        public IList<ITemplateAction> Actions { get; }

        public CarouselColumn(string thumbnailImageUrl, string title, string text, IList<ITemplateAction> actions)
        {
            ThumbnailImageUrl = thumbnailImageUrl;
            Title = title;
            Text = text;
            Actions = actions;
        }
    }
}
