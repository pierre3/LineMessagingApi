using System;

namespace Line.Messaging
{
    /// <summary>
    /// When a control associated with this action is tapped, a postback event is returned via webhook with the date and time selected by the user from the date and time selection dialog.
    /// https://developers.line.me/en/docs/messaging-api/reference/#datetime-picker-action
    /// </summary>
    public class DateTimePickerTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Datetimepicker;

        /// <summary>
        /// Label for the action
        /// Required for templates other than image carousel.Max: 20 characters
        /// Optional for image carousel templates.Max: 12 characters.
        /// Not applicable for rich menus.
        /// </summary>
        public string Label { get; protected set; }

        /// <summary>
        /// String returned via webhook in the postback.data property of the postback event
        /// Max: 300 characters
        /// </summary>
        public string Data { get; protected set; }

        /// <summary>
        /// Action mode
        /// date: Pick date
        /// time: Pick time
        /// datetime: Pick date and time
        /// </summary>
        public DateTimePickerMode Mode { get; protected set; }

        /// <summary>
        /// Initial value of date or time
        /// </summary>
        public string Initial { get; protected set; }

        /// <summary>
        /// Largest date or time value that can be selected.
        /// Must be greater than the min value.
        /// </summary>
        public string Max { get; protected set; }

        /// <summary>
        /// Smallest date or time value that can be selected.
        /// Must be less than the max value.
        /// </summary>
        public string Min { get; protected set; }

        public DateTimePickerTemplateAction(string label, string data, DateTimePickerMode mode, string initial = null, string min = null, string max = null)
        {
            Initialize(label, data, mode, initial, min, max);
        }

        public DateTimePickerTemplateAction(string label, string data, DateTimePickerMode mode, DateTime? initial = null, DateTime? min = null, DateTime? max = null)
        {
            var format = GetDateTimeFormat(mode);
            Initialize(label, data, mode, 
                initial == null ? null : ((DateTime)initial).ToString(format), 
                min == null ? null : ((DateTime)min).ToString(format), 
                max == null ? null : ((DateTime)max).ToString(format));
        }

        internal void Initialize(string label, string data, DateTimePickerMode mode, string initial, string min, string max)
        {
            Label = label;
            Data = data;
            Mode = mode;
            Initial = initial;
            Min = min;
            Max = max;
        }

        internal static string GetDateTimeFormat(DateTimePickerMode mode)
        {
            var format = "";
            switch (mode)
            {
                case DateTimePickerMode.Date:
                    format = "yyyy-MM-dd";
                    break;
                case DateTimePickerMode.Time:
                    format = "HH:mm";
                    break;
                case DateTimePickerMode.Datetime:
                    format = "yyyy-MM-ddTHH:mm";
                    break;
            }
            return format;
        }

        internal static DateTimePickerTemplateAction CreateFrom(dynamic dynamicObject)
        {
            var mode = (DateTimePickerMode)Enum.Parse(typeof(DateTimePickerMode), dynamicObject?.mode);
            var format = GetDateTimeFormat(mode);
            var initial = DateTime.ParseExact(dynamicObject?.initial, format, null);
            var min = DateTime.ParseExact(dynamicObject?.min, format, null);
            var max = DateTime.ParseExact(dynamicObject?.max, format, null);
            return new DateTimePickerTemplateAction((string)dynamicObject?.label, (string)dynamicObject?.data, mode, initial, min, max);
        }
    }
}
