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
            Date = date;
            Time = time;
            DateTime = datetime;
        }
    }
}
