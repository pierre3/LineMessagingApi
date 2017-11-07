namespace Line.Messaging
{
    /// <summary>
    /// Response from Get User Profile API. 
    /// https://developers.line.me/en/docs/messaging-api/reference/#get-profile
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Image URL
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Status message
        /// </summary>
        public string StatusMessage { get; set; }
    }
}
