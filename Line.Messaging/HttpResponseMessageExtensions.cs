using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Line.Messaging
{
    internal static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Validate the response status.
        /// </summary>
        /// <param name="response">HttpResponseMessage</param>
        /// <returns>HttpResponseMessage</returns>
        internal static async Task<HttpResponseMessage> EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = JsonConvert.DeserializeObject<ErrorResponseMessage>(content, new CamelCaseJsonSerializerSettings());
                throw new LineResponseException(errorMessage.Message) { StatusCode = response.StatusCode, ResponseMessage = errorMessage };
            }
        }
    }
}
