using System.Collections.Generic;

namespace Line.Messaging
{
    public class ChannelAccessToken
    {
        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }
        public string TokenType { get; } = "Bearer";
    }
}
