using System;

namespace Line.Messaging
{
    /// <summary>
    /// When a control associated with this action is tapped, the string in the text field is sent as a message from the user.
    /// https://developers.line.me/en/docs/messaging-api/reference/#datetime-picker-action
    /// </summary>
    public class MessageTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Message;

        /// <summary>
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Optional for rich menus. Spoken when the accessibility feature is enabled on the client device. Max: 20 characters. Supported on LINE iOS version 8.2.0 and later.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Text sent when the action is performed
        /// Max: 300 characters
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label">
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Optional for rich menus. Spoken when the accessibility feature is enabled on the client device. Max: 20 characters. Supported on LINE iOS version 8.2.0 and later.
        /// </param>
        /// <param name="text">
        /// Text sent when the action is performed
        /// Max: 300 characters
        /// </param>
        public MessageTemplateAction(string label, string text)
        {
            Label = label?.Substring(0, Math.Min(label.Length, 20));
            Text = text.Substring(0, Math.Min(text.Length, 300));
        }

        internal static MessageTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new MessageTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.text);
        }
    }
}
