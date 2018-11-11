using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
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
            try
            {
                var channelSecret = System.Configuration.ConfigurationManager.AppSettings["ChannelSecret"];
                var botUserId = System.Configuration.ConfigurationManager.AppSettings["BotUserId"];
                var events = await req.GetWebhookEventsAsync(channelSecret, botUserId);
                var connectionString = System.Configuration.ConfigurationManager.AppSettings["AzureWebJobsStorage"];
                var botStatus = await TableStorage<BotStatus>.CreateAsync(connectionString, "botstatus");
                var blobStorage = await BlobStorage.CreateAsync(connectionString, "linebotcontainer");

                var app = new LineBotApp(lineMessagingClient, botStatus, blobStorage, log);

                /* To run sample apps in the samples directory, comment out the above line and cancel this comment out.
                var app = await AppSwitcher.SwitchAppsAsync(events,lineMessagingClient, botStatus, blobStorage, log);
                // */

                await app.RunAsync(events);

            }
            catch (InvalidSignatureException e)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden, new { e.Message });
            }
            catch (LineResponseException e)
            {
                log.Error(e.ToString());
                var debugUserId = System.Configuration.ConfigurationManager.AppSettings["DebugUser"];
                if (debugUserId != null)
                {
                    await lineMessagingClient.PushMessageAsync(debugUserId, $"{e.StatusCode}({(int)e.StatusCode}), {e.ResponseMessage.ToString()}");
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                var debugUserId = System.Configuration.ConfigurationManager.AppSettings["DebugUser"];
                if (debugUserId != null)
                {
                    await lineMessagingClient.PushMessageAsync(debugUserId, e.Message);
                }
            }
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }

}
