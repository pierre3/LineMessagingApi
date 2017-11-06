namespace Line.Messaging
{
    /// <summary>
    /// When a control associated with this action is tapped, the string in the text field is sent as a message from the user.
    /// https://developers.line.me/en/docs/messaging-api/reference/#datetime-picker-action
    /// </summary>
    public class MessageTemplateAction: ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Message;

        /// <summary>
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Not applicable for rich menus.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Text sent when the action is performed
        /// Max: 300 characters
        /// </summary>
        public string Text { get; }

        public MessageTemplateAction(string label,string text)
        {
            Label = label;
            Text = text;
        }

        internal static MessageTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new MessageTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.text);
        }
    }
}
