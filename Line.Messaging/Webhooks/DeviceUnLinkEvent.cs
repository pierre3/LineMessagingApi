namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Indicates that a LINE Things-compatible device has been unlinked from LINE by a user operation. For more information, see Receiving device unlink events via webhook.
    /// </summary>
    public class DeviceUnlinkEvent : DeviceEvent
    {
        public DeviceUnlinkEvent(WebhookEventSource source, long timestamp, Things things) : base(source, timestamp, things)
        {
        }
    }

}
