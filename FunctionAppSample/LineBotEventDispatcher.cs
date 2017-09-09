using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class LineBotEventDispatcher : WebhookEventDispatcher
    {
        private LineMessagingClient MessagingClient { get; }
        private LineBotTableStorage TableStorage { get; }
        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        public LineBotEventDispatcher(LineMessagingClient lineMessagingClient, LineBotTableStorage tableStorage, BlobStorage blobStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            TableStorage = tableStorage;
            BlobStorage = blobStorage;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            var entry = await TableStorage.FindEntryAsync(ev.Source.Type.ToString(), ev.Source.Id);
            var blobDirectoryName = ev.Source.Type + "_" + ev.Source.Id;
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    if (entry?.Location != null)
                    {
                        await ConfirmMapSearchAsync(ev.ReplyToken, entry, ((TextEventMessage)ev.Message).Text);
                        break;
                    }
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
                    await SaveLocationAsync(ev, (LocationEventMessage)ev.Message);
                    break;

                case EventMessageType.Sticker:
                    break;

            }
        }

        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");

            await TableStorage.AddEntryAsync(ev.Source.Type.ToString(), ev.Source.Id);

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
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");
            await TableStorage.DeleteEntryAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your {ev.Source.Type.ToString().ToLower()}!");
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");
            await TableStorage.DeleteEntryAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override Task OnBeaconAsync(BeaconEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");
            return MessagingClient.ReplyMessageAsync(ev.ReplyToken, "Beacon event not supported.");
        }

        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");

            var data = JsonConvert.DeserializeAnonymousType(ev.Postback.Data, new { type = "", searchWord = "", location = "" });
            if (data.type == "keyword")
            {
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"https://www.google.co.jp/maps/search/{data.searchWord}/@{data.location}");
            }
            else
            {
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"https://www.google.co.jp/maps/dir/{data.location}/{data.searchWord}");
            }

            await TableStorage.UpdateEntryAsync(new LineBotEntry(ev.Source.Type.ToString(), ev.Source.Id) { Location = null });
        }

        private Task EchoAsync(string replyToken, string userMessage)
        {
            return MessagingClient.ReplyMessageAsync(replyToken, userMessage);
        }

        private async Task SaveLocationAsync(MessageEvent ev, LocationEventMessage locMessage)
        {
            await TableStorage.UpdateEntryAsync(
                new LineBotEntry(ev.Source.Type.ToString(), ev.Source.Id)
                {
                    Location = $"{locMessage.Latitude},{locMessage.Longitude}"
                });
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "Enter a search word for google map search.");
        }

        private async Task ConfirmMapSearchAsync(string replyToken, LineBotEntry entry, string searchWord)
        {
            var templateMessage = new TemplateMessage("Google map search",
                new ConfirmTemplate($"Select a search type.",
                    new[]
                    {
                                    new PostbackTemplateAction("Keyword",JsonConvert.SerializeObject(
                                        new
                                        {
                                            type = "keyword",
                                            searchWord,
                                            location = entry.Location
                                        }),null),
                                    new PostbackTemplateAction("Route",JsonConvert.SerializeObject(
                                        new
                                        {
                                            type = "route",
                                            searchWord,
                                            location = entry.Location
                                        }),null)
                    }));
            await MessagingClient.ReplyMessageAsync(replyToken, new[] { templateMessage });
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
