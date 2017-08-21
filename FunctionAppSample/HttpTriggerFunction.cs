using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppSample
{
    internal static class LineMessagingService
    {

        public static void Initialize(string channelAccessToken)
        {
        }

        public static async Task<bool> VerifySignature(HttpRequestMessage req, string channelSecret)
        {
            var content = await req.Content.ReadAsStringAsync();
            var xLineSignature = req.Headers.GetValues("X-Line-Signature").FirstOrDefault();
            return VerifySignature(channelSecret, xLineSignature, content);
        }

        private static bool VerifySignature(string channelSecret, string xLineSignature, string requestBody)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(channelSecret);
                var body = Encoding.UTF8.GetBytes(requestBody);

                using (HMACSHA256 hmac = new HMACSHA256(key))
                {
                    var hash = hmac.ComputeHash(body, 0, body.Length);
                    var hash64 = Convert.ToBase64String(hash);
                    return xLineSignature == hash64;
                }
            }
            catch
            {
                return false;
            }
        }
    }
    public static class HttpTriggerFunction
    {
        static LineMessagingClient lineMessagingClient;
        static HttpTriggerFunction()
        {
            lineMessagingClient = new LineMessagingClient(System.Configuration.ConfigurationManager.AppSettings["ChannelAccessToken"]);
            var sp = ServicePointManager.FindServicePoint(new Uri("https://api.line.me"));
            sp.ConnectionLeaseTimeout = 60 * 1000;
        }

        [FunctionName("LineMessagingApiSample")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            IEnumerable<WebhookEvent> events;
            try
            {
                events = await req.GetWebhookEventsAsync(System.Configuration.ConfigurationManager.AppSettings["ChannelSecret"]);
            }
            catch (InvalidSignatureException e)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden, new { Message = e.Message });
            }

            foreach (var ev in events.OfType<MessageEvent>())
            {
                switch (ev.Message.Type)
                {
                    case EventMessageType.Text:
                        var textMessage = (TextEventMessage)ev.Message;
                        await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { new TextMessage(textMessage.Text) });
                        break;
                    case EventMessageType.Image:
                    case EventMessageType.Audio:
                    case EventMessageType.Video:
                    case EventMessageType.File:
                    case EventMessageType.Location:
                    case EventMessageType.Sticker:
                        break;
                }
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }

    }
}
