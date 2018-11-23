using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class ImagemapSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        public ImagemapSampleApp(LineMessagingClient lineMessagingClient, BlobStorage blobStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            BlobStorage = blobStorage;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            switch (ev.Message.Type)
            {
                case EventMessageType.Image:
                    await ReplyImagemapAsync(ev.ReplyToken, ev.Message.Id, ev.Source.Type + "_" + ev.Source.Id);
                    break;
                case EventMessageType.Video:
                    await UploadVideoAsync(ev.ReplyToken, ev.Message.Id ,ev.Source.Type + "_" + ev.Source.Id);
                    break;
                case EventMessageType.Text:
                    await MessagingClient.ReplyMessageAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text);
                    break;
            }
        }


        private async Task ReplyImagemapAsync(string replyToken, string messageId, string blobDirectoryName)
        {
            var imageStream = await MessagingClient.GetContentStreamAsync(messageId);
            var image = System.Drawing.Image.FromStream(imageStream);

            using (var g = Graphics.FromImage(image))
            {
                g.DrawLine(Pens.Red, image.Width / 2, 0, image.Width / 2, image.Height);
                g.DrawLine(Pens.Red, 0, image.Height / 2, image.Width, image.Height / 2);
            }

            var uri = await UploadImageAsync(1040);
            await UploadImageAsync(700);
            await UploadImageAsync(460);
            await UploadImageAsync(300);
            await UploadImageAsync(240);
            var imageSize = new ImagemapSize(1024, (int)(1040 * (double)image.Height / image.Width));
            var areaWidth = imageSize.Width / 2;
            var areaHeight = imageSize.Height / 2;
            Video video = null;
            var videoUrl = BlobStorage.ListBlobUri(blobDirectoryName).FirstOrDefault(x => x.ToString().EndsWith("video.mp4"));
            if (videoUrl!=null)
            {
                video = new Video(videoUrl.ToString(), videoUrl.ToString().Replace("video.mp4","300"),
                    new ImagemapArea(areaWidth/2, areaHeight/2, areaWidth, areaHeight),
                    new ExternalLink("https://google.com", "google"));
            }
            var imagemapMessage = new ImagemapMessage(uri.ToString().Replace("/1040", ""),
                "Sample Imagemap",
                imageSize,
                new IImagemapAction[] {
                    new MessageImagemapAction(new ImagemapArea(0, 0, areaWidth,areaHeight),"Area Top-Left"),
                    new MessageImagemapAction(new ImagemapArea(areaWidth, 0, areaWidth,areaHeight),"Area Top-Right"),
                    new MessageImagemapAction(new ImagemapArea(0, areaHeight, areaWidth,areaHeight),"Area Bottom-Left"),
                    new MessageImagemapAction(new ImagemapArea(areaWidth, areaHeight, areaWidth,areaHeight),"Area Bottom-Right"),
                },
                video: video);

            await MessagingClient.ReplyMessageAsync(replyToken, new[] { imagemapMessage });

            async Task<Uri> UploadImageAsync(int baseSize)
            {
                var img = image.GetThumbnailImage(baseSize, image.Height * baseSize / image.Width, () => false, IntPtr.Zero);
                return await BlobStorage.UploadImageAsync(img, blobDirectoryName, baseSize.ToString());
            }
        }

        private async Task UploadVideoAsync(string replyToken, string messageId, string blobDirectoryName)
        {
            var videoStream = await MessagingClient.GetContentStreamAsync(messageId);
            var url = await BlobStorage.UploadFromStreamAsync(videoStream, blobDirectoryName, "video.mp4");

            await MessagingClient.ReplyMessageAsync(replyToken, url.ToString());
        }

    }
}
