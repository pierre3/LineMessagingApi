using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Line.Messaging.Liff
{
    /// <summary>
    /// HTTP Client for the LINE Front-end Framework (LIFF) API
    /// </summary>
    public class LiffClient
    {
        private HttpClient _client;
        private JsonSerializerSettings _jsonSerializerSettings;
        private string _uri;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channelAccessToken">
        /// Channel access token
        /// </param>
        /// <param name="uri">
        /// Request base URL
        /// </param>
        public LiffClient(string channelAccessToken, string uri = "https://api.line.me/liff/v1/apps")
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            _uri = uri;
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
        public async Task<string> AddLiffApp(ViewType viewType, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = JsonConvert.SerializeObject(new View(viewType, url), _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
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
        public async Task UpdateLiffApp(string liffId, ViewType viewType, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{url}/{liffId}/view");
            var content = JsonConvert.SerializeObject(new View(viewType, url), _jsonSerializerSettings);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Gets information on all the LIFF apps registered in the channel.
        /// </summary>
        /// <returns>A JSON object with the following properties.</returns>
        public async Task<IList<LiffApp>> GetAllLiffApp()
        {
            var content = await _client.GetStringAsync(_uri);
            return JsonConvert.DeserializeObject<IList<LiffApp>>(content);
        }

        /// <summary>
        /// Deletes a LIFF app.
        /// </summary>
        /// <param name="liffId">ID of the LIFF app to be deleted</param>
        public Task DeleteLiffApp(string liffId)
        {
            return _client.DeleteAsync($"{_uri}/{liffId}");
        }
    }
}
