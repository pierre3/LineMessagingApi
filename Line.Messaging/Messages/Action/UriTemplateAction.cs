using System;

namespace Line.Messaging
{
    /// <summary>
    /// When a control associated with this action is tapped, the URI specified in the uri field is opened.
    /// https://developers.line.me/en/docs/messaging-api/reference/#uri-action
    /// </summary>
    public class UriTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Uri;

        /// <summary>
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Optional for rich menus. Spoken when the accessibility feature is enabled on the client device. Max: 20 characters. Supported on LINE iOS version 8.2.0 and later.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// URI opened when the action is performed (Max: 1000 characters)
        /// Must start with http, https, or tel.
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label">
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Optional for rich menus. Spoken when the accessibility feature is enabled on the client device. Max: 20 characters. Supported on LINE iOS version 8.2.0 and later.
        /// </param>
        /// <param name="uri">
        /// URI opened when the action is performed (Max: 1000 characters)
        /// Must start with http, https, or tel.
        /// </param>
        public UriTemplateAction(string label, string uri)
        {
            Label = label?.Substring(0, Math.Min(label.Length, 20));
            Uri = uri;
        }

        internal static UriTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new UriTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.uri);
        }
    }
}
