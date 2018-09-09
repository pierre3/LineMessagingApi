using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class FlexMessageSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private BlobStorage BlobStorage { get; }
        private TraceWriter Log { get; }

        private static string blobDirectoryName = "TemplateImage";
        private static string imageName = "sample.jpeg";

        public FlexMessageSampleApp(LineMessagingClient lineMessagingClient, BlobStorage blobStorage, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            BlobStorage = blobStorage;
            Log = log;
        }


        protected override async Task OnMessageAsync(MessageEvent ev)
        {

            var msg = ev.Message as TextEventMessage;
            if (msg == null) { return; }

            switch (msg.Text)
            {
                
            }

            FlexMessage.CreateCarouselMessage("sample")
                .AddQuickReply(new QuickReply())
                .AddBubbleContainer(new BubbleContainer() {  })
                .AddBubbleContainer(new BubbleContainer())
                .AddBubbleContainer(new BubbleContainer());

        }

    }

    

    
}
