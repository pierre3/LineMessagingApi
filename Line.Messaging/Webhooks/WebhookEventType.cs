namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Webhook Event Type
    /// </summary>
    public enum WebhookEventType
    {
        Message,
        Follow,
        Unfollow,
        Join,
        Leave,
        Postback,
        Beacon
    }
}
