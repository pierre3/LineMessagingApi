namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Event object for when a user enters or leaves the range of a LINE Beacon. You can reply to beacon events.
    /// https://developers.line.me/en/docs/messaging-api/reference/#beacon-event
    /// </summary>
    public class BeaconEvent : ReplyableEvent
    {
        public Beacon Beacon { get; }

        public BeaconEvent(WebhookEventSource source, long timestamp, string replyToken, string hwid, BeaconType beaconType, string dm)
            : base(WebhookEventType.Beacon, source, timestamp, replyToken)
        {
            Beacon = new Beacon(hwid, beaconType, dm);
        }
    }
}
