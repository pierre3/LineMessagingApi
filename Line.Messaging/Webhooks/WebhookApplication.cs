using System.Collections.Generic;
using System.Threading.Tasks;

namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Inherit this class to implement LINE Bot. Then override each event handler.
    /// </summary>
    public abstract class WebhookApplication
    {
        public async Task RunAsync(IEnumerable<WebhookEvent> events)
        {
            foreach (var ev in events)
            {
                switch (ev.Type)
                {
                    case WebhookEventType.Message:
                        await OnMessageAsync((MessageEvent)ev).ConfigureAwait(false);
                        break;
                    case WebhookEventType.Join:
                        await OnJoinAsync((JoinEvent)ev).ConfigureAwait(false);
                        break;
                    case WebhookEventType.Leave:
                        await OnLeaveAsync((LeaveEvent)ev).ConfigureAwait(false);
                        break;
                    case WebhookEventType.Follow:
                        await OnFollowAsync((FollowEvent)ev).ConfigureAwait(false);
                        break;
                    case WebhookEventType.Unfollow:
                        await OnUnfollowAsync((UnfollowEvent)ev).ConfigureAwait(false);
                        break;
                    case WebhookEventType.Postback:
                        await OnPostbackAsync((PostbackEvent)ev).ConfigureAwait(false);
                        break;
                    case WebhookEventType.Beacon:
                        await OnBeaconAsync((BeaconEvent)ev).ConfigureAwait(false);
                        break;
                }
            }
        }

        protected virtual Task OnMessageAsync(MessageEvent ev) => Task.CompletedTask;

        protected virtual Task OnJoinAsync(JoinEvent ev) => Task.CompletedTask;

        protected virtual Task OnLeaveAsync(LeaveEvent ev) => Task.CompletedTask;

        protected virtual Task OnFollowAsync(FollowEvent ev) => Task.CompletedTask;

        protected virtual Task OnUnfollowAsync(UnfollowEvent ev) => Task.CompletedTask;

        protected virtual Task OnBeaconAsync(BeaconEvent ev) => Task.CompletedTask;

        protected virtual Task OnPostbackAsync(PostbackEvent ev) => Task.CompletedTask;
    }
}
