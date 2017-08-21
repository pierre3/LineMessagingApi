using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Line.Messaging.Webhooks
{
    public static class WebhookRequestMessageHelper
    {

        public static async Task<IEnumerable<WebhookEvent>> GetWebhookEventsAsync(this HttpRequestMessage req, string channelSecret)
        {
            if (req == null) { throw new ArgumentNullException(nameof(req)); }
            if (channelSecret == null) { throw new ArgumentNullException(nameof(channelSecret)); }

            var content = await req.Content.ReadAsStringAsync();

            var xLineSignature = req.Headers.GetValues("X-Line-Signature").FirstOrDefault();
            if (string.IsNullOrEmpty(xLineSignature) || !VerifySignature(channelSecret, xLineSignature, content))
            {
                throw new InvalidSignatureException("Signature validation faild.");
            }
            return WebhookEventParser.Parse(content);
        }

        public static bool VerifySignature(string channelSecret, string xLineSignature, string requestBody)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(channelSecret);
                var body = Encoding.UTF8.GetBytes(requestBody);

                using (HMACSHA256 hmac = new HMACSHA256(key))
                {
                    var hash = hmac.ComputeHash(body, 0, body.Length);
                    var hash64 = Convert.ToBase64String(hash);
                    return xLineSignature == hash64;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
