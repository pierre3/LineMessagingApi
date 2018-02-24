using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class ButtonsTemplateSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        private static string blobDirectoryName = "TemplateImage";
        private static string imageName = "sample.jpeg";

        public ButtonsTemplateSampleApp(LineMessagingClient lineMessagingClient, BlobStorage blobStorage, TraceWriter log)
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
                    await ReplyButtonsTemplateMessageAsync(ImageAspectRatioType.Rectangle, ImageSizeType.Cover);
                    break;
                case "Square-Contain":
                    await ReplyButtonsTemplateMessageAsync(ImageAspectRatioType.Square, ImageSizeType.Contain);
                    break;
                case "Square-Cover":
                    await ReplyButtonsTemplateMessageAsync(ImageAspectRatioType.Square, ImageSizeType.Cover);
                    break;
                case "Rectangle-Contein":
                default:
                    await ReplyButtonsTemplateMessageAsync(ImageAspectRatioType.Rectangle, ImageSizeType.Contain);
                    break;
            }

            async Task ReplyButtonsTemplateMessageAsync(ImageAspectRatioType imageAspectRatio, ImageSizeType imageSize)
            {
                var imageUri = BlobStorage.ListBlobUri(blobDirectoryName).FirstOrDefault(uri => uri.ToString().EndsWith(imageName));
                if (imageUri == null)
                {
                    imageUri = await BlobStorage.UploadImageAsync(Properties.Resources.sample_image, blobDirectoryName, imageName);
                }

                var actions = new[]
                {
                    new MessageTemplateAction("Rectangle-Contain", "Rectangle-Contain"),
                    new MessageTemplateAction("Rectangle-Cover", "Rectangle-Cover"),
                    new MessageTemplateAction("Square-Contain", "Square-Contain"),
                    new MessageTemplateAction("Square-Cover", "Square-Cover")
                };
                var templateMessage = new TemplateMessage("ButtonsTemplate",
                            new ButtonsTemplate(
                                imageAspectRatio.ToString() + "-" + imageSize.ToString(),
                                imageUri.ToString(),
                                "Test of thumbnail image settings",
                                actions,
                                imageAspectRatio,
                                imageSize,
                                imageBackgroundColor: "#FF0000",
                                defaultAction: new MessageTemplateAction("Default-Action","Default-Action")));
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { templateMessage });
            }

        }

    }
}
