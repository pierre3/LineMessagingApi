using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Template with multiple images which can be cycled like a carousel.
    /// https://developers.line.me/en/docs/messaging-api/reference/#image-carousel
    /// </summary>
    public class ImageCarouselTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Image_carousel;

        /// <summary>
        /// Array of columns
        /// Max: 10
        /// </summary>
        public IList<ImageCarouselColumn> Columns { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columns">
        /// Array of columns
        /// Max: 10
        /// </param>
        public ImageCarouselTemplate(IList<ImageCarouselColumn> columns = null)
        {
            Columns = columns ?? new List<ImageCarouselColumn>();
        }
    }
}
