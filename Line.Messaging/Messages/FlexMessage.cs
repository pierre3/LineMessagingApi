using System;
using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Template messages are messages with predefined layouts which you can customize. There are four types of templates available that can be used to interact with users through your bot.
    /// </summary>
    public class FlexMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Flex;

        /// <summary>
        /// These properties are used for the quick reply feature
        /// </summary>
        public QuickReply QuickReply { get; set; }

        /// <summary>
        /// Flex Message container object
        /// </summary>
        public IFlexContainer Container { get; set; }

        /// <summary>
        /// Alternative text.
        /// Max: 400 characters
        /// </summary>
        public string AltText { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="altText">
        /// Alternative text.
        /// Max: 400 characters
        ///</param>
        public FlexMessage(string altText)
        {
            AltText = altText.Substring(0, Math.Min(altText.Length, 400));
        }

        public static FlexMessage CreateBubbleMessage(string altText)
        {
            var message = new FlexMessage(altText);
            message.Container = new BubbleContainer();
            return message;
        }

        public static CarouselContainerFlexMessage CreateCarouselMessage(string altText)
        {
            return new CarouselContainerFlexMessage(altText)
            {
                Container = new CarouselContainer()
            };
        }
    }

    public class BubbleContainerFlexMessage : FlexMessage
    {
        public BubbleContainerFlexMessage AddBubbleContainer(BubbleContainer bubbleContainer)
        {
            Container = bubbleContainer ?? throw new ArgumentNullException(nameof(bubbleContainer));
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

    public class CarouselContainerFlexMessage : FlexMessage
    {
        private IList<BubbleContainer> contents;

        public CarouselContainerFlexMessage(string altText) : base(altText)
        {
            contents = (Container as CarouselContainer).Contents;
        }

        public CarouselContainerFlexMessage AddBubbleContainer(BubbleContainer bubbleContainer)
        {
            if (bubbleContainer == null) { throw new ArgumentNullException(nameof(bubbleContainer)); }
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
