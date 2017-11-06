using Newtonsoft.Json;
using System.Collections.Generic;

namespace Line.Messaging.Webhooks
{
    public static class WebhookEventParser
    {
        public static IEnumerable<WebhookEvent> Parse(string webhookContent)
        {
            dynamic dynamicObject = JsonConvert.DeserializeObject(webhookContent);
            if (dynamicObject == null) { yield break; }

            foreach (var ev in dynamicObject.events)
            {
                var webhookEvent = WebhookEvent.CreateFrom(ev);
                if (webhookEvent == null) { continue; }
                yield return webhookEvent;
            }
        }
    }
}
