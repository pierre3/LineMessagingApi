namespace Line.Messaging
{
    public class UriTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Uri;

        public string Label { get; }

        public string Uri { get; }

        public UriTemplateAction(string label, string uri)
        {
            Label = label;
            Uri = uri;
        }
    }
}
