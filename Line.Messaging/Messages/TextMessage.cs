namespace Line.Messaging
{
    public class TextMessage : ISendMessage
    {
        
        public MessageType Type { get; } = MessageType.Text;

        public string Text { get; set; }

        public TextMessage(string text)
        {
            Text = text;
        }
    }
}
