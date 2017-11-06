using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Short-lived channel access token that is valid for 30 days. 
    /// https://developers.line.me/en/docs/messaging-api/reference/#issue-channel-access-token
    /// </summary>
    public class ChannelAccessToken
    {
        /// <summary>
        /// Short-lived channel access token. Valid for 30 days.
        /// Note: Channel access tokens cannot be refreshed
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Time until channel access token expires in seconds from time the token is issued
        /// </summary>
        public long ExpiresIn { get; set; }
        
        /// <summary>
        /// Bearer
        /// </summary>
        public string TokenType { get; } = "Bearer";
    }
}
