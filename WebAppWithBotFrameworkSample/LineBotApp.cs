using Line.Messaging;
using Line.Messaging.Webhooks;
using WebAppWithBotFrameworkSample.CloudStorage;
using WebAppWithBotFrameworkSample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using dl = Microsoft.Bot.Connector.DirectLine;
using System.Configuration;
using Microsoft.Bot.Connector.DirectLine;
using WebAppWithBotFrameworkSample.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebAppWithBotFrameworkSample
{
    internal class LineBotApp : WebhookApplication
    {       
        private static string directLineSecret = ConfigurationManager.AppSettings["DirectLineSecret"].ToString();
        private LineMessagingClient messagingClient { get; }
        private DirectLineClient dlClient;
        private string conversationId; // DirectLine ConversationId
        private string watermark; // Limit the messages to get from DirectLine
        private Dictionary<string, object> userParams;
        private TableStorage<EventSourceState> sourceState { get; }
        private BlobStorage blobStorage { get; }

        public LineBotApp(LineMessagingClient lineMessagingClient, TableStorage<EventSourceState> tableStorage, BlobStorage blobStorage)
        {
            this.messagingClient = lineMessagingClient;
            this.sourceState = tableStorage;
            this.blobStorage = blobStorage;
            try
            {
                dlClient = new DirectLineClient(directLineSecret);
            }
            catch (Exception)
            { }
        }

        private async Task Initialize(MessageEvent ev)
        {
            var lineId = ev.Source.Id;

            if (CacheService.caches.Keys.Contains(lineId))
            {
                // Get preserved ConversationId and Watermark from cache.
                // If we scale out, then we have to use different method
                userParams = CacheService.caches[lineId] as Dictionary<string, object>;
                conversationId = userParams.Keys.Contains("ConversationId") ? userParams["ConversationId"].ToString() : "";
                watermark = userParams.Keys.Contains("Watermark") ? userParams["Watermark"].ToString() : null;
            }
            else
            {
                // If no cache, then create new one.
                userParams = new Dictionary<string, object>();
                var conversation = await dlClient.Conversations.StartConversationAsync();
                userParams["ConversationId"] = conversationId = conversation.ConversationId;
                CacheService.caches[lineId] = userParams;
                watermark = null;
            }
        }

        #region Handlers

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            await Initialize(ev);
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    await HandleTextAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text, ev.Source.UserId);
                    break;
                case EventMessageType.Image:
                case EventMessageType.Audio:
                case EventMessageType.Video:
                case EventMessageType.File:
                    // Prepare blob directory name for binary object.
                    var blobDirectoryName = ev.Source.Type + "_" + ev.Source.Id;
                    await HandleMediaAsync(ev.ReplyToken, ev.Message.Id, ev.Source.UserId);
                    break;
                case EventMessageType.Location:
                    var location = ((LocationEventMessage)ev.Message);
                    await HandleLocationAsync(ev.ReplyToken, location, ev.Source.Id);
                    break;
                case EventMessageType.Sticker:
                    await HandleStickerAsync(ev.ReplyToken);
                    break;
            }
        }
        
        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            string text = "";
            switch (ev.Postback.Data)
            {
                case "Date":
                    text = "You chose the date: " + ev.Postback.Params.Date;
                    break;
                case "Time":
                    text = "You chose the time: " + ev.Postback.Params.Time;
                    break;
                case "DateTime":
                    text = "You chose the date-time: " + ev.Postback.Params.DateTime;
                    break;
                default:
                    text = "Your postback is " + ev.Postback.Data;
                    break;
            }

            dl.Activity sendMessage = new dl.Activity()
            {
                Type = "message",
                Text = text,
                From = new dl.ChannelAccount(ev.Source.Id, ev.Source.Id)
            };

            // Send the message, then fetch and reply messages,
            await dlClient.Conversations.PostActivityAsync(conversationId, sendMessage);
            await GetAndReplyMessages(ev.ReplyToken, ev.Source.Id);
        }
        
        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            // Store source information which follows the bot.
            await sourceState.AddAsync(ev.Source.Type.ToString(), ev.Source.Id);

            var userName = "";
            if (!string.IsNullOrEmpty(ev.Source.Id))
            {
                var userProfile = await messagingClient.GetUserProfileAsync(ev.Source.Id);
                userName = userProfile?.DisplayName ?? "";
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"Hello {userName}! Thank you for following !");
        }
        
        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {
            // Remote source information which unfollows the bot.
            await sourceState.DeleteAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your {ev.Source.Type.ToString().ToLower()}!");
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            await sourceState.DeleteAsync(ev.Source.Type.ToString(), ev.Source.Id);
        }

        protected override async Task OnBeaconAsync(BeaconEvent ev)
        {
            var message = "";
            switch (ev.Beacon.Type)
            {
                case BeaconType.Enter:
                    message = "You entered the beacon area!";
                    break;
                case BeaconType.Leave:
                    message = "You leaved the beacon area!";
                    break;
                case BeaconType.Banner:
                    message = "You tapped the beacon banner!";
                    break;
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"{message}(Dm:{ev.Beacon.Dm}, Hwid:{ev.Beacon.Hwid})");
        }

        #endregion

        private async Task HandleTextAsync(string replyToken, string userMessage, string userId)
        {
            dl.Activity sendMessage = new dl.Activity()
            {
                Type = "message",
                Text = userMessage,
                From = new dl.ChannelAccount(userId, userId)
            };

            // Send the message, then fetch and reply messages,
            try
            {
                await dlClient.Conversations.PostActivityAsync(conversationId, sendMessage);
            }
            catch (Exception)
            {

            }

            await GetAndReplyMessages(replyToken, userId);
        }

        /// <summary>
        /// Upload the received data to blob and returns the address
        /// </summary>
        private async Task HandleMediaAsync(string replyToken, string messageId, string userId)
        {
            var stream = await messagingClient.GetContentStreamAsync(messageId);
            var ext = GetFileExtension(stream.ContentHeaders.ContentType.MediaType);
            await dlClient.Conversations.UploadAsync(conversationId, stream, userId, stream.ContentHeaders.ContentType.MediaType);
            await GetAndReplyMessages(replyToken, userId);
        }

        /// <summary>
        /// Reply the location user send.
        /// </summary>
        private async Task HandleLocationAsync(string replyToken, LocationEventMessage location, string userId)
        {
            dl.Activity sendMessage = new dl.Activity()
            {
                Type = "message",
                Text = location.Title,
                From = new dl.ChannelAccount(userId, userId),
                Entities = new List<Entity>()
                {
                    new Entity()
                    {
                        Type = "Place",
                        Properties = JObject.FromObject(new Place(address:location.Address,
                            geo:new dl.GeoCoordinates(
                                latitude: (double)location.Latitude,
                                longitude: (double)location.Longitude,
                                name: location.Title),
                            name:location.Title))
                    }
                }
            };

            // Send the message, then fetch and reply messages,
            await dlClient.Conversations.PostActivityAsync(conversationId, sendMessage);
            await GetAndReplyMessages(replyToken, userId);            
        }

        /// <summary>
        /// Replies random sticker
        /// Sticker ID of bssic stickers (packge ID =1)
        /// see https://devdocs.line.me/files/sticker_list.pdf
        /// </summary>
        private async Task HandleStickerAsync(string replyToken)
        {
            var stickerids = Enumerable.Range(1, 17)
                .Concat(Enumerable.Range(21, 1))
                .Concat(Enumerable.Range(100, 139 - 100 + 1))
                .Concat(Enumerable.Range(401, 430 - 400 + 1)).ToArray();

            var rand = new Random(Guid.NewGuid().GetHashCode());
            var stickerId = stickerids[rand.Next(stickerids.Length - 1)].ToString();
            await messagingClient.ReplyMessageAsync(replyToken, new[] {
                        new StickerMessage("1", stickerId)
                    });
        }

        /// <summary>
        /// Get all messages from DirectLine and reply back to Line
        /// </summary>
        private async Task GetAndReplyMessages(string replyToken, string userId)
        {
            dl.ActivitySet result = string.IsNullOrEmpty(watermark) ?
                await dlClient.Conversations.GetActivitiesAsync(conversationId) :
                await dlClient.Conversations.GetActivitiesAsync(conversationId, watermark);

            userParams["Watermark"] = (Int64.Parse(result.Watermark)).ToString();

            foreach (var activity in result.Activities)
            {
                if (activity.From.Id == userId)
                    continue;

                List<ISendMessage> messages = new List<ISendMessage>();

                if (activity.Attachments != null && activity.Attachments.Count != 0 && (activity.AttachmentLayout == null || activity.AttachmentLayout == "list"))
                {
                    foreach (var attachment in activity.Attachments)
                    {
                        if (attachment.ContentType.Contains("card.animation"))
                        {
                            // https://docs.botframework.com/en-us/core-concepts/reference/#animationcard
                            // Use TextMessage for title and use Image message for image. Not really an animation though.
                            AnimationCard card = JsonConvert.DeserializeObject<AnimationCard>(attachment.Content.ToString());
                            messages.Add(new TextMessage($"{card.Title}\r\n{card.Subtitle}\r\n{card.Text}"));
                            foreach (var media in card.Media)
                            {
                                var originalContentUrl = media.Url?.Replace("http://", "https://");
                                var previewImageUrl = card.Image?.Url?.Replace("http://", "https://");
                                messages.Add(new ImageMessage(originalContentUrl, previewImageUrl));
                            }
                        }
                        else if (attachment.ContentType.Contains("card.audio"))
                        {
                            // https://docs.botframework.com/en-us/core-concepts/reference/#audiocard
                            // Use TextMessage for title and use Audio message for image.
                            AudioCard card = JsonConvert.DeserializeObject<AudioCard>(attachment.Content.ToString());
                            messages.Add(new TextMessage($"{card.Title}\r\n{card.Subtitle}\r\n{card.Text}"));

                            foreach (var media in card.Media)
                            {
                                var originalContentUrl = media.Url?.Replace("http://", "https://");
                                var durationInMilliseconds = 1;

                                messages.Add(new AudioMessage(originalContentUrl, (long)durationInMilliseconds));
                            }
                        }
                        else if (attachment.ContentType.Contains("card.hero") || attachment.ContentType.Contains("card.thumbnail"))
                        {
                            // https://docs.botframework.com/en-us/core-concepts/reference/#herocard
                            // https://docs.botframework.com/en-us/core-concepts/reference/#thumbnailcard
                            HeroCard hcard = null;

                            if (attachment.ContentType.Contains("card.hero"))
                                hcard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());
                            else if (attachment.ContentType.Contains("card.thumbnail"))
                            {
                                ThumbnailCard tCard = JsonConvert.DeserializeObject<ThumbnailCard>(attachment.Content.ToString());
                                hcard = new HeroCard(tCard.Title, tCard.Subtitle, tCard.Text, tCard.Images, tCard.Buttons, null);
                            }

                            // Get four buttons per template.
                            for (int i = 0; i < (double)hcard.Buttons.Count / 4; i++)
                            {
                                ButtonsTemplate buttonsTemplate = new ButtonsTemplate(
                                  title: string.IsNullOrEmpty(hcard.Title) ? hcard.Text : hcard.Title,
                                thumbnailImageUrl: hcard.Images?.FirstOrDefault()?.Url?.Replace("http://", "https://"),
                                text: hcard.Subtitle ?? hcard.Text
                                );

                                if (hcard.Buttons != null)
                                {
                                    foreach (var button in hcard.Buttons.Skip(i * 4).Take(4))
                                    {
                                        buttonsTemplate.Actions.Add(GetAction(button));
                                    }
                                }
                                else
                                {
                                    // Action is mandatory, so create from title/subtitle.
                                    var actionLabel = hcard.Title?.Length < hcard.Subtitle?.Length ? hcard.Title : hcard.Subtitle;
                                    actionLabel = actionLabel.Substring(0, Math.Min(actionLabel.Length, 20));
                                    buttonsTemplate.Actions.Add(new PostbackTemplateAction(actionLabel, actionLabel, actionLabel));
                                }

                                messages.Add(new TemplateMessage("Buttons template", buttonsTemplate));
                            }
                        }
                        else if (attachment.ContentType.Contains("receipt"))
                        {
                            // https://docs.botframework.com/en-us/core-concepts/reference/#receiptcard
                            // Use TextMessage and Buttons. As LINE doesn't support thumbnail type yet.

                            ReceiptCard card = JsonConvert.DeserializeObject<ReceiptCard>(attachment.Content.ToString());
                            var text = card.Title + "\r\n\r\n";
                            foreach (var fact in card.Facts)
                            {
                                text += $"{fact.Key}:{fact.Value}\r\n";
                            }
                            text += "\r\n";
                            foreach (var item in card.Items)
                            {
                                text += $"{item.Title}\r\nprice:{item.Price},quantity:{item.Quantity}";
                            }

                            messages.Add(new TextMessage(text));

                            ButtonsTemplate buttonsTemplate = new ButtonsTemplate(
                                text: $"total:{card.Total}", title: $"tax:{card.Tax}");

                            foreach (var button in card.Buttons)
                            {
                                buttonsTemplate.Actions.Add(GetAction(button));
                            }

                            messages.Add(new TemplateMessage("Buttons template", buttonsTemplate));
                        }
                        else if (attachment.ContentType.Contains("card.signin"))
                        {
                            // https://docs.botframework.com/en-us/core-concepts/reference/#signincard
                            // Line doesn't support auth button yet, so simply represent link.
                            SigninCard card = JsonConvert.DeserializeObject<SigninCard>(attachment.Content.ToString());

                            ButtonsTemplate buttonsTemplate = new ButtonsTemplate(text: card.Text);

                            foreach (var button in card.Buttons)
                            {
                                buttonsTemplate.Actions.Add(GetAction(button));
                            }

                            messages.Add(new TemplateMessage("Buttons template", buttonsTemplate));
                        }
                        else if (attachment.ContentType.Contains("card.video"))
                        {
                            // https://docs.botframework.com/en-us/core-concepts/reference/#videocard
                            // Use Video message for video and buttons for action.

                            VideoCard card = JsonConvert.DeserializeObject<VideoCard>(attachment.Content.ToString());

                            foreach (var media in card.Media)
                            {
                                var originalContentUrl = media?.Url?.Replace("http://", "https://");
                                var previewImageUrl = card.Image?.Url?.Replace("http://", "https://");

                                messages.Add(new VideoMessage(originalContentUrl, previewImageUrl));
                            }

                            ButtonsTemplate buttonsTemplate = new ButtonsTemplate(
                                title: card.Title, text: $"{card.Subtitle}\r\n{card.Text}");
                            foreach (var button in card.Buttons)

                            {
                                buttonsTemplate.Actions.Add(GetAction(button));
                            }
                            messages.Add(new TemplateMessage("Buttons template", buttonsTemplate));
                        }
                        else if (attachment.ContentType.Contains("image"))
                        {
                            var originalContentUrl = attachment.ContentUrl?.Replace("http://", "https://");
                            var previewImageUrl = string.IsNullOrEmpty(attachment.ThumbnailUrl) ? attachment.ContentUrl?.Replace("http://", "https://") : attachment.ThumbnailUrl?.Replace("http://", "https://");

                            messages.Add(new ImageMessage(originalContentUrl, previewImageUrl));
                        }
                        else if (attachment.ContentType.Contains("audio"))
                        {
                            var originalContentUrl = attachment.ContentUrl?.Replace("http://", "https://");
                            var durationInMilliseconds = 0;

                            messages.Add(new AudioMessage(originalContentUrl, durationInMilliseconds));
                        }
                        else if (attachment.ContentType.Contains("video"))
                        {
                            var originalContentUrl = attachment.ContentUrl?.Replace("http://", "https://");
                            var previewImageUrl = attachment.ThumbnailUrl?.Replace("http://", "https://");

                            messages.Add(new VideoMessage(originalContentUrl, previewImageUrl));
                        }
                    }
                }
                else if (activity.Attachments != null && activity.Attachments.Count != 0 && activity.AttachmentLayout != null)
                {
                    CarouselTemplate carouselTemplate = new CarouselTemplate();

                    foreach (var attachment in activity.Attachments)
                    {
                        HeroCard hcard = null;

                        if (attachment.ContentType == "application/vnd.microsoft.card.hero")
                            hcard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());
                        else if (attachment.ContentType == "application/vnd.microsoft.card.thumbnail")
                        {
                            ThumbnailCard tCard = JsonConvert.DeserializeObject<ThumbnailCard>(attachment.Content.ToString());
                            hcard = new HeroCard(tCard.Title, tCard.Subtitle, tCard.Text, tCard.Images, tCard.Buttons, null);
                        }
                        else
                            continue;

                        CarouselColumn tColumn = new CarouselColumn(
                            title: hcard.Title,
                            thumbnailImageUrl: hcard.Images.FirstOrDefault()?.Url?.Replace("http://", "https://"),
                            text: string.IsNullOrEmpty(hcard.Subtitle) ? 
                                hcard.Text : hcard.Subtitle);

                        if (hcard.Buttons != null)
                        {
                            foreach (var button in hcard.Buttons.Take(3))
                            {
                                tColumn.Actions.Add(GetAction(button));
                            }
                        }
                        else
                        {
                            // Action is mandatory, so create from title/subtitle.
                            var actionLabel = hcard.Title?.Length < hcard.Subtitle?.Length ? hcard.Title : hcard.Subtitle;
                            actionLabel = actionLabel.Substring(0, Math.Min(actionLabel.Length, 20));
                            tColumn.Actions.Add(new PostbackTemplateAction(actionLabel, actionLabel, actionLabel));
                        }

                        carouselTemplate.Columns.Add(tColumn);
                    }

                    messages.Add(new TemplateMessage("Carousel template", carouselTemplate));
                }
                else if (activity.Entities != null && activity.Entities.Count != 0)
                {
                    foreach (var entity in activity.Entities)
                    {
                        switch (entity.Type)
                        {
                            case "Place":
                                Place place = entity.Properties.ToObject<Place>();
                                GeoCoordinates geo = JsonConvert.DeserializeObject<GeoCoordinates>(place.Geo.ToString());
                                messages.Add(new LocationMessage(place.Name, place.Address.ToString(), (decimal)geo.Latitude, (decimal)geo.Longitude));
                                break;
                            case "GeoCoordinates":
                                GeoCoordinates geoCoordinates = entity.Properties.ToObject<GeoCoordinates>();
                                messages.Add(new LocationMessage(activity.Text, geoCoordinates.Name, (decimal)geoCoordinates.Latitude, (decimal)geoCoordinates.Longitude));
                                break;
                        }
                    }
                }
                else if (activity.ChannelData != null)
                {
                }
                else if (!string.IsNullOrEmpty(activity.Text))
                {
                    if (activity.Text.Contains("\n\n*"))
                    {
                        var lines = activity.Text.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
                        ButtonsTemplate buttonsTemplate = new ButtonsTemplate(text: lines[0]);

                        foreach (var line in lines.Skip(1))
                        {
                            buttonsTemplate.Actions.Add(new PostbackTemplateAction(line, line.Replace("* ", ""), line.Replace("* ", "")));
                        }

                        messages.Add(new TemplateMessage("Buttons template", buttonsTemplate));
                    }
                    else
                        messages.Add(new TextMessage(activity.Text));
                }

                try
                {
                    // If messages contain more than 5 items, then do reply for first 5, then push the rest.
                    for (int i = 0; i < (double)messages.Count / 5; i++)
                    {
                        if (i == 0)
                            await messagingClient.ReplyMessageAsync(replyToken, messages.Take(5).ToList());
                        else
                            await messagingClient.PushMessageAsync(replyToken, messages.Skip(i * 5).Take(5).ToList());
                    }
                }
                catch (LineResponseException ex)
                {
                    if (ex.Message == "Invalid reply token")
                        try
                        {
                            for (int i = 0; i < (double)messages.Count / 5; i++)
                            {
                                await messagingClient.PushMessageAsync(userId, messages.Skip(i * 5).Take(5).ToList());
                            }
                        }
                        catch (LineResponseException innerEx)
                        {
                            await messagingClient.PushMessageAsync(userId, innerEx.Message);
                        }
                }
                catch (Exception ex)
                {
                    await messagingClient.PushMessageAsync(userId, ex.Message);
                }
            }
        }

        /// <summary>
        /// Create TemplateAction from CardAction.
        /// </summary>
        /// <param name="button">CardAction</param>
        /// <returns>TemplateAction</returns>
        private ITemplateAction GetAction(CardAction button)
        {
            switch (button.Type)
            {
                case "openUrl":
                case "playAudio":
                case "playVideo":
                case "showImage":
                case "signin":
                case "downloadFile":
                    return new UriTemplateAction(button.Title.Substring(0, Math.Min(button.Title.Length, 20)), button.Value.ToString());
                case "imBack":
                    return new MessageTemplateAction(button.Title.Substring(0, Math.Min(button.Title.Length, 20)), button.Value.ToString());
                case "postBack":
                    return new PostbackTemplateAction(button.Title.Substring(0, Math.Min(button.Title.Length, 20)), button.Value.ToString(), button.Value.ToString());
                default:
                    return null;
            }
        }

        private string GetFileExtension(string mediaType)
        {
            switch (mediaType)
            {
                case "image/jpeg":
                    return ".jpeg";
                case "audio/x-m4a":
                    return ".m4a";
                case "video/mp4":
                    return ".mp4";
                default:
                    return "";
            }
        }
    }
}