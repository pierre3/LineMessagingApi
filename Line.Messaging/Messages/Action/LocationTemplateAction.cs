using System;

namespace Line.Messaging
{
    /// <summary>
    /// This action can be configured only with quick reply buttons. When a button associated with this action is tapped, the location screen in the LINE app is opened.
    /// https://developers.line.me/en/reference/messaging-api/#location-action
    /// </summary>
    public class LocationTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Location;

        /// <summary>
        /// Label for the action
        /// Max: 20 characters
        /// </summary>
        public string Label { get; }
        
        public LocationTemplateAction(string label)
        {
            Label = label.Substring(0, Math.Min(label.Length, 20));
        }

        internal static LocationTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new LocationTemplateAction((string)dynamicObject?.label);
        }
    }
}
