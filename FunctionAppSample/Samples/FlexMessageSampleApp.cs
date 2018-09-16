using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class FlexMessageSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private TraceWriter Log { get; }

        public FlexMessageSampleApp(LineMessagingClient lineMessagingClient, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            Log = log;
        }

        private static readonly string FlexJson =
@"{
  ""type"": ""flex"",
  ""contents"": {
    ""type"": ""bubble"",
    ""direction"": ""ltr"",
    ""hero"": {
      ""type"": ""image"",
      ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png"",
      ""size"": ""full"",
      ""aspectRatio"": ""20:13"",
      ""aspectMode"": ""cover"",
      ""action"": {
        ""type"": ""uri"",
        ""uri"": ""http://linecorp.com/""
      }
    },
    ""body"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""text"",
          ""text"": ""Broun Cafe"",
          ""size"": ""xl"",
          ""weight"": ""bold""
        },
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""icon"",
              ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png"",
              ""size"": ""sm""
            },
            {
              ""type"": ""icon"",
              ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png"",
              ""size"": ""sm""
            },
            {
              ""type"": ""icon"",
              ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png"",
              ""size"": ""sm""
            },
            {
              ""type"": ""icon"",
              ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png"",
              ""size"": ""sm""
            },
            {
              ""type"": ""icon"",
              ""url"": ""https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gray_star_28.png"",
              ""size"": ""sm""
            },
            {
              ""type"": ""text"",
              ""text"": ""4.0"",
              ""flex"": 0,
              ""margin"": ""md"",
              ""size"": ""sm"",
              ""color"": ""#999999""
            }
          ],
          ""margin"": ""md""
        },
        {
          ""type"": ""box"",
          ""layout"": ""vertical"",
          ""contents"": [
            {
              ""type"": ""box"",
              ""layout"": ""baseline"",
              ""contents"": [
                {
                  ""type"": ""text"",
                  ""text"": ""Place"",
                  ""flex"": 1,
                  ""size"": ""sm"",
                  ""color"": ""#aaaaaa""
                },
                {
                  ""type"": ""text"",
                  ""text"": ""Miraina Tower, 4-1-6 Shinjuku, Tokyo"",
                  ""flex"": 5,
                  ""size"": ""sm"",
                  ""wrap"": true,
                  ""color"": ""#666666""
                }
              ],
              ""spacing"": ""sm""
            }
          ],
          ""spacing"": ""sm"",
          ""margin"": ""lg""
        },
        {
          ""type"": ""box"",
          ""layout"": ""baseline"",
          ""contents"": [
            {
              ""type"": ""text"",
              ""text"": ""Time"",
              ""flex"": 1,
              ""size"": ""sm"",
              ""color"": ""#aaaaaa""
            },
            {
              ""type"": ""text"",
              ""text"": ""10:00 - 23:00"",
              ""flex"": 5,
              ""size"": ""sm"",
              ""wrap"": true,
              ""color"": ""#666666""
            }
          ],
          ""spacing"": ""sm""
        }
      ]
    },
    ""footer"": {
      ""type"": ""box"",
      ""layout"": ""vertical"",
      ""contents"": [
        {
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""Call"",
            ""uri"": ""https://linecorp.com""
          },
          ""height"": ""sm"",
          ""style"": ""link""
        },
        {
          ""type"": ""button"",
          ""action"": {
            ""type"": ""uri"",
            ""label"": ""WEBSITE"",
            ""uri"": ""https://linecorp.com""
          },
          ""height"": ""sm"",
          ""style"": ""link""
        },
        {
          ""type"": ""spacer"",
          ""size"": ""sm""
        }
      ],
      ""flex"": 0,
      ""spacing"": ""sm""
    }
  },
  ""altText"": ""Restrant""
}";

        protected override async Task OnMessageAsync(MessageEvent ev)
        {

            if (!(ev.Message is TextEventMessage msg)) { return; }

            if (msg.Text == "s")
            {
                await ReplyFlexWithStringJson(ev);
            }
            else
            {
                await ReplyFlexWithMethodChane(ev);
            }

        }

        private async Task ReplyFlexWithStringJson(MessageEvent ev)
        {
            await MessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexJson);
        }

        private async Task ReplyFlexWithMethodChane(MessageEvent ev)
        {
            FlexMessage flex = FlexMessage.CreateBubbleMessage("Restrant")
                .SetBubbleContainer(new BubbleContainer()
                    .SetHero(imageUrl: "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png",
                            flex: null,
                            margin: null,
                            align: null,
                            gravity: null,
                            size: ComponentSize.Full,
                            aspectRatio: AspectRatio._20_13,
                            aspectMode: AspectMode.Cover)
                        .SetHeroAction(new UriTemplateAction(null, "http://linecorp.com/"))
                    .SetBody(boxLayout: BoxLayout.Vertical,
                            flex: null,
                            spacing: null,
                            margin: null)
                        .AddBodyContents(new TextComponent("Broun Cafe") { Weight = Weight.Bold, Size = ComponentSize.Xl })
                        .AddBodyContents(new BoxComponent(BoxLayout.Baseline) { Margin = Spacing.Md }
                            .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png") { Size = ComponentSize.Sm })
                            .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png") { Size = ComponentSize.Sm })
                            .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png") { Size = ComponentSize.Sm })
                            .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gray_star_28.png") { Size = ComponentSize.Sm })
                            .AddContents(new TextComponent("4.0") { Size = ComponentSize.Sm, Margin = Spacing.Md, Flex = 0, Color = "#999999" }))
                        .AddBodyContents(new BoxComponent(BoxLayout.Vertical) { Margin = Spacing.Lg, Spacing = Spacing.Sm }
                            .AddContents(new BoxComponent(BoxLayout.Baseline) { Spacing = Spacing.Sm }
                                .AddContents(new TextComponent("Place") { Size = ComponentSize.Sm, Color = "#aaaaaa", Flex = 1 })
                                .AddContents(new TextComponent("Miraina Tower, 4-1-6 Shinjuku, Tokyo") { Size = ComponentSize.Sm, Wrap = true, Color = "#666666", Flex = 5 }))
                        .AddContents(new BoxComponent(BoxLayout.Baseline) { Spacing = Spacing.Sm }
                                .AddContents(new TextComponent("Time") { Size = ComponentSize.Sm, Color = "#aaaaaa", Flex = 1 })
                                .AddContents(new TextComponent("10:00 - 23:00") { Size = ComponentSize.Sm, Wrap = true, Color = "#666666", Flex = 5 })))
                    .SetFooter(new BoxComponent(BoxLayout.Vertical) { Spacing = Spacing.Sm, Flex = 0 }
                        .AddContents(new ButtonComponent(new UriTemplateAction("Call", "https://linecorp.com")) { Style = ButtonStyle.Link, Height = ButtonHeight.Sm })
                        .AddContents(new ButtonComponent(new UriTemplateAction("WEBSITE", "https://linecorp.com")) { Style = ButtonStyle.Link, Height = ButtonHeight.Sm })
                        .AddContents(new SpacerComponent(ComponentSize.Sm))));

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { flex });
        }
    }
}
