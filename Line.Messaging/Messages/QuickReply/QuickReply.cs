using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// https://developers.line.me/en/reference/messaging-api/#quick-reply
    /// These properties are used for the quick reply feature. Supported on LINE 8.11.0 and later for iOS and Android. For more information, see Using quick replies.
    /// </summary>
    public class QuickReply
    {
        /// <summary>
        /// Quick reply button objects. Max: 13 objects
        /// </summary>
        public IList<QuickReplyButtonObject> Items { get; set; }

        public QuickReply(IList<QuickReplyButtonObject> items = null)
        {
            Items = items ?? new List<QuickReplyButtonObject>();
        }
    }
}