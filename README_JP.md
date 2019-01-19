# LINE Messaging API

[![NuGet](https://img.shields.io/nuget/v/Line.Messaging.svg)](https://www.nuget.org/packages/Line.Messaging)
[![NuGet](https://img.shields.io/nuget/dt/Line.Messaging.svg)](https://www.nuget.org/packages/Line.Messaging)

[LINE Messaging API](https://developers.line.me/messaging-api/overview) の C# 用 SDK 実装です。

## はじめに
このレポジトリには SDK 本体だけでなく、サンプルや Visual Studio 用の各種テンプレートが含まれます。

### .Net Standard クラスライブラリ   
NuGet マネージャーなどでプロジェクトに参照可能です。

[NuGet ギャラリー | Line.Messaging](https://www.nuget.org/packages/Line.Messaging/)  

### サンプル
SDK を利用するサンプルをいくつか公開しています。詳細はリンク先を参照してください。
- [Azure ファンクション バージョン 1 サンプル](https://github.com/pierre3/LineMessagingApi/tree/master/FunctionAppSample)
- [Azure ファンクション バージョン 2 サンプル](https://github.com/pierre3/LineMessagingApi/tree/master/FunctionAppSample.v2)
- [Web App (API) サンプル](https://github.com/pierre3/LineMessagingApi/tree/master/WebAppSample)
- [Web App (API) と BotFramework DirectLine 接続のサンプル](https://github.com/pierre3/LineMessagingApi/tree/master/WebAppWithBotFrameworkSample)

### Visual Studio テンプレート  
テンプレートはマーケットプレースに公開済みですが、中身を変更したい場合はソースも公開しています。
- [LINE ボットテンプレート - Visual Studio マーケットプレース](https://marketplace.visualstudio.com/items?itemName=pierre3.LINEBotCSharpTemplate)
- [Visual Studio テンプレートプロジェクト](https://github.com/pierre3/LineMessagingApi/tree/master/ProjectTemplate)

# 利用方法
以下の 3 ステップで SDK を利用します。
  - LineMessagingClient のインスタンス作成
  - WebhookApplication を継承したクラスの作成
  - 各イベント発生時のロジックを実装

## LineMessagingClient クラス

このクラスで LINE Messaging API プラットフォームと通信します。内部で HttpClient ベースの非同期通信を利用しており、以下のような機能を提供します。
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

## Webhook イベントのパース
GetWebhookEventsAsync 拡張メソッドを呼び出して、要求から LINE イベントを取得できます。例) [FunctionAppSample/HttpTriggerFunction.sc](https://github.com/pierre3/LineMessagingApi/blob/master/FunctionAppSample/HttpTriggerFunction.cs) 

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
## Webhook イベントのハンドル
WebhookApplication を継承したクラスを作成し、各種イベント発生時のロジックを実装します。

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

最後に作成したクラスのインスタンスを作成し、RunAsync メソッドに対してパースした LINE イベントを渡します。

例) [Line.Messaging/Webhooks/WebhookApplication.cs](https://github.com/pierre3/LineMessagingApi/blob/master/Line.Messaging/Webhooks/WebhookApplication.cs) 




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
各サンプルでより詳細が確認できます。
