using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class PostbackMessageSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }
        private TableStorage<BotStatus> SourceLocation { get; }
        private TraceWriter Log { get; }

        public PostbackMessageSampleApp(LineMessagingClient lineMessagingClient, TableStorage<BotStatus> tableStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            SourceLocation = tableStorage;
            Log = log;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            var state = await SourceLocation.FindAsync(ev.Source.Type.ToString(), ev.Source.Id);
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    var text = ((TextEventMessage)ev.Message).Text;
                    if (state?.Location != null || text != "use the text parameter")
                    {
                        await ConfirmMapSearchAsync(ev.ReplyToken, state.Location, text);
                        break;
                    }
                    await EchoAsync(ev.ReplyToken, text);
                    break;


                case EventMessageType.Location:
                    await SaveLocationAsync(ev, (LocationEventMessage)ev.Message);
                    break;

            }
        }

        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}");

            var data = JsonConvert.DeserializeAnonymousType(ev.Postback.Data, new { type = "", searchWord = "", location = "" });
            var searchword = Uri.EscapeUriString(Regex.Replace(data.searchWord, "\\s", "+"));
            if (data.type == "keyword")
            {
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"https://www.google.co.jp/maps/search/?api=1&query={data.searchWord}&query={data.location}");
            }
            else
            {
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"https://www.google.co.jp/maps/dir/?api=1&origin={data.location}&destination={searchword}");
            }

            await SourceLocation.UpdateAsync(new BotStatus()
            {
                SourceType = ev.Source.Type.ToString(),
                SourceId = ev.Source.Id,
                Location = null
            });
        }

        private Task EchoAsync(string replyToken, string userMessage)
        {
            return MessagingClient.ReplyMessageAsync(replyToken, userMessage);
        }

        private async Task SaveLocationAsync(MessageEvent ev, LocationEventMessage locMessage)
        {
            await SourceLocation.UpdateAsync(
                new BotStatus()
                {
                    SourceType = ev.Source.Type.ToString(),
                    SourceId = ev.Source.Id,
                    Location = $"{locMessage.Latitude},{locMessage.Longitude}"
                });
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "Enter a search word for google map search.");
        }

        private async Task ConfirmMapSearchAsync(string replyToken, string location, string searchWord)
        {
            var templateMessage = new TemplateMessage("Google map search",
                new ConfirmTemplate($"Select a search type.",
                    new[]
                    {
                        new PostbackTemplateAction("Keyword",JsonConvert.SerializeObject(
                            new {
                                type = "keyword",
                                searchWord,
                                location
                            }),"use the displayText parameter",useDisplayText:true),
                        new PostbackTemplateAction("Route",JsonConvert.SerializeObject(
                            new {
                                type = "route",
                                searchWord,
                                location
                            }),"use the text parameter",useDisplayText:false)
                    }));
            await MessagingClient.ReplyMessageAsync(replyToken, new[] { templateMessage });
        }


    }
}
