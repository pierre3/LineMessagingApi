using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Line.Messaging
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeAsync(this HttpResponseMessage res)
        {
            if (res.IsSuccessStatusCode) { return res; }
            var content = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
            var errorMessage = JsonConvert.DeserializeObject<ErrorResponseMessage>(content, new CamelCaseJsonSerializerSettings());
            throw new LineResponseException(errorMessage.Message) { StatusCode = res.StatusCode, ResponseMessage = errorMessage };
        }
    }
}
