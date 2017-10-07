using System.Collections.Generic;

namespace Line.Messaging
{
    public class ImageCarouselTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Image_carousel;

        public IList<ImageCarouselColumn> Columns { get; }

        public ImageCarouselTemplate(IList<ImageCarouselColumn> columns)
        {
            Columns = columns;
        }
    }
}
