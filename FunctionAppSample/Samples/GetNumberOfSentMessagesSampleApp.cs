using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class GetNumberOfSentMessagesSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private TraceWriter Log { get; }

        public GetNumberOfSentMessagesSampleApp(LineMessagingClient lineMessagingClient, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            if (!(ev.Message is TextEventMessage msg)) { return; }
            var messages = new List<ISendMessage>();
            var date1 = new DateTime(2018, 1, 1);
            var date2 = DateTime.Today - TimeSpan.FromDays(2);
            var date3 = DateTime.Today - TimeSpan.FromDays(1);
            var date4 = DateTime.Today;
            NumberOfSentMessages count1 = null, count2 = null, count3 = null, count4 = null;
            bool isDefault = false;
            switch (msg.Text)
            {
                case "Reply":
                    count1 = await MessagingClient.GetNumberOfSentReplyMessagesAsync(date1);
                    count2 = await MessagingClient.GetNumberOfSentReplyMessagesAsync(date2);
                    count3 = await MessagingClient.GetNumberOfSentReplyMessagesAsync(date3);
                    count4 = await MessagingClient.GetNumberOfSentReplyMessagesAsync(date4);
                    break;
                case "Push":
                    await MessagingClient.PushMessageAsync(ev.Source.Id, "This is pushed message.");
                    count1 = await MessagingClient.GetNumberOfSentPushMessagesAsync(date1);
                    count2 = await MessagingClient.GetNumberOfSentPushMessagesAsync(date2);
                    count3 = await MessagingClient.GetNumberOfSentPushMessagesAsync(date3);
                    count4 = await MessagingClient.GetNumberOfSentPushMessagesAsync(date4);
                    break;
                case "Multicast":
                    await MessagingClient.MultiCastMessageAsync(new[] { ev.Source.Id }, "This is sent message by multicast API.");
                    count1 = await MessagingClient.GetNumberOfSentMulticastMessagesAsync(date1);
                    count2 = await MessagingClient.GetNumberOfSentMulticastMessagesAsync(date2);
                    count3 = await MessagingClient.GetNumberOfSentMulticastMessagesAsync(date3);
                    count4 = await MessagingClient.GetNumberOfSentMulticastMessagesAsync(date4);
                    break;
                default:
                    isDefault = true;
                    break;
            }

            if (!isDefault)
            {
                var message =
    $@"Number of ""{msg.Text}"" messages.
{date1.ToString("yyyy/MM/dd")}: ({count1.Status}){count1.Success}
{date2.ToString("yyyy/MM/dd")}: ({count2.Status}){count1.Success}
{date3.ToString("yyyy/MM/dd")}: ({count3.Status}){count1.Success}
{date4.ToString("yyyy/MM/dd")}: ({count4.Status}){count1.Success}";
                messages.Add(new TextMessage(message));
            }
            messages.Add(new TemplateMessage("sampleTemplate",
                new ButtonsTemplate(
                    text: "Select the type of message.",
                    actions: new[] {
                        new MessageTemplateAction("Num of reply","Reply"),
                        new MessageTemplateAction("Num of push", "Push"),
                        new MessageTemplateAction("Num of multicast","Multicast")
                    })));
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, messages);
        }
    }
}
