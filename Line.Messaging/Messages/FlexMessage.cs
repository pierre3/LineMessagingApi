using System;

namespace Line.Messaging
{
    /// <summary>
    /// Template messages are messages with predefined layouts which you can customize. There are four types of templates available that can be used to interact with users through your bot.
    /// </summary>
    public class FlexMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Flex;

        /// <summary>
        /// Flex Message container object
        /// </summary>
        public IFlexContainer Container { get; }

        /// <summary>
        /// Alternative text.
        /// Max: 400 characters
        /// </summary>
        public string AltText { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="altText">
        /// Alternative text.
        /// Max: 400 characters
        ///</param>
        /// <param name="container">
        /// Flex Message container object
        /// </param>
        public FlexMessage(string altText, IFlexContainer container)
        {
            AltText = altText.Substring(0, Math.Min(altText.Length, 400));
            Container = container;
        }
    }
}
