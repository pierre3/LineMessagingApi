class LineBotApp : WebhookApplication
{
  private LineMessagingClient MessagingClient { get; }
  private TraceWriter Log { get; }
  
  public WebhookApplication(LineMessagingClient lineMessagingClient,TraceWriter log)
  {
      MessagingClient = lineMessagingClient;
      Log = log;
  }

  protected override async Task OnMessageAsync(MessageEvent ev)
  {
    Log.Info($"SourceType:{ev.Source.Type},SourceId:{ev.Source.Id}");
    switch (ev.Message)
    {
      case TextEventMessage textMessage:
        await MessagingClient.ReplyMessageAsync(ev.ReplyToken, textMessage.Text);
        break;
      case ImageEventMessage imageMessage:
        //...
        break;
      case AudioEventMessage audioEventMessage:
        //...
        break;
      case VideoEventMessage videoMessage:
        //...
        break;
      case FileEventMessage fileMessage:
        //...
        break;
      case LocationEventMessage locationMessage:
        //...
        break;
      case StickerEventMessage stickerMessage:
        //...         
        break;
    }
  }
}
