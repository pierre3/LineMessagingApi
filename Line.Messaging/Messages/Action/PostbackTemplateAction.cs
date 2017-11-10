namespace Line.Messaging
{
    /// <summary>
    /// When a control associated with this action is tapped, a postback event is returned via webhook with the specified string in the data field.
    /// If you have included the text field, the string in the text field is sent as a message from the user.
    /// https://developers.line.me/en/docs/messaging-api/reference/#postback-action
    /// </summary>
    public class PostbackTemplateAction: ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Postback;

        /// <summary>
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Not applicable for rich menus.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// Text sent when the action is performed
        /// Max: 300 characters
        /// </summary>
        public string Text { get; }

        public PostbackTemplateAction(string label, string data, string text = null)
        {
            Data = data;
            Label = label;
            Text = text;
        }

        internal static PostbackTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new PostbackTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.data, (string)dynamicObject?.text);
        }
    }
}
