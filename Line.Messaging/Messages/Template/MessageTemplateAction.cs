namespace Line.Messaging
{
    public class MessageTemplateAction: ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Message;

        public string Label { get; }

        public string Text { get; }

        public MessageTemplateAction(string label,string text)
        {
            Label = label;
            Text = text;
        }

        public static MessageTemplateAction CreateFrom(dynamic dynamicObj)
        {
            return new MessageTemplateAction((string)dynamicObj?.label, (string)dynamicObj?.text);
        }
    }
}
