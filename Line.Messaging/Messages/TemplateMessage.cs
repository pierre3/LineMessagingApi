namespace Line.Messaging
{
    public class TemplateMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Template;

        public ITemplate Template { get; }

        public string AltText { get; }

        public TemplateMessage(string altText, ITemplate template)
        {
            AltText = altText;
            Template = template;
        }
    }

}
