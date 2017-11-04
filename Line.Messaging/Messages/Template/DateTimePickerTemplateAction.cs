using System;

namespace Line.Messaging
{
    public class DateTimePickerTemplateAction : ITemplateAction
    {
        public TemplateActionType Type { get; } = TemplateActionType.Datetimepicker;

        public string Label { get; protected set; }

        public string Data { get; protected set; }

        public DateTimePickerMode Mode { get; protected set; }

        public string Initial { get; protected set; }

        public string Min { get; protected set; }

        public string Max { get; protected set; }

        public DateTimePickerTemplateAction(string label, string data, DateTimePickerMode mode, string initial, string min, string max)
        {
            Initialize(label, data, mode, initial, min, max);
        }

        public DateTimePickerTemplateAction(string label, string data, DateTimePickerMode mode, DateTime initial, DateTime min, DateTime max)
        {
            var format = GetDateTimeFormat(mode);
            Initialize(label, data, mode, initial.ToString(format), min.ToString(format), max.ToString(format));
        }

        protected void Initialize(string label, string data, DateTimePickerMode mode, string initial, string min, string max)
        {
            Label = label;
            Data = data;
            Mode = mode;
            Initial = initial;
            Min = min;
            Max = max;
        }

        protected static string GetDateTimeFormat(DateTimePickerMode mode)
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

        public static DateTimePickerTemplateAction CreateFrom(dynamic dynamicObj)
        {
            var mode = (DateTimePickerMode)Enum.Parse(typeof(DateTimePickerMode), dynamicObj?.mode);
            var format = GetDateTimeFormat(mode);
            var initial = DateTime.ParseExact(dynamicObj?.initial, format, null);
            var min = DateTime.ParseExact(dynamicObj?.min, format, null);
            var max = DateTime.ParseExact(dynamicObj?.max, format, null);
            return new DateTimePickerTemplateAction((string)dynamicObj?.label, (string)dynamicObj?.data, mode, initial, min, max);
        }
    }
}
