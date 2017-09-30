using System;

namespace Line.Messaging.Webhooks
{
    public abstract class WebhookEvent
    {
        public WebhookEventType Type { get; }

        public WebhookEventSource Source { get; }

        public long Timestamp { get; }

        public WebhookEvent(WebhookEventType type, WebhookEventSource source, long timestamp)
        {
            Type = type;
            Source = source;
            Timestamp = timestamp;
        }

        public static WebhookEvent CreateFrom(dynamic dynamicObject)
        {
            if (dynamicObject == null) { throw new ArgumentNullException(nameof(dynamicObject)); }

            var eventSource = WebhookEventSource.CreateFrom(dynamicObject);

            if (eventSource == null)
            {
                return null;
            }
            if (!Enum.TryParse((string)dynamicObject.type, true, out WebhookEventType eventType))
            {
                return null;
            }

            switch (eventType)
            {
                case WebhookEventType.Message:
                    EventMessage eventMessage = EventMessage.CreateFrom(dynamicObject);
                    if (eventMessage == null) { return null; }
                    return new MessageEvent(eventSource, (long)dynamicObject.timestamp, eventMessage, (string)dynamicObject.replyToken);
                case WebhookEventType.Follow:
                    return new FollowEvent(eventSource, (long)dynamicObject.timestamp, (string)dynamicObject.replyToken);
                case WebhookEventType.Unfollow:
                    return new UnfollowEvent(eventSource, (long)dynamicObject.timestamp);
                case WebhookEventType.Join:
                    return new JoinEvent(eventSource, (long)dynamicObject.timestamp, (string)dynamicObject.replyToken);
                case WebhookEventType.Leave:
                    return new LeaveEvent(eventSource, (long)dynamicObject.timestamp);
                case WebhookEventType.Postback:
                    var postback = new Postback(
                        (string)dynamicObject.postback?.data,
                        (string)dynamicObject.postback?.@params?.date,
                        (string)dynamicObject.postback?.@params?.time,
                        (string)dynamicObject.postback?.@params?.datetime);
                    return new PostbackEvent(eventSource, (long)dynamicObject.timestamp, (string)dynamicObject.replyToken, postback);
                case WebhookEventType.Beacon:
                    if (!Enum.TryParse((string)dynamicObject.beacon.type, true, out BeaconType beaconType))
                    {
                        return null;
                    }
                    return new BeaconEvent(eventSource, (long)dynamicObject.timestamp, (string)dynamicObject.replyToken,
                        (string)dynamicObject.beacon.hwid, beaconType, (string)dynamicObject.beacon.dm);
                default:
                    return null;
            }
        }
    }

    public abstract class ReplyableEvent : WebhookEvent
    {
        public string ReplyToken { get; }

        public ReplyableEvent(WebhookEventType eventType, WebhookEventSource source, long timestamp, string replyToken)
            : base(eventType, source, timestamp)
        {
            ReplyToken = replyToken;
        }

    }
    public class MessageEvent : ReplyableEvent
    {
        public EventMessage Message { get; }

        public MessageEvent(WebhookEventSource source, long timestamp, EventMessage message, string replyToken)
            : base(WebhookEventType.Message, source, timestamp, replyToken)
        {
            Message = message;
        }
    }

    public class FollowEvent : ReplyableEvent
    {
        public FollowEvent(WebhookEventSource source, long timestamp, string replyToken)
            : base(WebhookEventType.Follow, source, timestamp, replyToken)
        {
        }
    }

    public class UnfollowEvent : WebhookEvent
    {
        public UnfollowEvent(WebhookEventSource source, long timestamp)
            : base(WebhookEventType.Unfollow, source, timestamp)
        {
        }
    }

    public class JoinEvent : ReplyableEvent
    {
        public JoinEvent(WebhookEventSource source, long timestamp, string replyToken)
            : base(WebhookEventType.Join, source, timestamp, replyToken)
        {

        }
    }

    public class LeaveEvent : WebhookEvent
    {
        public LeaveEvent(WebhookEventSource source, long timestamp)
            : base(WebhookEventType.Leave, source, timestamp)
        {
        }
    }

    public class PostbackEvent : ReplyableEvent
    {
        public Postback Postback { get; }

        public PostbackEvent(WebhookEventSource source, long timestamp, string replyToken, Postback postback)
            : base(WebhookEventType.Postback, source, timestamp, replyToken)
        {
            Postback = postback;
        }
    }

    public class BeaconEvent : ReplyableEvent
    {
        public Beacon Beacon { get; }

        public BeaconEvent(WebhookEventSource source, long timestamp, string replyToken, string hwid, BeaconType beaconType, string dm)
            : base(WebhookEventType.Beacon, source, timestamp, replyToken)
        {
            Beacon = new Beacon(hwid, beaconType, dm);
        }
    }

    public class Postback
    {
        public string Data { get; }
        public PostbackParams Params { get; }

        public Postback(string data, string date, string time, string datetime)
        {
            Data = data;
            Params = new PostbackParams(date, time, datetime);
        }
    }

    public class PostbackParams
    {
        public string Date { get; }
        public string Time { get; }
        public string DateTime { get; }

        public PostbackParams(string date, string time, string datetime)
        {
            Date = date;
            Time = time;
            DateTime = datetime;
        }
    }

    public class Beacon
    {
        public string Hwid { get; }

        public BeaconType Type { get; }

        public string Dm { get; }
        public Beacon(string hwid, BeaconType type, string dm)
        {
            Hwid = hwid;
            Type = type;
            Dm = dm;
        }
    }

    public enum BeaconType
    {
        Enter,
        Leave,
        Banner
    }

}
