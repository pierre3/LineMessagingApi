namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Postback
    /// </summary>
    public class Postback
    {
        /// <summary>
        /// Postback data
        /// </summary>
        public string Data { get; }

        /// <summary>
        /// date and time selected by a user through a datetime picker action.
        /// Only returned for postback actions via the datetime picker.
        /// </summary>
        public PostbackParams Params { get; }

        public Postback(string data, string date, string time, string datetime)
        {
            Data = data;
            Params = new PostbackParams(date, time, datetime);
        }
    }
}
