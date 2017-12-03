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

        public CarouselTemplate(IList<CarouselColumn> columns = null,
            ImageAspectRatioType imageAspectRatio = ImageAspectRatioType.Rectangle, ImageSizeType imageSize = ImageSizeType.Cover)
        {
            Columns = columns ?? new List<CarouselColumn>();
            ImageAspectRatio = imageAspectRatio;
            ImageSize = imageSize;
        }
    }
}
