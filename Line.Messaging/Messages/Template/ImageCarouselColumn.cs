namespace Line.Messaging
{
    public class ImageCarouselColumn
    {
        public string ImageUrl { get; }

        public ITemplateAction Action { get; }

        public ImageCarouselColumn(string imageUrl, ITemplateAction action)
        {
            ImageUrl = imageUrl;
            Action = action;
        }
    }
}
