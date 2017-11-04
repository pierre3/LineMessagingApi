using System.Collections.Generic;

namespace Line.Messaging
{
    public class ActionArea
    {
        public ImagemapArea Bounds { get; set; }
        public ITemplateAction Action { get; set; }

        public static ActionArea CreateFrom(dynamic dynamicObj)
        {
            return new ActionArea()
            {
                Bounds = new ImagemapArea(
                    (int)(dynamicObj?.bounds?.x ?? 0),
                    (int)(dynamicObj?.bounds.y ?? 0),
                    (int)(dynamicObj?.bounds?.width ?? 0),
                    (int)(dynamicObj?.bounds?.height ?? 0)),
                Action = ParseTemplateAction(dynamicObj?.action)
            };
        }

        public static ITemplateAction ParseTemplateAction(dynamic dynamicObj)
        {
            var type = (TemplateActionType)System.Enum.Parse(typeof(TemplateActionType), (string)dynamicObj?.type);
            switch (type)
            {
                case TemplateActionType.Message:
                    return MessageTemplateAction.CreateFrom(dynamicObj);
                case TemplateActionType.Uri:
                    return UriTemplateAction.CreateFrom(dynamicObj);
                case TemplateActionType.Postback:
                    return PostbackTemplateAction.CreateFrom(dynamicObj);
                case TemplateActionType.Datetimepicker:
                    return DateTimePickerTemplateAction.CreateFrom(dynamicObj);
                default:
                    return null;
            }
        }
    }
}
