using System;

namespace Line.Messaging
{
    /// <summary>
    /// Template messages are messages with predefined layouts which you can customize. There are four types of templates available that can be used to interact with users through your bot.
    /// </summary>
    public class TemplateMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Template;

        /// <summary>
        /// A Buttons, Confirm, Carousel, or Image Carousel object.
        /// </summary>
        public ITemplate Template { get; }

        /// <summary>
        /// Alternative text.
        /// Max: 400 characters
        /// </summary>
        public string AltText { get; }

        public TemplateMessage(string altText, ITemplate template)
        {
            AltText = altText.Substring(0, Math.Min(altText.Length, 400)); 
            Template = template;
        }
    }
}
