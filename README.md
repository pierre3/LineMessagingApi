# LINE Messaging API

This is a C# implementation of the [LINE Messaging API](https://developers.line.me/messaging-api/overview).

## Getting Started
- .Net Standard Class Library   
[NuGet Gallery | Line.Messaging 0.2.0-alpha](https://www.nuget.org/packages/Line.Messaging/0.2.0-alpha)
- Azure Function Project Template for Visual Studio 2017  
[Line Bot Function - Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=pierre3.LineBotFunction)

## LineMessagingApiClient Class

HttpClient-based asynchronous methods.
```cs
Task ReplyMessageAsync(string replyToken, IList<ISendMessage> messages)
Task PushMessageAsync(string to, IList<ISendMessage> messages)
Task MultiCastMessageAsync(IList<string> to, IList<ISendMessage> messages)
Task<UserProfile> GetUserProfileAsync(string userId)
Task<Stream> GetContentStreamAsync(string messageId)
Task<byte[]> GetContentBytesAsync(string messageId)
Task<UserProfile> GetGroupMemberProfileAsync(string groupId, string userId)
Task<UserProfile> GetRoomMemberProfileAsync(string roomId, string userId)
Task<GroupMemberIds> GetGroupMemberIdsAsync(string groupId, string continuationToken = null)
Task<GroupMemberIds> GetRoomMemberIdsAsync(string roomId, string continuationToken = null)
Task ReaveFromGroupAsync(string groupId)
Task ReaveFromRoomAsync(string roomId)
```


## Examples 

Examples of use in Azure functions. 

```cs
using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //Initialize a LINE Messaging API client
            var channelAccessToken = System.Configuration.ConfigurationManager.AppSettings["ChannelAccessToken"];
            lineMessagingClient = new LineMessagingClient(channelAccessToken);
            var sp = ServicePointManager.FindServicePoint(new Uri("https://api.line.me"));
            sp.ConnectionLeaseTimeout = 60 * 1000;
        }

        [FunctionName("LineMessagingApiSample")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            //Get Webhook events from request body.
            IEnumerable<WebhookEvent> events;
            try
            {
                var channelAccessToken = System.Configuration.ConfigurationManager.AppSettings["ChannelSecret"];
                events = await req.GetWebhookEventsAsync(channelAccessToken);
            }
            catch (InvalidSignatureException e)
            {
                //Signature validation failed.
                return req.CreateResponse(HttpStatusCode.Forbidden, new { Message = e.Message });
            }
            
            //Process message events 
            foreach (var ev in events.OfType<MessageEvent>())
            {
                switch (ev.Message.Type)
                {
                    case EventMessageType.Text:
                        var textMessage = (TextEventMessage)ev.Message;
                        await lineMessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { new TextMessage(textMessage.Text) });
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

            return req.CreateResponse(HttpStatusCode.OK);
        }

    }
}
```
