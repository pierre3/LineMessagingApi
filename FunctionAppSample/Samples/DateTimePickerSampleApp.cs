using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class DateTimePickerSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }
        private TraceWriter Log { get; }

        public DateTimePickerSampleApp(LineMessagingClient lineMessagingClient, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, Id:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            var template = new TemplateMessage("DataTimePickerTest",
                new ButtonsTemplate(
                    text: "DateTimePicker TEST",
                    thumbnailImageUrl: null,
                    title: "Select a date or time.",
                    actions: new[] {
                        new DateTimePickerTemplateAction("Date","Date", DateTimePickerMode.Date, DateTime.Today, new DateTime(2000,1,1), new DateTime(2020,12,31)),
                        new DateTimePickerTemplateAction("Time","Time", DateTimePickerMode.Time, DateTime.Now.ToString("HH:mm"), "00:00", "23:59"),
                        new DateTimePickerTemplateAction("DatetTime","DateTime", DateTimePickerMode.Datetime, DateTime.Now, new DateTime(2000,1,1,0,0,0), new DateTime(2020,12,31,23,59,59))
                    }));
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { template });
        }



        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, EntryId:{ev.Source.Id}");

            switch (ev.Postback.Data)
            {
                case "Date":
                    await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "You chose the date: " + ev.Postback.Params.Date);
                    break;
                case "Time":
                    await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "You chose the time: " + ev.Postback.Params.Time);
                    break;
                case "DateTime":
                    await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "You chose the date-time: " + ev.Postback.Params.DateTime);
                    break;
            }

        }

    }

}
