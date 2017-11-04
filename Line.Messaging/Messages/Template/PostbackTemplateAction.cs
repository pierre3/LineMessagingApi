namespace Line.Messaging
{
    public class PostbackTemplateAction: ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Postback;

        public string Label { get; }

        public string Data { get; }

        public string Text { get; }

        public PostbackTemplateAction(string label,string data,string text)
        {
            Data = data;
            Label = label;
            Text = text;
        }

        public static PostbackTemplateAction CreateFrom(dynamic dynamicObj)
        {
            return new PostbackTemplateAction((string)dynamicObj?.label, (string)dynamicObj?.data, (string)dynamicObj?.text);
        }
    }
}
