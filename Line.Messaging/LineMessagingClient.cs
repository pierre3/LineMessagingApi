using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Line.Messaging
{

    public class LineMessagingClient : IDisposable
    {
        private HttpClient _client;
        private JsonSerializerSettings _jsonSerializerSettings;

        public LineMessagingClient(string channelAccessToken)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }

        public async Task ReplyMessageAsync(string replyToken, IList<ISendMessage> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/reply");
            var content = JsonConvert.SerializeObject(new { replyToken, messages }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task PushMessageAsync(string to, IList<ISendMessage> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/push");
            var content = JsonConvert.SerializeObject(new { to, messages }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task MultiCastMessageAsync(IList<string> to, IList<ISendMessage> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/multicast");
            var content = JsonConvert.SerializeObject(new { to, messages }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }


        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            var response = await _client.GetStringAsync($"https://api.line.me/v2/bot/profile/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(response);
        }

        public Task<Stream> GetContentStreamAsync(string messageId)
        {
            return _client.GetStreamAsync($"https://api.line.me/v2/bot/message/{messageId}/content");
        }

        public Task<byte[]> GetContentBytesAsync(string messageId)
        {
            return _client.GetByteArrayAsync($"https://api.line.me/v2/bot/message/{messageId}/content");
        }

        public async Task<UserProfile> GetGroupMemberProfile(string groupId, string userId)
        {
            var response = await _client.GetStringAsync($"https://api.line.me/v2/bot/group/{groupId}/member/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(response);
        }

        public async Task<UserProfile> GetRoomMemberProfile(string roomId, string userId)
        {
            var response = await _client.GetStringAsync($"https://api.line.me/v2/bot/room/{roomId}/member/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(response);
        }

        public async Task<GroupMemberIds> GetGroupMemberIds(string groupId, string continuationToken = null)
        {
            var requestUrl = $"https://api.line.me/v2/bot/group/{groupId}/members/ids";
            if (continuationToken != null)
            {
                requestUrl += $"?start={continuationToken}";
            }

            var response = await _client.GetStringAsync(requestUrl).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<GroupMemberIds>(response);
        }

        public async Task<GroupMemberIds> GetRoomMemberIds(string roomId, string continuationToken = null)
        {
            var requestUrl = $"https://api.line.me/v2/bot/room/{roomId}/members/ids";
            if (continuationToken != null)
            {
                requestUrl += $"?start={continuationToken}";
            }

            var response = await _client.GetStringAsync(requestUrl).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<GroupMemberIds>(response);
        }

        public async Task ReaveFromGroupAsync(string groupId)
        {
            var response = await _client.PostAsync($"https://api.line.me/v2/bot/group/{groupId}/leave", null).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
        public async Task ReaveFromRoomAsync(string roomId)
        {
            var response = await _client.PostAsync($"https://api.line.me/v2/bot/room/{roomId}/leave", null).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
