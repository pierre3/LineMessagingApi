using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FunctionAppSample
{
    public static class AppSwitcher
    {
        public static async Task<WebhookApplication> SwitchAppsAsync(IEnumerable<WebhookEvent> events, LineMessagingClient line, TableStorage<BotStatus> botStatus, BlobStorage blobStorage, TraceWriter log)
        {

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
                text = status.CurrentApp;
            }
            text = text.Trim().ToLower();
            WebhookApplication app = null;

            if (text != "@richmenu")
            {
                try
                {
                    await line.UnLinkRichMenuFromUserAsync(ev.Source.Id);
                }
                catch (LineResponseException e)
                {
                    if (e.StatusCode != HttpStatusCode.NotFound)
                    {
                        throw;
                    }
                }
            }

            switch (text)
            {
                case "@":
                    app = new LineBotApp(line, botStatus, blobStorage, log);
                    break;
                case "@buttons":
                    app = new ButtonsTemplateSampleApp(line, blobStorage, log);
                    break;
                case "@carousel":
                    app = new CarouselTemplateSampleApp(line, blobStorage, log);
                    break;
                case "@postback":
                    app = new PostbackMessageSampleApp(line, botStatus, log);
                    break;
                case "@imagemap":
                    app = new ImagemapSampleApp(line, blobStorage, log);
                    break;
                case "@imagecarousel":
                    app = new ImageCarouselSampleApp(line, blobStorage, log);
                    break;
                case "@datetime":
                    app = new DateTimePickerSampleApp(line, log);
                    break;
                case "@richmenu":
                    app = new RichMenuSampleApp(line, log);
                    break;
                case "@flex":
                    app = new FlexMessageSampleApp(line, log);
                    break;
                case "@num":
                    app = new GetNumberOfSentMessagesSampleApp(line, log);
                    break;
                default:
                    text = "@";
                    app = new LineBotApp(line, botStatus, blobStorage, log);
                    break;
            }
            status.CurrentApp = text;
            await botStatus.UpdateAsync(status);
            return app;
        }
    }
}
