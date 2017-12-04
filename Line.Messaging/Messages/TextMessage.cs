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
        /// Message text
        /// Max: 2000 characters
        /// </summary>
        public string Text { get; set; }

        public TextMessage(string text)
        {
            Text = text.Substring(0, Math.Min(text.Length, 2000));
        }
    }
}
