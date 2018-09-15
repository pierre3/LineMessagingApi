using System;

namespace Line.Messaging
{
    /// <summary>
    /// Location
    /// https://developers.line.me/en/docs/messaging-api/reference/#location
    /// </summary>
    public class LocationMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Location;

        /// <summary>
        /// These properties are used for the quick reply feature
        /// </summary>
        public QuickReply QuickReply { get; set; }

        /// <summary>
        /// Title
        /// Max: 100 characters
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Address
        /// Max: 100 characters
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">
        /// Title
        /// Max: 100 characters
        /// </param>
        /// <param name="address">
        /// Address
        /// Max: 100 characters
        /// </param>
        /// <param name="latitude">
        /// Latitude
        /// </param>
        /// <param name="longitude">
        /// Longitude
        /// </param>
        /// <param name="quickReply">
        /// QuickReply
        /// </param>
        public LocationMessage(string title, string address, decimal latitude, decimal longitude, QuickReply quickReply = null)
        {
            Title = title.Substring(0, Math.Min(title.Length, 100));
            Address = address.Substring(0, Math.Min(address.Length, 100));
            Latitude = latitude;
            Longitude = longitude;
            QuickReply = quickReply;
        }
    }
}
