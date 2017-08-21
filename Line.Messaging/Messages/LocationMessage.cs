namespace Line.Messaging
{
    public class LocationMessage : ISendMessage
    {
        public MessageType Type { get; } = MessageType.Location;

        public string Title { get; }

        public string Address { get; }

        public decimal Latitude { get; }

        public decimal Longitude { get; }
        
        public LocationMessage(string title, string address, decimal latitude, decimal longitude)
        {
            Title = title;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
