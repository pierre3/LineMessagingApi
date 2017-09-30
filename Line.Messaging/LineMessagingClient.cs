using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await ThrowIfRequestFailed(response);
        }

        public Task ReplyMessageAsync(string replyToken, params string[] messages)
        {
            return ReplyMessageAsync(replyToken, messages.Select(msg => new TextMessage(msg)).ToArray());
        }

        public async Task PushMessageAsync(string to, IList<ISendMessage> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/push");
            var content = JsonConvert.SerializeObject(new { to, messages }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await ThrowIfRequestFailed(response);
        }

        public Task PushMessageAsync(string to, params string[] messages)
        {
            return PushMessageAsync(to, messages.Select(msg => new TextMessage(msg)).ToArray());
        }

        public async Task MultiCastMessageAsync(IList<string> to, IList<ISendMessage> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/multicast");
            var content = JsonConvert.SerializeObject(new { to, messages }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await ThrowIfRequestFailed(response);
        }

        public Task MultiCastMessageAsync(IList<string> to, params string[] messages)
        {
            return MultiCastMessageAsync(to, messages.Select(msg => new TextMessage(msg)).ToArray());
        }

        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            var response = await _client.GetStringAsync($"https://api.line.me/v2/bot/profile/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(response);
        }

        public async Task<ContentStream> GetContentStreamAsync(string messageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.line.me/v2/bot/message/{messageId}/content");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await ThrowIfRequestFailed(response);
            return new ContentStream(await response.Content.ReadAsStreamAsync(), response.Content.Headers);
        }

        public async Task<byte[]> GetContentBytesAsync(string messageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.line.me/v2/bot/message/{messageId}/content");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await ThrowIfRequestFailed(response);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<UserProfile> GetGroupMemberProfileAsync(string groupId, string userId)
        {
            var response = await _client.GetStringAsync($"https://api.line.me/v2/bot/group/{groupId}/member/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(response);
        }

        public async Task<UserProfile> GetRoomMemberProfileAsync(string roomId, string userId)
        {
            var response = await _client.GetStringAsync($"https://api.line.me/v2/bot/room/{roomId}/member/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(response);
        }

        public async Task<IList<UserProfile>> GetGroupMemberProfilesAsync(string groupId)
        {
            var result = new List<UserProfile>();
            string continuationToken = null;
            do
            {
                var ids = await GetGroupMemberIdsAsync(groupId, continuationToken);

                var tasks = ids.MemberIds.Select(async userId => await GetGroupMemberProfileAsync(groupId, userId));
                var profiles = await Task.WhenAll(tasks.ToArray());

                result.AddRange(profiles);
                continuationToken = ids.Next;
            }
            while (continuationToken != null);
            return result;
        }

        public async Task<IList<UserProfile>> GetRoomMemberProfilesAsync(string roomId)
        {
            var result = new List<UserProfile>();
            string continuationToken = null;
            do
            {
                var ids = await GetGroupMemberIdsAsync(roomId, continuationToken);

                var tasks = ids.MemberIds.Select(async userId => await GetGroupMemberProfileAsync(roomId, userId));
                var profiles = await Task.WhenAll(tasks.ToArray());

                result.AddRange(profiles);
                continuationToken = ids.Next;
            }
            while (continuationToken != null);
            return result;
        }

        public async Task<GroupMemberIds> GetGroupMemberIdsAsync(string groupId, string continuationToken)
        {

            var requestUrl = $"https://api.line.me/v2/bot/group/{groupId}/members/ids";
            if (continuationToken != null)
            {
                requestUrl += $"?start={continuationToken}";
            }

            var response = await _client.GetStringAsync(requestUrl).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<GroupMemberIds>(response);
        }

        public async Task<GroupMemberIds> GetRoomMemberIdsAsync(string roomId, string continuationToken = null)
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
            await ThrowIfRequestFailed(response);
        }

        public async Task ReaveFromRoomAsync(string roomId)
        {
            var response = await _client.PostAsync($"https://api.line.me/v2/bot/room/{roomId}/leave", null).ConfigureAwait(false);
            await ThrowIfRequestFailed(response);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private async Task ThrowIfRequestFailed(HttpResponseMessage res)
        {
            if (res.IsSuccessStatusCode) { return; }
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            var errorMessage = JsonConvert.DeserializeObject<ErrorResponseMessage>(content, _jsonSerializerSettings);
            throw new LineResponseException(errorMessage.Message) { StatusCode = res.StatusCode, ResponseMessage = errorMessage };

        }
    }
}
