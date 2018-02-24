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
            try
            {
                var channelSecret = System.Configuration.ConfigurationManager.AppSettings["ChannelSecret"];
                var events = await req.GetWebhookEventsAsync(channelSecret);

                var app = await SwitchAppsAsync(events, log);
                await app.RunAsync(events);

            }
            catch (InvalidSignatureException e)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden, new { Message = e.Message });
            }
            catch (LineResponseException e)
            {
                log.Error(e.ToString());
                var debugUserId = System.Configuration.ConfigurationManager.AppSettings["DebugUser"];
                if (debugUserId != null)
                {
                    await lineMessagingClient.PushMessageAsync(debugUserId, @"{e.StatusCode}({(int)e.StatusCode}), {e.ResponseMessage.ToString()}");
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                var debugUserId = System.Configuration.ConfigurationManager.AppSettings["DebugUser"];
                if (debugUserId != null)
                {
                    await lineMessagingClient.PushMessageAsync(debugUserId, e.Message);
                }
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }

        private static async Task<WebhookApplication> SwitchAppsAsync(IEnumerable<WebhookEvent> events, TraceWriter log)
        {
            var connectionString = System.Configuration.ConfigurationManager.AppSettings["AzureWebJobsStorage"];
            var botStatus = await TableStorage<BotStatus>.CreateAsync(connectionString, "botstatus");
            var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");

            var ev = events.First();
            var status = await botStatus.FindAsync(ev.Source.Type.ToString(), ev.Source.Id);
            if (status == null)
            {
                status = new BotStatus()
                {
                    SourceType = ev.Source.Type.ToString(),
                    SourceId = ev.Source.Id,
                    CurrentApp = "@"
                };
            }

            var message = (ev as MessageEvent)?.Message as TextEventMessage;
            var text = message?.Text;
            if (text == null || !text.StartsWith("@"))
            {
                text = status.CurrentApp ?? "@";
            }
            text = text.Trim().ToLower();
            WebhookApplication app = null;

            if (text != "@richmenu")
            {
                await lineMessagingClient.UnLinkRichMenuFromUserAsync(ev.Source.Id);
            }

            switch (text)
            {
                case "@":
                    app = new LineBotApp(lineMessagingClient, botStatus, blobStorage, log);
                    break;
                case "@buttons":
                    app = new ButtonsTemplateSampleApp(lineMessagingClient, blobStorage, log);
                    break;
                case "@carousel":
                    app = new CarouselTemplateSampleApp(lineMessagingClient, blobStorage, log);
                    break;
                case "@postback":
                    app = new PostbackMessageSampleApp(lineMessagingClient, botStatus, log);
                    break;
                case "@imagemap":
                    app = new ImagemapSampleApp(lineMessagingClient, blobStorage, log);
                    break;
                case "@imagecarousel":
                    app = new ImageCarouselSampleApp(lineMessagingClient, blobStorage, log);
                    break;
                case "@datetime":
                    app = new DateTimePickerSampleApp(lineMessagingClient, log);
                    break;
                case "@richmenu":
                    app = new RichMenuSampleApp(lineMessagingClient, log);
                    break;
                default:
                    text = "@";
                    app = new LineBotApp(lineMessagingClient, botStatus, blobStorage, log);
                    break;
            }
            status.CurrentApp = text;
            await botStatus.UpdateAsync(status);
            return app;
        }
    }

}
