using System;
using System.Text.RegularExpressions;

namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Object with the date and time selected by a user through a datetime picker action. The full-date, time-hour, and time-minute formats follow the RFC3339 protocol.
    /// </summary>
    public class PostbackParams
    {
        /// <summary>
        /// Date selected by user. Only included in the date mode. Format: full-date
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// Time selected by the user. Only included in the time mode. Format: time-hour ":" time-minute
        /// </summary>
        public string Time { get; }

        /// <summary>
        /// Date and time selected by the user. Only included in the datetime mode. Format: full-date "T" time-hour ":" time-minute
        /// </summary>
        public string DateTime { get; }

        public PostbackParams(string date, string time, string datetime)
        {
            if (date != null && !Regex.Match(date, @"^(\d{4})-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$").Success)
            {
                throw new ArgumentException($"Date format must be \"yyyy-MM-dd\".", nameof(date));
            }
            if (time != null && !Regex.Match(time, @"^([01][0-9]|2[0-3]):([0-5][0-9])$").Success)
            {
                throw new ArgumentException($"Time format must be \"HH:mm\".", nameof(time));
            }
            if (datetime != null && !Regex.Match(datetime, @"^(\d{4})-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])T([01][0-9]|2[0-3]):([0-5][0-9])$").Success)
            {
                throw new ArgumentException("Date-Time format must be \"yyyy-MM-ddTHH:mm\".", nameof(datetime));
            }

            Date = date;
            Time = time;
            DateTime = datetime;
        }
    }
}
