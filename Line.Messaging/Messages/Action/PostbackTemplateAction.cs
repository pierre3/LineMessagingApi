using System;

namespace Line.Messaging
{
    /// <summary>
    /// When a control associated with this action is tapped, a postback event is returned via webhook with the specified string in the data field.
    /// If you have included the text field, the string in the text field is sent as a message from the user.
    /// https://developers.line.me/en/docs/messaging-api/reference/#postback-action
    /// </summary>
    public class PostbackTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Postback;

        /// <summary>
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Optional for rich menus. Spoken when the accessibility feature is enabled on the client device. Max: 20 characters. Supported on LINE iOS version 8.2.0 and later.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// Deprecated. Text displayed in the chat as a message sent by the user when the action is performed. Returned from the server through a webhook.
        /// Max: 300 characters
        /// The displayText and text fields cannot both be used at the same time.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Text displayed in the chat as a message sent by the user when the action is performed.
        /// Max: 300 characters
        /// The displayText and text fields cannot both be used at the same time.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label">
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Optional for rich menus. Spoken when the accessibility feature is enabled on the client device. Max: 20 characters. Supported on LINE iOS version 8.2.0 and later.
        /// </param>
        /// <param name="data">
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </param>
        /// <param name="text">
        /// Text displayed in the chat as a message sent by the user when the action is performed.
        /// And only when <paramref name="useDisplayText"/> is false, returned from the server through a webhook.
        /// <para>Max: 300 characters</para>
        /// </param>
        /// <param name="useDisplayText">
        /// If set to true, <paramref name="text"/> parameter is set to DisplayText property.
        /// (Deprecated) If set to false, <paramref name="text"/> parameter is set to Text property. However text property is deprecated.
        /// </param>
        public PostbackTemplateAction(string label, string data, string text = null, bool useDisplayText = true)
        {
            Data = data.Substring(0, Math.Min(data.Length, 300));
            Label = label?.Substring(0, Math.Min(label.Length, 20));

            if (useDisplayText)
            {
                DisplayText = text?.Substring(0, Math.Min(text.Length, 300));
            }
            else
            {
                Text = text?.Substring(0, Math.Min(text.Length, 300));
            }

        }

        internal static PostbackTemplateAction CreateFrom(dynamic dynamicObject)
        {
            bool useDisplayText = true;
            string text = dynamicObject?.displayText;
            if (text == null)
            {
                text = dynamicObject?.text;
                useDisplayText = false;
            }
            return new PostbackTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.data, text, useDisplayText);
        }
    }
}
