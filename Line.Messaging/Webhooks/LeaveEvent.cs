namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Event object for when your account leaves a group.
    /// No event is generated when your account leaves a room.
    /// Leave events are not generated if you leave a group or room using leave group or leave room.
    /// </summary>
    public class LeaveEvent : WebhookEvent
    {
        public LeaveEvent(WebhookEventSource source, long timestamp)
            : base(WebhookEventType.Leave, source, timestamp)
        {
        }
    }
}
