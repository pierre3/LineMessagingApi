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
using System.Threading.Tasks;

namespace FunctionAppSample
{
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
