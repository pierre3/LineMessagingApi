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
                switch (ev)
                {
                    case MessageEvent message:
                        await OnMessageAsync(message).ConfigureAwait(false);
                        break;
                    case JoinEvent join:
                        await OnJoinAsync(join).ConfigureAwait(false);
                        break;
                    case LeaveEvent leave:
                        await OnLeaveAsync(leave).ConfigureAwait(false);
                        break;
                    case FollowEvent follow:
                        await OnFollowAsync(follow).ConfigureAwait(false);
                        break;
                    case UnfollowEvent unFollow:
                        await OnUnfollowAsync(unFollow).ConfigureAwait(false);
                        break;
                    case PostbackEvent postback:
                        await OnPostbackAsync(postback).ConfigureAwait(false);
                        break;
                    case BeaconEvent beacon:
                        await OnBeaconAsync(beacon).ConfigureAwait(false);
                        break;
                    case AccountLinkEvent accountLink:
                        await OnAccountLinkAsync(accountLink).ConfigureAwait(false);
                        break;
                    case MemberJoinEvent memberJoin:
                        await OnMemberJoinAsync(memberJoin).ConfigureAwait(false);
                        break;
                    case MemberLeaveEvent memberLeave:
                        await OnMemberLeaveAsync(memberLeave).ConfigureAwait(false);
                        break;
                    case DeviceLinkEvent deviceLink:
                        await OnDeviceLinkAsync(deviceLink).ConfigureAwait(false);
                        break;
                    case DeviceUnlinkEvent deviceUnlink:
                        await OnDeviceUnlinkAsync(deviceUnlink).ConfigureAwait(false);
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

        protected virtual Task OnAccountLinkAsync(AccountLinkEvent ev) => Task.CompletedTask;

        protected virtual Task OnMemberJoinAsync(MemberJoinEvent ev) => Task.CompletedTask;

        protected virtual Task OnMemberLeaveAsync(MemberLeaveEvent ev) => Task.CompletedTask;

        protected virtual Task OnDeviceLinkAsync(DeviceLinkEvent ev) => Task.CompletedTask;

        protected virtual Task OnDeviceUnlinkAsync(DeviceUnlinkEvent ev) => Task.CompletedTask;
    }
}
