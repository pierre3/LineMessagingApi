using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Template message with multiple columns which can be cycled like a carousel.
    /// https://developers.line.me/en/docs/messaging-api/reference/#carousel
    /// </summary>
    public class CarouselTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Carousel;

        /// <summary>
        /// Array of columns
        /// Max: 10
        /// </summary>
        public IList<CarouselColumn> Columns { get; }

        public CarouselTemplate(IList<CarouselColumn> columns)
        {
            Columns = columns;
        }
    }
}
