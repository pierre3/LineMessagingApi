using System;

namespace Line.Messaging
{
    /// <summary>
    /// This action can be configured only with quick reply buttons. When a button associated with this action is tapped, the camera screen in the LINE app is opened.
    /// https://developers.line.me/en/reference/messaging-api/#camera-action
    /// </summary>
    public class CameraTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Camera;

        /// <summary>
        /// Label for the action
        /// Max: 20 characters
        /// </summary>
        public string Label { get; }
        
        public CameraTemplateAction(string label)
        {
            Label = label.Substring(0, Math.Min(label.Length, 20));
        }

        internal static CameraTemplateAction CreateFrom(dynamic dynamicObject)
        {
            return new CameraTemplateAction((string)dynamicObject?.label);
        }
    }
}
