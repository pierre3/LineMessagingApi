using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Rich menu  Area
    /// https://developers.line.me/en/docs/messaging-api/reference/#area-object
    /// </summary>
    public class ActionArea
    {
        /// <summary>
        /// Object describing the boundaries of the area in pixels. See bounds object.
        /// </summary>
        public ImagemapArea Bounds { get; set; }

        /// <summary>
        /// Action performed when the area is tapped. See action objects. Note: The label field is not supported for actions in rich menus.
        /// </summary>
        public ITemplateAction Action { get; set; }

        internal static ActionArea CreateFrom(dynamic dynamicObject)
        {
            return new ActionArea()
            {
                Bounds = new ImagemapArea(
                    (int)(dynamicObject?.bounds?.x ?? 0),
                    (int)(dynamicObject?.bounds?.y ?? 0),
                    (int)(dynamicObject?.bounds?.width ?? 0),
                    (int)(dynamicObject?.bounds?.height ?? 0)),
                Action = ParseTemplateAction(dynamicObject?.action)
            };
        }

        public static ITemplateAction ParseTemplateAction(dynamic dynamicObject)
        {
            var type = (TemplateActionType)System.Enum.Parse(typeof(TemplateActionType), (string)dynamicObject?.type, true);
            switch (type)
            {
                case TemplateActionType.Message:
                    return MessageTemplateAction.CreateFrom(dynamicObject);
                case TemplateActionType.Uri:
                    return UriTemplateAction.CreateFrom(dynamicObject);
                case TemplateActionType.Postback:
                    return PostbackTemplateAction.CreateFrom(dynamicObject);
                case TemplateActionType.Datetimepicker:
                    return DateTimePickerTemplateAction.CreateFrom(dynamicObject);
                default:
                    return null;
            }
        }
    }
}
