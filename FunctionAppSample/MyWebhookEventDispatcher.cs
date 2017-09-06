using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{
    class MyWebhookEventDispatcher : WebhookEventDispatcher
    {

        private LineMessagingClient MessagingClient { get; }
        private LineBotTableStorage TableStorage { get; }
        private TraceWriter Log { get; }

        public MyWebhookEventDispatcher(LineMessagingClient lineMessagingClient, LineBotTableStorage tableStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            TableStorage = tableStorage;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.EntryId}, MessageType:{ev.Message.Type}");

            var entry = await TableStorage.FindEntryAsync(ev.Source.Type.ToString(), ev.Source.EntryId);
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    var textMessage = (TextEventMessage)ev.Message;
                    if (entry?.Location != null)
                    {
                        await MapSearchAsync(ev, entry, textMessage.Text);
                        break;
                    }
                    await EchoAsync(ev.ReplyToken, textMessage.Text);
                    break;
                case EventMessageType.Image:
                case EventMessageType.Audio:
                case EventMessageType.Video:
                case EventMessageType.File:
                    break;

                case EventMessageType.Location:
                    var locMessage = (LocationEventMessage)ev.Message;
                    await SaveLocationAsync(ev, locMessage);
                    break;

                case EventMessageType.Sticker:
                    break;
            }
        }

        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.EntryId}");

            await TableStorage.AddEntryAsync(ev.Source.Type.ToString(), ev.Source.EntryId);

            var userName = "";
            if (!string.IsNullOrEmpty(ev.Source.EntryId))
            {
                var userProfile = await MessagingClient.GetUserProfileAsync(ev.Source.EntryId);
                userName = userProfile?.DisplayName ?? "";
            }

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Hello {userName}! Thank you for following !");
        }

        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.EntryId}");
            await TableStorage.DeleteEntryAsync(ev.Source.Type.ToString(), ev.Source.EntryId);
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.EntryId}");
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your {ev.Source.Type.ToString().ToLower()}!");
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.EntryId}");
            await TableStorage.DeleteEntryAsync(ev.Source.Type.ToString(), ev.Source.EntryId);
        }

        protected override Task OnBeaconAsync(BeaconEvent ev)
        {
            throw new NotImplementedException();
        }

        protected override Task OnPostbackAsync(PostbackEvent ev)
        {
            throw new NotImplementedException();
        }

        private Task EchoAsync(string replyToken, string userMessage)
        {
            return MessagingClient.ReplyMessageAsync(replyToken, userMessage);
        }

        private async Task MapSearchAsync(MessageEvent ev, LineEntry entry, string searchWord)
        {
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"https://www.google.co.jp/maps/search/{searchWord}/{entry.Location}");
            await TableStorage.UpdateEntryAsync(new LineEntry(ev.Source.Type.ToString(), ev.Source.EntryId) { Location = null });
        }

        private async Task SaveLocationAsync(MessageEvent ev, LocationEventMessage locMessage)
        {
            await TableStorage.UpdateEntryAsync(
                new LineEntry(ev.Source.Type.ToString(), ev.Source.EntryId)
                {
                    Location = $"@{locMessage.Latitude},{locMessage.Longitude}"
                });
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "Input a search word.");
        }

    }


}
