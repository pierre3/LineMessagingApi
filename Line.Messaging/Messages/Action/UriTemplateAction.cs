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
        /// URI opened on LINE for macOS and Windows when the action is performed (Max: 1000 characters) If the altUri.desktop property is set, 
        /// the uri property is ignored on LINE for macOS and Windows.<para>
        /// The available schemes are http, https, line, and tel.For more information about the LINE URL scheme, see Using the LINE URL scheme. 
        /// This property is supported on the following version of LINE.
        /// LINE 5.12.0 or later for macOS and Windows</para>
        /// Note: The altUri.desktop property is supported only when you set URI actions in Flex Messages.
        /// </summary>
        public AltUri AltUri { get; }

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
        /// <param name="altUri">
        /// URI opened on LINE for macOS and Windows when the action is performed (Max: 1000 characters) If the altUri.desktop property is set, 
        /// the uri property is ignored on LINE for macOS and Windows.<para>
        /// The available schemes are http, https, line, and tel.For more information about the LINE URL scheme, see Using the LINE URL scheme. 
        /// This property is supported on the following version of LINE.
        /// LINE 5.12.0 or later for macOS and Windows</para>
        /// Note: The altUri.desktop property is supported only when you set URI actions in Flex Messages.
        /// </param>
        public UriTemplateAction(string label, string uri, AltUri altUri = null)
        {
            Label = label?.Substring(0, Math.Min(label.Length, 20));
            Uri = uri;
            AltUri = altUri;
        }

        internal static UriTemplateAction CreateFrom(dynamic dynamicObject)
        {
            var desktopUri = (string)dynamicObject?.altUri?.desktop;
            var altUri = (desktopUri == null) ? null : new AltUri(desktopUri);
            return new UriTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.uri, altUri);
        }
    }
}
