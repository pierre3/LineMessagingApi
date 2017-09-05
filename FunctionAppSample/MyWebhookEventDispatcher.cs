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
            Log.WriteInfo($"SourceType:{ev.Source.Type}, UserId:{ev.Source.UserId}, MessageType:{ev.Message.Type}");

            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    var textMessage = (TextEventMessage)ev.Message;
                    await MessagingClient.ReplyMessageAsync(ev.ReplyToken, textMessage.Text);
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

        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, UserId:{ev.Source.UserId}");

            await TableStorage.AddEntryAsync(ev.Source.Type.ToString(), ev.Source.UserId);

            var userName = "";
            if (!string.IsNullOrEmpty(ev.Source.UserId))
            {
                var userProfile = await MessagingClient.GetUserProfileAsync(ev.Source.UserId);
                userName = userProfile?.DisplayName ?? "";
            }

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Hello {userName}! Thank you for following !");
        }

        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, UserId:{ev.Source.UserId}");

            await TableStorage.DeleteEntryAsync(ev.Source.Type.ToString(), ev.Source.UserId);
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            var entryId = (ev.Source.Type == EventSourceType.Group) ?
                ((SourceGroup)ev.Source).GroupId :
                ((SourceRoom)ev.Source).RoomId;
            Log.WriteInfo($"SourceType:{ev.Source.Type}, UserId:{entryId}");

            await TableStorage.DeleteEntryAsync(ev.Source.Type.ToString(), entryId);
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            if (ev.Source.Type == EventSourceType.Group)
            {
                var groupId = ((SourceGroup)ev.Source).GroupId;
                Log.WriteInfo($"SourceType:{ev.Source.Type}, UserId:{groupId}");
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your group!");
            }
            else if (ev.Source.Type == EventSourceType.Room)
            {
                var roomId = ((SourceRoom)ev.Source).RoomId;
                Log.WriteInfo($"SourceType:{ev.Source.Type}, UserId:{roomId}");
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your room!");
            }
        }
        protected override Task OnBeaconAsync(BeaconEvent ev)
        {
            throw new NotImplementedException();
        }

        protected override Task OnPostbackAsync(PostbackEvent ev)
        {
            throw new NotImplementedException();
        }
    }


}
