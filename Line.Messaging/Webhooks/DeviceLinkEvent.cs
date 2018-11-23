namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Indicates that a LINE Things-compatible device has been linked with LINE by a user operation. For more information, see Receiving device link events via webhook.
    /// </summary>
    public class DeviceLinkEvent : DeviceEvent
    {
        public DeviceLinkEvent(WebhookEventSource source, long timestamp, Things things) : base(source, timestamp, things)
        {
        }
    }
}
