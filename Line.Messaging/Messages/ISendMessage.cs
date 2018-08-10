namespace Line.Messaging
{
    public interface ISendMessage
    {
        MessageType Type { get; }

        /// <summary>
        /// These properties are used for the quick reply feature. Supported on LINE 8.11.0 and later for iOS and Android. For more information, see Using quick replies.
        /// </summary>
        QuickReply QuickReply { get; set; }
    }
}
