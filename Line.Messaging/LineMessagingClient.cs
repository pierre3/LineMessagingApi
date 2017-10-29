using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static async Task<ChannelAccessToken> IssueChannelAccessTokenAsync(HttpClient httpClient, string channelId, string channelAccessToken)
        {
            var response = await httpClient.PostAsync("https://api.line.me/v2/oauth/accessToken",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = channelId,
                    ["client_secret"] = channelAccessToken
                })).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ChannelAccessToken>(json,
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });
        }

        public static async Task RevokeChannelAccessTokenAsync(HttpClient httpClient, string channelAccessToken)
        {
            var response = await httpClient.PostAsync("https://api.line.me/v2/oauth/revoke",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["access_token"] = channelAccessToken
                })).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        public static async Task<LineMessagingClient> CreateAsync(string channelId, string channelSecret)
        {
            if (string.IsNullOrEmpty(channelId)) { throw new ArgumentNullException(nameof(channelId)); }
            if (string.IsNullOrEmpty(channelSecret)) { throw new ArgumentNullException(nameof(channelSecret)); }
            using (var client = new HttpClient())
            {
                var accessToken = await IssueChannelAccessTokenAsync(client, channelId, channelSecret);
                return new LineMessagingClient(accessToken.AccessToken);
            }
        }

        public LineMessagingClient(string channelAccessToken)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            _jsonSerializerSettings = new CamelCaseJsonSerializerSettings();
        }

        public async Task ReplyMessageAsync(string replyToken, IList<ISendMessage> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/reply");
            var content = JsonConvert.SerializeObject(new { replyToken, messages }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
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
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
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
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        public Task MultiCastMessageAsync(IList<string> to, params string[] messages)
        {
            return MultiCastMessageAsync(to, messages.Select(msg => new TextMessage(msg)).ToArray());
        }

        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            var content = await GetStringAsync($"https://api.line.me/v2/bot/profile/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(content);
        }

        public async Task<ContentStream> GetContentStreamAsync(string messageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.line.me/v2/bot/message/{messageId}/content");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
            return new ContentStream(await response.Content.ReadAsStreamAsync(), response.Content.Headers);
        }

        public async Task<byte[]> GetContentBytesAsync(string messageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.line.me/v2/bot/message/{messageId}/content");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<UserProfile> GetGroupMemberProfileAsync(string groupId, string userId)
        {
            var content = await GetStringAsync($"https://api.line.me/v2/bot/group/{groupId}/member/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(content);
        }

        public async Task<UserProfile> GetRoomMemberProfileAsync(string roomId, string userId)
        {
            var content = await GetStringAsync($"https://api.line.me/v2/bot/room/{roomId}/member/{userId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<UserProfile>(content);
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

            var content = await GetStringAsync(requestUrl).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<GroupMemberIds>(content);
        }

        public async Task<GroupMemberIds> GetRoomMemberIdsAsync(string roomId, string continuationToken = null)
        {
            var requestUrl = $"https://api.line.me/v2/bot/room/{roomId}/members/ids";
            if (continuationToken != null)
            {
                requestUrl += $"?start={continuationToken}";
            }

            var content = await GetStringAsync(requestUrl).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<GroupMemberIds>(content);
        }

        public async Task LeaveFromGroupAsync(string groupId)
        {
            var response = await _client.PostAsync($"https://api.line.me/v2/bot/group/{groupId}/leave", null).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        public async Task LeaveFromRoomAsync(string roomId)
        {
            var response = await _client.PostAsync($"https://api.line.me/v2/bot/room/{roomId}/leave", null).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        public async Task<RichMenu> GetRichMenuAsync(string richMenuId)
        {
            var json = await GetStringAsync($"https://api.line.me/v2/bot/richmenu/{richMenuId}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ResponseRichMenu>(json);
        }

        public async Task<string> CreateRichMenuAsync(RichMenu richMenu)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/reply");
            var content = JsonConvert.SerializeObject(richMenu, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeAnonymousType(json, new { richMenuId = "" }).richMenuId;
        }

        public async Task DeleteRichMenuAsync(string richMenuId)
        {
            var response = await _client.DeleteAsync($"https://api.line.me/v2/bot/richmenu/{richMenuId}");
        }

        public async Task<string> GetRichMenuIdOfUserAsync(string userId)
        {
            var json = await GetStringAsync($"https://api.line.me/v2/bot/user/{userId}/richmenu");
            return JsonConvert.DeserializeAnonymousType(json, new { richMenuId = "" }).richMenuId;
        }

        public async Task LinkRichMenuToUserAsync(string userId, string richMenuId)
        {
            var response = await _client.PostAsync($"https://api.line.me/v2/bot/user/{userId}/richmenu/{richMenuId}", null);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        public async Task UnLinkRichMenuFromUserAsync(string userId)
        {
            var res = await _client.DeleteAsync($"https://api.line.me/v2/bot/user/{userId}/richmenu").ConfigureAwait(false);
            await res.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

        public async Task<ContentStream> DownloadRichMenuImageAsync(string richMenuId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.line.me/v2/bot/richmenu/{richMenuId}/content");
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
            return new ContentStream(await response.Content.ReadAsStreamAsync(), response.Content.Headers);
        }

        public Task UploadRichMenuJpegImageAsync(Stream stream, string richMenuId)
        {
            return UploadRichMenuImageAsync(stream, richMenuId, "image/jpeg");
        }

        public Task UploadRichMenuPngImageAsync(Stream stream, string richMenuId)
        {
            return UploadRichMenuImageAsync(stream, richMenuId, "image/png");
        }

        public async Task<IList<ResponseRichMenu>> GetRichMenuList()
        {
            var json = await GetStringAsync("https://api.line.me/v2/bot/richmenu/list").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IList<ResponseRichMenu>>(json, _jsonSerializerSettings);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private async Task<string> GetStringAsync(string requestUri)
        {
            var response = await _client.GetAsync(requestUri).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task UploadRichMenuImageAsync(Stream stream, string richMenuId, string mediaType)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.line.me/v2/bot/richmenu/{richMenuId}/content");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            request.Content = new StreamContent(stream);
            var response = await _client.SendAsync(request).ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
        }

    }
}
