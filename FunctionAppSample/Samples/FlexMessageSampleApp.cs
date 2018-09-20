using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
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
        private static readonly string TextMessageJson = "{ \"type\" : \"text\", \"text\" : \"I Sent a flex message with json string.\" }";

        protected override async Task OnMessageAsync(MessageEvent ev)
        {

            if (!(ev.Message is TextEventMessage msg)) { return; }

            if (msg.Text == "s")
            {
                await ReplyFlexWithJson(ev);
            }
            else if (msg.Text == "e")
            {
                await ReplyFlexWithExtensions(ev);
            }
            else
            {
                await ReplyFlexWithObjectInitializer(ev);
            }

        }

        private async Task ReplyFlexWithJson(MessageEvent ev)
        {
            await MessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, FlexJson, TextMessageJson);
        }

        private async Task ReplyFlexWithExtensions(MessageEvent ev)
        {
            var restrant = CreateRestrantWithObjectInitializer();
            var news = CreateNewsWithExtensions();
            var receipt = CreateReceiptWithExtensions();

            var bubble = FlexMessage.CreateBubbleMessage("Bubble Message")
                .SetBubbleContainer(restrant);

            var carousel = FlexMessage.CreateCarouselMessage("Carousel Message")
                .AddBubbleContainer(restrant)
                .AddBubbleContainer(news)
                .AddBubbleContainer(receipt)
                .SetQuickReply(new QuickReply(new[]
                {
                    new QuickReplyButtonObject(new CameraTemplateAction("Camera")),
                    new QuickReplyButtonObject(new LocationTemplateAction("Location"))
                }));

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble, carousel });
        }

        private async Task ReplyFlexWithObjectInitializer(MessageEvent ev)
        {
            var restrant = CreateRestrantWithObjectInitializer();
            var news = CreateNewsWithExtensions();
            var receipt = CreateReceiptWithExtensions();

            var bubble = new FlexMessage("Bubble Message")
            {
                Contents = restrant
            };

            var carousel = new FlexMessage("Carousel Message")
            {
                Contents = new CarouselContainer()
                {
                    Contents = new BubbleContainer[]
                    {
                        restrant,
                        news,
                        receipt
                    }
                },
                QuickReply = new QuickReply(new[]
                {
                    new QuickReplyButtonObject(new CameraRollTemplateAction("CameraRoll")),
                    new QuickReplyButtonObject(new CameraTemplateAction("Camera")),
                    new QuickReplyButtonObject(new LocationTemplateAction("Location"))
                })
            };

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new FlexMessage[] { bubble, carousel });
        }

        private static BubbleContainer CreateNewsWithExtensions()
        {
            return new BubbleContainer()
                .SetHeader(BoxLayout.Horizontal)
                    .AddHeaderContents(new TextComponent("NEWS DIGEST") { Weight = Weight.Bold, Color = "#aaaaaa", Size = ComponentSize.Sm })
                .SetHero(imageUrl: "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_4_news.png",
                        size: ComponentSize.Full, aspectRatio: AspectRatio._20_13, aspectMode: AspectMode.Cover)
                    .SetHeroAction(new UriTemplateAction(null, "http://linecorp.com/"))
                .SetBody(boxLayout: BoxLayout.Horizontal, spacing: Spacing.Md)
                    .AddBodyContents(new BoxComponent(BoxLayout.Vertical) { Flex = 1 }
                        .AddContents(new ImageComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/02_1_news_thumbnail_1.png")
                        { AspectMode = AspectMode.Cover, AspectRatio = AspectRatio._4_3, Size = ComponentSize.Sm, Gravity = Gravity.Bottom })
                        .AddContents(new ImageComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/02_1_news_thumbnail_2.png")
                        { AspectMode = AspectMode.Cover, AspectRatio = AspectRatio._4_3, Size = ComponentSize.Sm, Gravity = Gravity.Bottom }))
                    .AddBodyContents(new BoxComponent(BoxLayout.Vertical) { Flex = 2 }
                        .AddContents(new TextComponent("7 Things to Know for Today") { Gravity = Gravity.Top, Size = ComponentSize.Xs, Flex = 1 })
                        .AddContents(new SeparatorComponent())
                        .AddContents(new TextComponent("Hay fever goes wild") { Gravity = Gravity.Center, Size = ComponentSize.Xs, Flex = 2 })
                        .AddContents(new SeparatorComponent())
                        .AddContents(new TextComponent("LINE Pay Begins Barcode Payment Service") { Gravity = Gravity.Center, Size = ComponentSize.Xs, Flex = 2 })
                        .AddContents(new SeparatorComponent())
                        .AddContents(new TextComponent("LINE Adds LINE Wallet") { Gravity = Gravity.Bottom, Size = ComponentSize.Xs, Flex = 1 }))
                .SetFooter(BoxLayout.Horizontal)
                    .AddFooterContents(new ButtonComponent() { Action = new UriTemplateAction("More", "https://linecorp.com") });
        }

        private static BubbleContainer CreateReceiptWithExtensions()
        {
            return new BubbleContainer()
                .SetFooterStyle(new BlockStyle() { Separator = true })
                .SetBody(BoxLayout.Vertical)
                    .AddBodyContents(new TextComponent("RECEIPT") { Weight = Weight.Bold, Color = "#1DB446", Size = ComponentSize.Sm })
                    .AddBodyContents(new TextComponent("Brown Store") { Weight = Weight.Bold, Size = ComponentSize.Xxl, Margin = Spacing.Md })
                    .AddBodyContents(new TextComponent("Miraina Tower, 4 - 1 - 6 Shinjuku, Tokyo") { Size = ComponentSize.Xs, Color = "#aaaaaa", Wrap = true })
                    .AddBodyContents(new SeparatorComponent() { Margin = Spacing.Xxl })
                    .AddBodyContents(new BoxComponent(BoxLayout.Vertical) { Margin = Spacing.Xxl, Spacing = Spacing.Sm }
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("Energy Drink") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("$2.99") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("Chewing Gum") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("$0.99") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("Bottled Water") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("$3.33") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End }))
                        .AddContents(new SeparatorComponent() { Margin = Spacing.Xxl })
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("ITEMS") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("3") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("TOTAL") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("$7.31") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("CASH") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("$8.0") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("CHANGE") { Size = ComponentSize.Sm, Color = "#555555", Flex = 0 })
                            .AddContents(new TextComponent("$0.69") { Size = ComponentSize.Sm, Color = "#111111", Align = Align.End })))
                    .AddBodyContents(new SeparatorComponent() { Margin = Spacing.Xl })
                    .AddBodyContents(new BoxComponent(BoxLayout.Horizontal) { Margin = Spacing.Md }
                        .AddContents(new TextComponent("PAYMENT ID") { Size = ComponentSize.Xs, Color = "#aaaaaa", Flex = 0 })
                        .AddContents(new TextComponent("#743289384279") { Size = ComponentSize.Xs, Color = "#aaaaaa", Align = Align.End }));
        }


        private static BubbleContainer CreateRestrantWithObjectInitializer()
        {
            return new BubbleContainer()
            {
                Hero = new ImageComponent()
                {
                    Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png",
                    Size = ComponentSize.Full,
                    AspectRatio = AspectRatio._20_13,
                    AspectMode = AspectMode.Cover,
                    Action = new UriTemplateAction(null, "http://linecorp.com/")
                },
                Body = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Contents = new IFlexComponent[]
                    {
                        new TextComponent()
                        {
                            Text = "Broun Cafe",
                            Weight = Weight.Bold,
                            Size = ComponentSize.Xl
                        },
                        new BoxComponent()
                        {
                            Layout = BoxLayout.Baseline,
                            Margin = Spacing.Md,
                            Contents = new IFlexComponent[]
                            {
                                new IconComponent()
                                {
                                    Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png",
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent()
                                {
                                    Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png",
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent()
                                {
                                    Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png",
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent()
                                {
                                    Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png",
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent()
                                {
                                    Url = "https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gray_star_28.png",
                                    Size = ComponentSize.Sm
                                },
                                new TextComponent()
                                {
                                    Text = "4.0",
                                    Size = ComponentSize.Sm,
                                    Margin = Spacing.Md,
                                    Flex = 0,
                                    Color = "#999999"
                                }
                            }
                        },
                        new BoxComponent()
                        {
                            Layout = BoxLayout.Vertical,
                            Margin = Spacing.Lg,
                            Spacing = Spacing.Sm,
                            Contents = new IFlexComponent[]
                            {
                                new BoxComponent()
                                {
                                    Layout = BoxLayout.Baseline,
                                    Spacing = Spacing.Sm,
                                    Contents = new IFlexComponent[]
                                    {
                                        new TextComponent()
                                        {
                                            Text = "Place",
                                            Size = ComponentSize.Sm,
                                            Color = "#aaaaaa",
                                            Flex = 1
                                        },
                                        new TextComponent()
                                        {
                                            Text = "Miraina Tower, 4-1-6 Shinjuku, Tokyo",
                                            Size = ComponentSize.Sm,
                                            Wrap = true,
                                            Color = "#666666",
                                            Flex = 5
                                        }
                                    }
                                }
                            }
                        },
                        new BoxComponent(BoxLayout.Baseline)
                        {
                            Spacing = Spacing.Sm,
                            Contents = new IFlexComponent[]
                            {
                                new TextComponent()
                                {
                                    Text = "Time",
                                    Size = ComponentSize.Sm,
                                    Color = "#aaaaaa",
                                    Flex = 1
                                },
                                new TextComponent()
                                {
                                    Text = "10:00 - 23:00",
                                    Size = ComponentSize.Sm,
                                    Wrap = true,
                                    Color = "#666666",
                                    Flex=5
                                }
                            }
                        }
                    }
                },
                Footer = new BoxComponent()
                {
                    Layout = BoxLayout.Vertical,
                    Spacing = Spacing.Sm,
                    Flex = 0,
                    Contents = new IFlexComponent[]
                        {
                            new ButtonComponent()
                            {
                                Action = new UriTemplateAction("Call", "https://linecorp.com"),
                                Style = ButtonStyle.Link,
                                Height = ButtonHeight.Sm
                            },
                            new ButtonComponent()
                            {
                                Action = new UriTemplateAction("WEBSITE", "https://linecorp.com"),
                                Style = ButtonStyle.Link,
                                Height = ButtonHeight.Sm
                            },
                            new SpacerComponent()
                            {
                                Size = ComponentSize.Sm
                            }
                        }
                },
                Styles = new BubbleStyles()
                {
                    Body = new BlockStyle()
                    {
                        BackgroundColor = ColorCode.FromRgb(192, 200, 200),
                        Separator = true,
                        SeparatorColor = ColorCode.DarkViolet
                    },
                    Footer = new BlockStyle()
                    {
                        BackgroundColor = ColorCode.Ivory
                    }
                }
            };
        }

    }
}
