namespace Line.Messaging
{
    public interface ITemplateAction
    {
        TemplateActionType Type { get; }
        string Label { get; }
    }
}
