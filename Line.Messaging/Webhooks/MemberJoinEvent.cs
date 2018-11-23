using System.Collections.Generic;

namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Event object for when a user joins a group or room that the bot is in.
    /// </summary>
    public class MemberJoinEvent : ReplyableEvent
    {
        /// <summary>
        /// User ID of users who joined
        /// </summary>
        public Moved Joined { get; }

        public MemberJoinEvent(WebhookEventSource source, long timestamp, string replyToken, IList<WebhookEventSource> members)
            : base(WebhookEventType.MemberJoined, source, timestamp, replyToken)
        {
            Joined = new Moved(members);
        }
    }

}
