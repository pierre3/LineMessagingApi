# LINE Messaging API

This is a C# implementation of the [LINE Messaging API](https://developers.line.me/messaging-api/overview).

## Getting Started
- .Net Standard Class Library   
[NuGet Gallery | Line.Messaging 0.3.0-alpha](https://www.nuget.org/packages/Line.Messaging/0.3.0-alpha)
- Azure Function Project Template for Visual Studio 2017  
[Line Bot Function - Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=pierre3.LineBotFunction)

## LineMessagingApiClient Class

HttpClient-based asynchronous methods.
```cs
Task ReplyMessageAsync(string replyToken, IList<ISendMessage> messages)
Task ReplyMessageAsync(string replyToken, params string[] messages)
Task PushMessageAsync(string to, IList<ISendMessage> messages)
Task PushMessageAsync(string to, params string[] messages)
Task MultiCastMessageAsync(IList<string> to, IList<ISendMessage> messages)
Task MultiCastMessageAsync(IList<string> to, params string[] messages)
Task<ContentStream> GetContentStreamAsync(string messageId)
Task<UserProfile> GetUserProfileAsync(string userId)
Task<byte[]> GetContentBytesAsync(string messageId)
Task<UserProfile> GetGroupMemberProfileAsync(string groupId, string userId)
Task<UserProfile> GetRoomMemberProfileAsync(string roomId, string userId)
Task<IList<UserProfile>> GetGroupMemberProfilesAsync(string groupId)
Task<IList<UserProfile>> GetRoomMemberProfilesAsync(string roomId)
Task<GroupMemberIds> GetGroupMemberIdsAsync(string groupId, string continuationToken)
Task<GroupMemberIds> GetRoomMemberIdsAsync(string roomId, string continuationToken = null)
Task ReaveFromGroupAsync(string groupId)
Task ReaveFromRoomAsync(string roomId)
```


## Examples 

Examples of use in Azure functions. 
- [FunctionAppSample](https://github.com/pierre3/LineMessagingApi/blob/master/FunctionAppSample)

### Parse and Dispatch Webhook-Events

see [FunctionAppSample/HttpTriggerFunction.sc](https://github.com/pierre3/LineMessagingApi/blob/master/FunctionAppSample/HttpTriggerFunction.cs)

```cs
using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionAppSample
{


  public static class HttpTriggerFunction
  {
    static LineMessagingClient lineMessagingClient;

    static HttpTriggerFunction()
    {
      lineMessagingClient = new LineMessagingClient(System.Configuration.ConfigurationManager.AppSettings["ChannelAccessToken"]);
      var sp = ServicePointManager.FindServicePoint(new Uri("https://api.line.me"));
      sp.ConnectionLeaseTimeout = 60 * 1000;
    }

    [FunctionName("LineMessagingApiSample")]
    public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
    {
      IEnumerable<WebhookEvent> events;
      try
      {
        //Parse Webhook-Events
        var channelSecret = System.Configuration.ConfigurationManager.AppSettings["ChannelSecret"];
        events = await req.GetWebhookEventsAsync(channelSecret);
      }
      catch (InvalidSignatureException e)
      {
        //Signature validation failed
        return req.CreateResponse(HttpStatusCode.Forbidden, new { Message = e.Message });
      }

      try
      {
        var connectionString = System.Configuration.ConfigurationManager.AppSettings["AzureWebJobsStorage"];
        var tableStorage = await LineBotTableStorage.CreateAsync(connectionString);
        var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");
        
        var dispatcher = new LineBotEventDispatcher(lineMessagingClient, tableStorage, blobStorage, log);
        await dispatcher.DispatchAsync(events);
      }
      catch (Exception e)
      {
        log.Error(e.ToString());
      }

      return req.CreateResponse(HttpStatusCode.OK);
    }
  }

}
```
### Dispatch Webhook-Events using the class that inherited the WebhookEventDespatcher class
        
[Line.Messaging/Webhooks/WebhookEventDispatcher.cs](https://github.com/pierre3/LineMessagingApi/blob/master/Line.Messaging/Webhooks/WebhookEventDispatcher.cs)   

```cs
public abstract class WebhookEventDispatcher
{
  protected virtual Task OnMessageAsync(MessageEvent ev);
  protected virtual Task OnJoinAsync(JoinEvent ev);
  protected virtual Task OnLeaveAsync(LeaveEvent ev);
  protected virtual Task OnFollowAsync(FollowEvent ev);
  protected virtual Task OnUnfollowAsync(UnfollowEvent ev);
  protected virtual Task OnBeaconAsync(BeaconEvent ev);
  protected virtual Task OnPostbackAsync(PostbackEvent ev);
}
```


```cs
class MyWebhookEventDispatcher : WebhookEventDispatcher
{
  private LineMessagingClient MessagingClient { get; }
  private TraceWriter Log { get; }
  
  public MyWebhookEventDispatcher(LineMessagingClient lineMessagingClient,TraceWriter log)
  {
      MessagingClient = lineMessagingClient;
      Log = log;
  }

  protected override async Task OnMessageAsync(MessageEvent ev)
  {
    Log.Info($"SourceType:{ev.Source.Type},SourceId:{ev.Source.Id}");
    switch (ev.Message.Type)
    {
      case EventMessageType.Text:
        await MessagingClient.ReplyMessageAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text);
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
      throw new NotImplementedException();
  }

  protected override async Task OnUnfollowAsync(UnfollowEvent ev)
  {
      throw new NotImplementedException();
  }

  protected override async Task OnJoinAsync(JoinEvent ev)
  {
    throw new NotImplementedException();
  }

  protected override async Task OnLeaveAsync(LeaveEvent ev)
  {
    throw new NotImplementedException();
  }

  protected override Task OnBeaconAsync(BeaconEvent ev)
  {
    throw new NotImplementedException();
  }

  protected override async Task OnPostbackAsync(PostbackEvent ev)
  {
    throw new NotImplementedException();
  }

}
```

see also [FunctionAppSample/LineBotEventDispatcher.cs](https://github.com/pierre3/LineMessagingApi/blob/master/FunctionAppSample/LineBotEventDispatcher.cs)
