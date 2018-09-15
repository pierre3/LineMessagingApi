using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class BubbleContainerFlexMessage : FlexMessage
    {
        public BubbleContainerFlexMessage AddBubbleContainer(BubbleContainer bubbleContainer)
        {
            Contents = bubbleContainer ?? throw new ArgumentNullException(nameof(bubbleContainer));
            return this;
        }
        public BubbleContainerFlexMessage(string altText) : base(altText)
        {

        }

        public BubbleContainerFlexMessage AddQuickReply(QuickReply quickReply)
        {
            QuickReply = quickReply;
            return this;
        }
    }
}
