namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Event object for when your account is added as a friend (or unblocked). You can reply to follow events.
    /// </summary>
    public class FollowEvent : ReplyableEvent
    {
        public FollowEvent(WebhookEventSource source, long timestamp, string replyToken)
            : base(WebhookEventType.Follow, source, timestamp, replyToken)
        {
        }
    }
}
