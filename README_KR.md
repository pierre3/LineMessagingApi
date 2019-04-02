# LINE Messaging API

[![NuGet](https://img.shields.io/nuget/v/Line.Messaging.svg)](https://www.nuget.org/packages/Line.Messaging)
[![NuGet](https://img.shields.io/nuget/dt/Line.Messaging.svg)](https://www.nuget.org/packages/Line.Messaging)

이것은 [LINE Messaging API](https://developers.line.me/messaging-api/overview)의 C#용 구현체입니다.

## 시작하기

이 저장소에는 SDK 본체와 함께 샘플과 Visual Studio용 템플릿이 포함되어 있습니다.

### .Net Standard 클래스 라이브러리

NuGet 관리자를 이용해 프로젝트에 추가할 수 있습니다.

[NuGet 갤러리 | Line.Messaging](https://www.nuget.org/packages/Line.Messaging/)  

### 샘플

SDK를 이용하는 샘플 몇가지가 공개되어 있습니다. 자세한 것은 아래를 참조하시기 바랍니다.

- [Azure Function v1 샘플](https://github.com/pierre3/LineMessagingApi/tree/master/FunctionAppSample)
- [Azure Function v2 샘플](https://github.com/pierre3/LineMessagingApi/tree/master/FunctionAppSample.v2)
- [Web App (API) 샘플](https://github.com/pierre3/LineMessagingApi/tree/master/WebAppSample)
- [Web App (API)와 BotFramework DirectLine 접속 샘플](https://github.com/pierre3/LineMessagingApi/tree/master/WebAppWithBotFrameworkSample)

### Visual Studio 템플릿

템플릿은 마켓플레이스에 공개되어 있습니다만, 내부를 변경하고 싶은 경우를 위해 소스도 공개되어 있습니다.

- [LINE 봇 템플릿 - Visual Studio 마켓플레이스](https://marketplace.visualstudio.com/items?itemName=pierre3.LINEBotCSharpTemplate)
- [Visual Studio 템플릿 프로젝트](https://github.com/pierre3/LineMessagingApi/tree/master/ProjectTemplate)

# 사용방법

다음 3단계를 통해 SDK를 이용하실 수 있습니다.

- LineMessagingClient의 인스턴스화
- WebhookApplication을 상속받는 클래스 작성
- 각 이벤트 발생 시의 로직 구현

## LineMessagingClient 클래스

이 클래스에서 LINE Messaging API 플랫폼과 통신합니다. 내부적으로 HttpClient 기반의 비동기 통신을 이용하고 있으며, 아래와 같은 기능을 제공합니다.

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

## Webhook 이벤트 파싱

GetWebhookEventsAsync 확장 메서드를 호출하여, 요청(리퀘스트)에서 LINE 이벤트를 취득할 수 있습니다. 예시) [FunctionAppSample/HttpTriggerFunction.sc](https://github.com/pierre3/LineMessagingApi/blob/master/FunctionAppSample/HttpTriggerFunction.cs)

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

## Webhook 이벤트 처리

WebhookApplication를 상속받는 클래스를 작성하여, 각종 이벤트 발생 시의 로직을 구현합니다.

```cs
public abstract class WebhookApplication
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

마지막으로 작성한 클래스의 인스턴스를 작성하여, RunAsync 메서드에 파싱한 LINE 이벤트를 넘겨줍니다. 예시) [Line.Messaging/Webhooks/WebhookApplication.cs](https://github.com/pierre3/LineMessagingApi/blob/master/Line.Messaging/Webhooks/WebhookApplication.cs)

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

자세한 것은 각 샘플을 참조하시기 바랍니다.
