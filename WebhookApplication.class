public abstract class WebhookApplication
{
    public async Task RunAsync(IEnumerable<WebhookEvent> events);
    
    protected virtual Task OnMessageAsync(MessageEvent ev);
    protected virtual Task OnJoinAsync(JoinEvent ev);
    protected virtual Task OnLeaveAsync(LeaveEvent ev);
    protected virtual Task OnFollowAsync(FollowEvent ev);
    protected virtual Task OnUnfollowAsync(UnfollowEvent ev);
    protected virtual Task OnBeaconAsync(BeaconEvent ev);
    protected virtual Task OnPostbackAsync(PostbackEvent ev);
    protected virtual Task OnAccountLinkAsync(AccountLinkEvent ev);
    protected virtual Task OnMemberJoinAsync(MemberJoinEvent ev);
    protected virtual Task OnMemberLeaveAsync(MemberLeaveEvent ev);
    protected virtual Task OnDeviceLinkAsync(DeviceLinkEvent ev);
    protected virtual Task OnDeviceUnlinkAsync(DeviceUnlinkEvent ev);
}
