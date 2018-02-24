using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class CarouselTemplateSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        private static string blobDirectoryName = "TemplateImage";
        private static string imageName = "sample.jpeg";

        public CarouselTemplateSampleApp(LineMessagingClient lineMessagingClient, BlobStorage blobStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            BlobStorage = blobStorage;
            Log = log;
        }


        protected override async Task OnMessageAsync(MessageEvent ev)
        {

            var msg = ev.Message as TextEventMessage;
            if (msg == null) { return; }

            switch (msg.Text)
            {
                case "Rectangle-Cover":
                    await ReplyCarouselTemplateMessageAsync(ev.ReplyToken, ImageAspectRatioType.Rectangle, ImageSizeType.Cover);
                    break;
                case "Square-Contain":
                    await ReplyCarouselTemplateMessageAsync(ev.ReplyToken, ImageAspectRatioType.Square, ImageSizeType.Contain);
                    break;
                case "Square-Cover":
                    await ReplyCarouselTemplateMessageAsync(ev.ReplyToken, ImageAspectRatioType.Square, ImageSizeType.Cover);
                    break;
                case "Rectangle-Contein":
                default:
                    await ReplyCarouselTemplateMessageAsync(ev.ReplyToken, ImageAspectRatioType.Rectangle, ImageSizeType.Contain);
                    break;
            }

            async Task ReplyCarouselTemplateMessageAsync(string replyToken, ImageAspectRatioType imageAspectRatio, ImageSizeType imageSize)
            {
                var imageUri = BlobStorage.ListBlobUri(blobDirectoryName).FirstOrDefault(uri => uri.ToString().EndsWith(imageName));
                if (imageUri == null)
                {
                    imageUri = await BlobStorage.UploadImageAsync(Properties.Resources.sample_image, blobDirectoryName, imageName);
                }
                var defaultAction = new MessageTemplateAction("Default-Action", "Default-Action");
                var templateMessage = new TemplateMessage("CarouselTemplate",
                    new CarouselTemplate(new[]
                    {
                        new CarouselColumn(
                            imageAspectRatio + "-" + imageSize,
                            imageUri.ToString(),
                            "Test of thumbnail image settings",
                            new []{ new MessageTemplateAction("Rectangle-Contain", "Rectangle-Contain") },
                            "#FF0000",
                            defaultAction),
                        new CarouselColumn(
                            imageAspectRatio + "-" + imageSize,
                            imageUri.ToString(),
                            "Test of thumbnail image settings",
                            new []{ new MessageTemplateAction("Rectangle-Cover", "Rectangle-Cover") },
                            "#00FF00",
                            defaultAction),
                        new CarouselColumn(
                            imageAspectRatio + "-" + imageSize,
                            imageUri.ToString(),
                            "Test of thumbnail image settings",
                            new []{ new MessageTemplateAction("Square-Contain", "Square-Contain") },
                            "#0000FF",
                            defaultAction),
                        new CarouselColumn(
                            imageAspectRatio + "-" + imageSize,
                            imageUri.ToString(),
                            "Test of thumbnail image settings",
                            new []{ new MessageTemplateAction("Square-Cover", "Square-Cover") },
                            "#FF00FF",
                            defaultAction)
                    },
                    imageAspectRatio,
                    imageSize));

                await MessagingClient.ReplyMessageAsync(replyToken, new[] { templateMessage });
            }
        }
    }

}


