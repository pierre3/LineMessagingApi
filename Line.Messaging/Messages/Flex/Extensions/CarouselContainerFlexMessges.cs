using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class CarouselContainerFlexMessage : FlexMessage
    {
        public CarouselContainerFlexMessage(string altText) : base(altText)
        {
        }

        public CarouselContainerFlexMessage AddBubbleContainer(BubbleContainer bubbleContainer)
        {
            if (bubbleContainer == null) { throw new ArgumentNullException(nameof(bubbleContainer)); }
            var contents = (Contents as CarouselContainer).Contents;
            contents.Add(bubbleContainer);
            return this;
        }

        public CarouselContainerFlexMessage AddQuickReply(QuickReply quickReply)
        {
            QuickReply = quickReply;
            return this;
        }
    }
}
