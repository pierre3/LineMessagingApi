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
        public IFlexContainer Contents { get; set; }

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

        public static BubbleContainerFlexMessage CreateBubbleMessage(string altText)
        {
            return new BubbleContainerFlexMessage(altText)
            {
                Contents = new BubbleContainer()
            };
        }

        public static CarouselContainerFlexMessage CreateCarouselMessage(string altText)
        {
            return new CarouselContainerFlexMessage(altText)
            {
                Contents = new CarouselContainer()
            };
        }
    }

}
