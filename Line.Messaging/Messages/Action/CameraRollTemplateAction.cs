using System;

namespace Line.Messaging
{
    /// <summary>
    /// This action can be configured only with quick reply buttons. When a button associated with this action is tapped, the camera roll screen in the LINE app is opened.
    /// https://developers.line.me/en/reference/messaging-api/#camera-roll-action
    /// </summary>
    public class CameraRollTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.CameraRoll;

        /// <summary>
        /// Label for the action
        /// Max: 20 characters
        /// </summary>
        public string Label { get; }
        
        public CameraRollTemplateAction(string label)
        {
            Label = label.Substring(0, Math.Min(label.Length, 20));
        }

        internal static CameraRollTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new CameraRollTemplateAction((string)dynamicObject?.label);
        }
    }
}
