namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Message object which contains the location data sent from the source.
    /// </summary>
    public class LocationEventMessage : EventMessage
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Latitude { get; }

        /// <summary>
        /// Longitude
        /// </summary>
        public decimal Longitude { get; }

        public LocationEventMessage(string id, string title, string address, decimal latitude, decimal longitude) : base(EventMessageType.Location, id)
        {
            Title = title;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
