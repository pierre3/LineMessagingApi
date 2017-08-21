using System.Collections.Generic;

namespace Line.Messaging
{
    public class CarouselTemplate : ITemplate
    {
        public TemplateType Type { get; } = TemplateType.Carousel;

        public IList<CarouselColumn> Columns { get; }

        public CarouselTemplate(IList<CarouselColumn> columns)
        {
            Columns = columns;
        }
    }
}
