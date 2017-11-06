namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Event object for when your account joins a group or talk room. You can reply to join events.
    /// </summary>
    public class JoinEvent : ReplyableEvent
    {
        public JoinEvent(WebhookEventSource source, long timestamp, string replyToken)
            : base(WebhookEventType.Join, source, timestamp, replyToken)
        {
        }
    }
}
