namespace Line.Messaging.Webhooks
{
    public abstract class DeviceEvent : WebhookEvent
    {
        public Things Things { get; }
        public DeviceEvent(WebhookEventSource source, long timestamp, Things things)
            : base(WebhookEventType.Things, source, timestamp)
        {
            Things = things;
        }

        public static DeviceEvent Create(WebhookEventSource source, long timestamp, Things things)
        {
            return (things.Type == ThingsType.Link)
                ? new DeviceLinkEvent(source, timestamp, things) as DeviceEvent
                : new DeviceUnlinkEvent(source, timestamp, things) as DeviceEvent;
        }
    }

}
