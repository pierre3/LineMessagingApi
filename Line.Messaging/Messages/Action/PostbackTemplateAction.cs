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
        /// Not applicable for rich menus.
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
        /// Not applicable for rich menus.
        /// </param>
        /// <param name="data">
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </param>
        /// <param name="text">
        /// Deprecated. Text displayed in the chat as a message sent by the user when the action is performed.
        /// <para></para>
        /// </param>
        /// <param name="displayText">
        /// Text displayed in the chat as a message sent by the user when the action is performed.
        /// Max: 300 characters
        /// The displayText and text fields cannot both be used at the same time.
        /// </param>
        [Obsolete("Use the static method \"Create\" .")]
        public PostbackTemplateAction(string label, string data, string text = null, string displayText=null)
        {
            Data = data.Substring(0, Math.Min(data.Length, 300));
            Label = label.Substring(0, Math.Min(label.Length, 20));

            if(displayText!=null)
            {
                DisplayText = text.Substring(0, Math.Min(text.Length, 300));
            }
            else if(text!=null)
            {
                Text = text.Substring(0, Math.Min(text.Length, 300));
            }
            
        }
#pragma warning disable CS0618
        /// <summary>
        /// Create a new instance 
        /// </summary>
        /// <param name="label">
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Not applicable for rich menus.
        /// </param>
        /// <param name="data">
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </param>
        /// <param name="displayText">
        /// Text displayed in the chat as a message sent by the user when the action is performed.
        /// Max: 300 characters
        /// The displayText and text fields cannot both be used at the same time.
        /// </param>
        /// <returns>Instance of PostbackTemplateAction</returns>
        public static PostbackTemplateAction Create(string label,string data, string displayText=null)
        {
            return new PostbackTemplateAction(label, data, null, displayText);
        }

        internal static PostbackTemplateAction CreateFrom(dynamic dynamicObject)
        {
            string displayText = dynamicObject?.displayText;
            string text = null; 
            if (displayText == null)
            {
                text = dynamicObject?.text;
            }
            return new PostbackTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.data, text, displayText);
        }
#pragma warning restore
    }
}
