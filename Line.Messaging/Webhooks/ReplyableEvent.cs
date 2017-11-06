namespace Line.Messaging.Webhooks
{
    public abstract class ReplyableEvent : WebhookEvent
    {
        public string ReplyToken { get; }

        public ReplyableEvent(WebhookEventType eventType, WebhookEventSource source, long timestamp, string replyToken)
            : base(eventType, source, timestamp)
        {
            ReplyToken = replyToken;
        }
    }
}
