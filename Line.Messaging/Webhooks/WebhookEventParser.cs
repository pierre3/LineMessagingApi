using Newtonsoft.Json;
using System.Collections.Generic;

namespace Line.Messaging.Webhooks
{
    public static class WebhookEventParser
    {
        public static IEnumerable<WebhookEvent> Parse(string webhookContent)
        {
            dynamic dynamicObj = JsonConvert.DeserializeObject(webhookContent);
            if (dynamicObj == null) { yield break; }

            foreach (var ev in dynamicObj.events)
            {
                var webhookEvent = WebhookEvent.CreateFrom(ev);
                if (webhookEvent == null) { continue; }
                yield return webhookEvent;
            }
        }
    }
}
