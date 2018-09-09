using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class LineBotApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }
        private TableStorage<BotStatus> SourceState { get; }
        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        public LineBotApp(LineMessagingClient lineMessagingClient, TableStorage<BotStatus> tableStorage, BlobStorage blobStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            SourceState = tableStorage;
            BlobStorage = blobStorage;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            var entry = await SourceState.FindAsync(ev.Source.Type.ToString(), ev.Source.Id);
            var blobDirectoryName = ev.Source.Type + "_" + ev.Source.Id;
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    await EchoAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text);
                    break;

                case EventMessageType.Image:
                    await EchoImageAsync(ev.ReplyToken, ev.Message.Id, blobDirectoryName);
                    break;

                case EventMessageType.Audio:
                case EventMessageType.Video:
                case EventMessageType.File:
                    await UploadMediaContentAsync(ev.ReplyToken, ev.Message.Id, blobDirectoryName, ev.Message.Id);
                    break;

                case EventMessageType.Location:
                    var location = ((LocationEventMessage)ev.Message);
                    await EchoAsync(ev.ReplyToken, $"@{location.Latitude},{location.Longitude}");
                    break;

                case EventMessageType.Sticker:
                    await ReplyRandomStickerAsync(ev.ReplyToken);
                    break;

            }
        }

        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}");

            await SourceState.AddAsync(ev.Source.Type.ToString(), ev.Source.Id);

            var userName = "";
            if (!string.IsNullOrEmpty(ev.Source.Id))
            {
                var userProfile = await MessagingClient.GetUserProfileAsync(ev.Source.Id);
                userName = userProfile?.DisplayName ?? "";
            }

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Hello {userName}! Thank you for following !");
        }

        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}");
            await SourceState.DeleteAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}");
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your {ev.Source.Type.ToString().ToLower()}!");
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}");
            await SourceState.DeleteAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnBeaconAsync(BeaconEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}");
            var message = "";
            switch (ev.Beacon.Type)
            {
                case BeaconType.Enter:
                    message = "You entered the beacon area!";
                    break;
                case BeaconType.Leave:
                    message = "You leaved the beacon area!";
                    break;
                case BeaconType.Banner:
                    message = "You tapped the beacon banner!";
                    break;
            }
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"{message}(Dm:{ev.Beacon.Dm}, Hwid:{ev.Beacon.Hwid})");
        }

        private Task EchoAsync(string replyToken, string userMessage)
        {
            return MessagingClient.ReplyMessageAsync(replyToken, userMessage);
        }

        private async Task EchoImageAsync(string replyToken, string messageId, string blobDirectoryName)
        {
            var imageName = messageId + ".jpeg";
            var previewImageName = messageId + "_preview.jpeg";

            var imageStream = await MessagingClient.GetContentStreamAsync(messageId);

            var image = System.Drawing.Image.FromStream(imageStream);
            var previewImage = image.GetThumbnailImage((int)(image.Width * 0.25), (int)(image.Height * 0.25), () => false, IntPtr.Zero);

            var blobImagePath = await BlobStorage.UploadImageAsync(image, blobDirectoryName, imageName);
            var blobPreviewPath = await BlobStorage.UploadImageAsync(previewImage, blobDirectoryName, previewImageName);

            await MessagingClient.ReplyMessageAsync(replyToken, new[] { new ImageMessage(blobImagePath.ToString(), blobPreviewPath.ToString()) });
        }

        private async Task UploadMediaContentAsync(string replyToken, string messageId, string blobDirectoryName, string blobName)
        {
            var stream = await MessagingClient.GetContentStreamAsync(messageId);
            var ext = GetFileExtension(stream.ContentHeaders.ContentType.MediaType);
            var uri = await BlobStorage.UploadFromStreamAsync(stream, blobDirectoryName, blobName + ext);
            await MessagingClient.ReplyMessageAsync(replyToken, uri.ToString());
        }

        private async Task ReplyRandomStickerAsync(string replyToken)
        {
            //Sticker ID of bssic stickers (packge ID =1)
            //see https://devdocs.line.me/files/sticker_list.pdf
            var stickerids = Enumerable.Range(1, 17)
                .Concat(Enumerable.Range(21, 1))
                .Concat(Enumerable.Range(100, 139 - 100 + 1))
                .Concat(Enumerable.Range(401, 430 - 400 + 1)).ToArray();

            var rand = new Random(Guid.NewGuid().GetHashCode());
            var stickerId = stickerids[rand.Next(stickerids.Length - 1)].ToString();
            await MessagingClient.ReplyMessageAsync(replyToken, new[] {
                        new StickerMessage("1", stickerId)
                    });
        }

        private string GetFileExtension(string mediaType)
        {
            switch (mediaType)
            {
                case "image/jpeg":
                    return ".jpeg";
                case "audio/x-m4a":
                    return ".m4a";
                case "video/mp4":
                    return ".mp4";
                default:
                    return "";
            }
        }
        
    }
}
