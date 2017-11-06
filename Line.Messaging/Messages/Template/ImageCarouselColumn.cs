namespace Line.Messaging
{
    /// <summary>
    /// Column object for image carousel
    /// </summary>
    public class ImageCarouselColumn
    {
        /// <summary>
        /// Image URL (Max: 1000 characters)
        /// HTTPS
        /// JPEG or PNG
        /// Aspect ratio: 1:1
        /// Max width: 1024px
        /// Max: 1 MB
        /// </summary>
        public string ImageUrl { get; }

        /// <summary>
        /// Action when image is tapped
        /// </summary>
        public ITemplateAction Action { get; }

        public ImageCarouselColumn(string imageUrl, ITemplateAction action)
        {
            ImageUrl = imageUrl;
            Action = action;
        }
    }
}
