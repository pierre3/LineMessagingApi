using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionAppSample
{


    public static class HttpTriggerFunction
    {
        static LineMessagingClient lineMessagingClient;

        static HttpTriggerFunction()
        {
            lineMessagingClient = new LineMessagingClient(System.Configuration.ConfigurationManager.AppSettings["ChannelAccessToken"]);
            var sp = ServicePointManager.FindServicePoint(new Uri("https://api.line.me"));
            sp.ConnectionLeaseTimeout = 60 * 1000;
        }

        [FunctionName("LineMessagingApiSample")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            IEnumerable<WebhookEvent> events;
            try
            {
                var channelSecret = System.Configuration.ConfigurationManager.AppSettings["ChannelSecret"];
                events = await req.GetWebhookEventsAsync(channelSecret);
            }
            catch (InvalidSignatureException e)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden, new { Message = e.Message });
            }

            try
            {

                //var connectionString = System.Configuration.ConfigurationManager.AppSettings["AzureWebJobsStorage"];
                //var tableStorage = await LineBotTableStorage.CreateAsync(connectionString);
                //var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");
                //var app = new LineBotApp(lineMessagingClient, tableStorage, blobStorage, log);

                var app = new DateTimePickerTestApp(lineMessagingClient, log);

                await app.RunAsync(events);

            }
            catch (LineResponseException e)
            {
                log.Error(e.ToString());
                var debugUserId = System.Configuration.ConfigurationManager.AppSettings["DebugUser"];
                if (debugUserId != null)
                {
                    await lineMessagingClient.PushMessageAsync(debugUserId, e.ResponseMessage.ToString());
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }

}
