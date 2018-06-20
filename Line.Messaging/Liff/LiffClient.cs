using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Line.Messaging.Liff
{
    /// <summary>
    /// HTTP Client for the LINE Front-end Framework (LIFF) API
    /// </summary>
    public class LiffClient
    {
        private HttpClient _client;
        private JsonSerializerSettings _jsonSerializerSettings;
        private string _requestUri;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channelAccessToken">
        /// Channel access token
        /// </param>
        /// <param name="requestUri">
        /// Request base URL
        /// </param>
        public LiffClient(string channelAccessToken, string requestUri = "https://api.line.me/liff/v1/apps")
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            _jsonSerializerSettings = new CamelCaseJsonSerializerSettings();
            _requestUri = requestUri;
        }

        /// <summary>
        /// Adds an app to LIFF. You can add up to 10 LIFF apps on one channel.
        /// </summary>
        /// <param name="viewType">
        /// Size of the LIFF app view. Specify one of the following values
        /// </param>
        /// <param name="url">
        /// URL of the LIFF app. Must start with HTTPS.    
        /// </param>
        /// <returns>
        /// LIFF app ID
        /// </returns>
        public async Task<string> AddLiffAppAsync(ViewType viewType, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            var content = JsonConvert.SerializeObject(new { view = new View(viewType, url) }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            await response.EnsureSuccessStatusCodeAsync();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeAnonymousType(result, new { liffId = "" }).liffId;
        }

        /// <summary>
        /// Updates LIFF app settings.
        /// </summary>
        /// <param name="liffId">ID of the LIFF app to be updated</param>
        /// <param name="viewType">
        /// Size of the LIFF app view. Specify one of the following values
        /// </param>
        /// <param name="url">
        /// URL of the LIFF app. Must start with HTTPS.    
        /// </param>
        public async Task UpdateLiffAppAsync(string liffId, ViewType viewType, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_requestUri}/{liffId}/view");
            var content = JsonConvert.SerializeObject(new { type = viewType, url }, _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            await response.EnsureSuccessStatusCodeAsync();
        }

        /// <summary>
        /// Gets information on all the LIFF apps registered in the channel.
        /// </summary>
        /// <returns>A JSON object with the following properties.</returns>
        public async Task<IList<LiffApp>> GetAllLiffAppAsync()
        {
            var content = await _client.GetStringAsync(_requestUri);
            return JsonConvert.DeserializeAnonymousType(content, new { apps = new LiffApp[0] }).apps;
        }

        /// <summary>
        /// Deletes a LIFF app.
        /// </summary>
        /// <param name="liffId">ID of the LIFF app to be deleted</param>
        public Task DeleteLiffAppAsync(string liffId)
        {
            return _client.DeleteAsync($"{_requestUri}/{liffId}");
        }
    }
}
