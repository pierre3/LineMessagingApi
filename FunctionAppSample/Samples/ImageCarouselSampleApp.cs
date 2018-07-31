using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class ImageCarouselSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        public ImageCarouselSampleApp(LineMessagingClient lineMessagingClient, BlobStorage blobStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            BlobStorage = blobStorage;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            var blobDirectoryName = "imageCarousel/" + ev.Source.Id;
            switch (ev.Message.Type)
            {
                case EventMessageType.Image:
                    await UploadImageAsync(ev.ReplyToken, ev.Message.Id, blobDirectoryName);
                    break;
                case EventMessageType.Text:
                    if (((TextEventMessage)ev.Message).Text.StartsWith("I selected image"))
                    { break; }

                    await ReplyImageCarouselAsync(ev.ReplyToken, blobDirectoryName);
                    break;
            }
        }

        private async Task UploadImageAsync(string replyToken, string messageId, string blobDirectoryName)
        {
            var imageStream = await MessagingClient.GetContentStreamAsync(messageId);
            var image = System.Drawing.Image.FromStream(imageStream);

            var imageCount = BlobStorage.ListBlobUri(blobDirectoryName).Count();
            if (imageCount == 5)
            {
                await BlobStorage.DeleteDirectoryAsync(blobDirectoryName);
                imageCount = 0;
            }

            await BlobStorage.UploadImageAsync(image, blobDirectoryName, (imageCount + 1) + ".jpeg");

            await MessagingClient.ReplyMessageAsync(replyToken, $"Image uploaded ({imageCount + 1}).");
        }


        private async Task ReplyImageCarouselAsync(string replyToken, string blobDirectoryName)
        {
            var columns = BlobStorage.ListBlobUri(blobDirectoryName)
                .Select(uri =>
                    new ImageCarouselColumn(uri.ToString(),
                        new MessageTemplateAction(uri.Segments.Last(), $"I selected image {uri.Segments.Last()}!"))).ToList();
            if (columns.Count == 0)
            {
                await MessagingClient.ReplyMessageAsync(replyToken, "Upload image for \"Carousel Message\".");
                return;
            }
            var template = new ImageCarouselTemplate(columns);

            await MessagingClient.ReplyMessageAsync(replyToken, new[] { new TemplateMessage("imageCarousel", template) });
        }

    }
}
