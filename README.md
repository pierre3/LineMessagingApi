# LINE Messaging API

[![NuGet](https://img.shields.io/nuget/v/Line.Messaging.svg)](https://www.nuget.org/packages/Line.Messaging)
[![NuGet](https://img.shields.io/nuget/dt/Line.Messaging.svg)](https://www.nuget.org/packages/Line.Messaging)

[日本語の説明はこちら](./README_JP.md)

This is a C# implementation of the [LINE Messaging API](https://developers.line.me/messaging-api/overview).

## Getting Started
This repository contains SDK itself, as well as base samples and Visual Studio templates.

### .Net Standard Class Library   
  Use NuGet manager to import the library to your project.
[NuGet Gallery | Line.Messaging](https://www.nuget.org/packages/Line.Messaging/)  

### Samples
There are several samples which uses the SDK. You can find detail instructions in each directory.
- [Azure Function v1 Sample](https://github.com/pierre3/LineMessagingApi/tree/master/FunctionAppSample)
- [Azure Function v2 Sample](https://github.com/pierre3/LineMessagingApi/tree/master/FunctionAppSample.v2)
- [Web App (API) Sample](https://github.com/pierre3/LineMessagingApi/tree/master/WebAppSample)
- [Web App (API) to BotFramework DirectLine Sample](https://github.com/pierre3/LineMessagingApi/tree/master/WebAppWithBotFrameworkSample)

### Visual Studio Templates  
The template can be found in Market Place, but if you want to tweak it, the source is also available.
- [Line Bot Templates - Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=pierre3.LINEBotCSharpTemplate)
- [Visual Studio Templates Project](https://github.com/pierre3/LineMessagingApi/tree/master/ProjectTemplate)

# Usage
Basically, there are three steps to use the SDK.
  - Instantiate LineMessagingClient.
  - Implement a class which inherits WebhookApplication.
  - Override the method to handle the event.

## LineMessagingClient Class

This is a class to communicate with LINE Messaging API platform. It uses HttpClient-based asynchronous methods such as followings.
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
Task LeaveFromGroupAsync(string groupId)
Task LeaveFromRoomAsync(string roomId)
```

## Parse and process Webhook-Events
Use GetWebhookEventsAsync extension method for incoming request to parse the LINE events from the LINE platform. See [FunctionAppSample/HttpTriggerFunction.sc](https://github.com/pierre3/LineMessagingApi/blob/master/FunctionAppSample/HttpTriggerFunction.cs) as an example.

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
        //Process the webhook-events
        var app = new LineBotApp(lineMessagingClient, tableStorage, blobStorage, log);
        await app.RunAsync(events);
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
## Process Webhook-events
Create a class which inherits WebhookApplication class, then overrides the method you want to handle the LINE evnet in your class.

```cs
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
```

Finally, instantiate the class and run RunAsync method by giving the parsed LINE events as shown above. 

See [Line.Messaging/Webhooks/WebhookApplication.cs](https://github.com/pierre3/LineMessagingApi/blob/master/Line.Messaging/Webhooks/WebhookApplication.cs) as processing event class. 


```cs
class LineBotApp : WebhookApplication
{
  private LineMessagingClient MessagingClient { get; }
  private TraceWriter Log { get; }
  
  public WebhookApplication(LineMessagingClient lineMessagingClient,TraceWriter log)
  {
      MessagingClient = lineMessagingClient;
      Log = log;
  }

  protected override async Task OnMessageAsync(MessageEvent ev)
  {
    Log.Info($"SourceType:{ev.Source.Type},SourceId:{ev.Source.Id}");
    switch (ev.Message)
    {
      case TextEventMessage textMessage:
        await MessagingClient.ReplyMessageAsync(ev.ReplyToken, textMessage.Text);
        break;
      case ImageEventMessage imageMessage:
        //...
        break;
      case AudioEventMessage audioEventMessage:
        //...
        break;
      case VideoEventMessage videoMessage:
        //...
        break;
      case FileEventMessage fileMessage:
        //...
        break;
      case LocationEventMessage locationMessage:
        //...
        break;
      case StickerEventMessage stickerMessage:
        //...         
        break;
    }
  }
}
```
See each samples for more detail.
