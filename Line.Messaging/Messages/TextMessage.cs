using System;

namespace Line.Messaging
{
    /// <summary>
    /// Text
    /// https://developers.line.me/en/docs/messaging-api/reference/#text
    /// </summary>
    public class TextMessage : ISendMessage
    {        
        public MessageType Type { get; } = MessageType.Text;

        /// <summary>
        /// These properties are used for the quick reply feature
        /// </summary>
        public QuickReply QuickReply { get; set; }

        /// <summary>
        /// Message text
        /// Max: 2000 characters
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">
        /// Message text
        /// Max: 2000 characters
        /// </param>
        /// <param name="quickReply">
        /// QuickReply
        /// </param>
        public TextMessage(string text, QuickReply quickReply = null)
        {
            Text = text.Substring(0, Math.Min(text.Length, 2000));
            QuickReply = quickReply;
        }
    }
}
