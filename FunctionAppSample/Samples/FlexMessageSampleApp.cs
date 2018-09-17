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
                await ReplyFlexWithJson(ev);
            }
            else if (msg.Text == "m")
            {
                await ReplyFlexWithMethodChaine(ev);
            }
            else
            {
                await ReplyFlexWithObjectInitializer(ev);
            }

        }

        private async Task ReplyFlexWithJson(MessageEvent ev)
        {
            await MessagingClient.ReplyMessageWithJsonAsync(ev.ReplyToken, $"[{FlexJson}]");
        }

        private async Task ReplyFlexWithMethodChaine(MessageEvent ev)
        {
            var restrant = CreateRestrantBubbleContainer();
            var news = CreateNewsBubbleContainer();
            var receipt = CreateReceiptBubbleContainer();

            var flex = FlexMessage.CreateCarouselMessage("Carousel Message")
                .AddBubbleContainer(restrant)
                .AddBubbleContainer(news)
                .AddBubbleContainer(receipt)
                .SetQuickReply(new QuickReply(new[]
                {
                    new QuickReplyButtonObject(new CameraTemplateAction("Camera")),
                    new QuickReplyButtonObject(new LocationTemplateAction("Location"))
                }));

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { flex });
        }

        private static BubbleContainer CreateNewsBubbleContainer()
        {
            return new BubbleContainer()
                .SetHeader(BoxLayout.Horizontal)
                    .AddHeaderContents(new TextComponent("NEWS DIGEST")
                    {
                        Weight = Weight.Bold,
                        Color = "#aaaaaa",
                        Size = ComponentSize.Sm
                    })
                .SetHero(imageUrl: "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_4_news.png",
                        size: ComponentSize.Full,
                        aspectRatio: AspectRatio._20_13,
                        aspectMode: AspectMode.Cover)
                    .SetHeroAction(new UriTemplateAction(null, "http://linecorp.com/"))
                .SetBody(boxLayout: BoxLayout.Horizontal,
                         spacing: Spacing.Md)
                    .AddBodyContents(
                        new BoxComponent(BoxLayout.Vertical) { Flex = 1 }
                        .AddContents(new ImageComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/02_1_news_thumbnail_1.png")
                        {
                            AspectMode = AspectMode.Cover,
                            AspectRatio = AspectRatio._4_3,
                            Size = ComponentSize.Sm,
                            Gravity = Gravity.Bottom
                        })
                        .AddContents(new ImageComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/02_1_news_thumbnail_2.png")
                        {
                            AspectMode = AspectMode.Cover,
                            AspectRatio = AspectRatio._4_3,
                            Size = ComponentSize.Sm,
                            Gravity = Gravity.Bottom
                        }))
                    .AddBodyContents(new BoxComponent(BoxLayout.Vertical) { Flex = 2 }
                        .AddContents(new TextComponent("7 Things to Know for Today")
                        {
                            Gravity = Gravity.Top,
                            Size = ComponentSize.Xs,
                            Flex = 1
                        })
                        .AddContents(new SeparatorComponent())
                        .AddContents(new TextComponent("Hay fever goes wild")
                        {
                            Gravity = Gravity.Center,
                            Size = ComponentSize.Xs,
                            Flex = 2
                        })
                        .AddContents(new SeparatorComponent())
                        .AddContents(new TextComponent("LINE Pay Begins Barcode Payment Service")
                        {
                            Gravity = Gravity.Center,
                            Size = ComponentSize.Xs,
                            Flex = 2
                        })
                        .AddContents(new SeparatorComponent())
                        .AddContents(new TextComponent("LINE Adds LINE Wallet")
                        {
                            Gravity = Gravity.Bottom,
                            Size = ComponentSize.Xs,
                            Flex = 1
                        }))
                .SetFooter(BoxLayout.Horizontal)
                    .AddFooterContents(new ButtonComponent()
                    {
                        Action = new UriTemplateAction("More", "https://linecorp.com")
                    });
        }

        private static BubbleContainer CreateRestrantBubbleContainer()
        {
            return new BubbleContainer()
                .SetHero(
                    imageUrl: "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png",
                    size: ComponentSize.Full,
                    aspectRatio: AspectRatio._20_13,
                    aspectMode: AspectMode.Cover)
                    .SetHeroAction(new UriTemplateAction(null, "http://linecorp.com/"))
                .SetBody(BoxLayout.Vertical)
                    .AddBodyContents(new TextComponent("Broun Cafe")
                    {
                        Weight = Weight.Bold,
                        Size = ComponentSize.Xl
                    })
                    .AddBodyContents(new BoxComponent(BoxLayout.Baseline) { Margin = Spacing.Md }
                        .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                        {
                            Size = ComponentSize.Sm
                        })
                        .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                        {
                            Size = ComponentSize.Sm
                        })
                        .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                        {
                            Size = ComponentSize.Sm
                        })
                        .AddContents(new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gray_star_28.png")
                        {
                            Size = ComponentSize.Sm
                        })
                        .AddContents(new TextComponent("4.0")
                        {
                            Size = ComponentSize.Sm,
                            Margin = Spacing.Md,
                            Flex = 0,
                            Color = "#999999"
                        }))
                    .AddBodyContents(new BoxComponent(BoxLayout.Vertical) { Margin = Spacing.Lg, Spacing = Spacing.Sm }
                        .AddContents(new BoxComponent(BoxLayout.Baseline)
                        {
                            Spacing = Spacing.Sm
                        }
                            .AddContents(new TextComponent("Place")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#aaaaaa",
                                Flex = 1
                            })
                            .AddContents(new TextComponent("Miraina Tower, 4-1-6 Shinjuku, Tokyo") { Size = ComponentSize.Sm, Wrap = true, Color = "#666666", Flex = 5 }))
                    .AddContents(new BoxComponent(BoxLayout.Baseline) { Spacing = Spacing.Sm }
                            .AddContents(new TextComponent("Time") { Size = ComponentSize.Sm, Color = "#aaaaaa", Flex = 1 })
                            .AddContents(new TextComponent("10:00 - 23:00") { Size = ComponentSize.Sm, Wrap = true, Color = "#666666", Flex = 5 })))
                .SetFooter(new BoxComponent(BoxLayout.Vertical) { Spacing = Spacing.Sm, Flex = 0 }
                    .AddContents(new ButtonComponent(new UriTemplateAction("Call", "https://linecorp.com")) { Style = ButtonStyle.Link, Height = ButtonHeight.Sm })
                    .AddContents(new ButtonComponent(new UriTemplateAction("WEBSITE", "https://linecorp.com")) { Style = ButtonStyle.Link, Height = ButtonHeight.Sm })
                    .AddContents(new SpacerComponent(ComponentSize.Sm)))
                .SetBodyStyle(new BlockStyle()
                {
                    BackgroundColor = ColorCode.LightSlateGrey,
                    Separator = true,
                    SeparatorColor = ColorCode.DarkSlateGray
                })
                .SetFooterStyle(new BlockStyle()
                {
                    BackgroundColor = ColorCode.Azure
                });
        }

        private static BubbleContainer CreateReceiptBubbleContainer()
        {
            return new BubbleContainer()
                .SetFooterStyle(new BlockStyle() { Separator = true })
                .SetBody(BoxLayout.Vertical)
                    .AddBodyContents(new TextComponent("RECEIPT")
                    {
                        Weight = Weight.Bold,
                        Color = "#1DB446",
                        Size = ComponentSize.Sm
                    })
                    .AddBodyContents(new TextComponent("Brown Store")
                    {
                        Weight = Weight.Bold,
                        Size = ComponentSize.Xxl,
                        Margin = Spacing.Md
                    })
                    .AddBodyContents(new TextComponent("Miraina Tower, 4 - 1 - 6 Shinjuku, Tokyo")
                    {
                        Size = ComponentSize.Xs,
                        Color = "#aaaaaa",
                        Wrap = true
                    })
                    .AddBodyContents(new SeparatorComponent() { Margin = Spacing.Xxl })
                    .AddBodyContents(new BoxComponent(BoxLayout.Vertical)
                    {
                        Margin = Spacing.Xxl,
                        Spacing = Spacing.Sm
                    }
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("Energy Drink")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("$2.99")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("Chewing Gum")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("$0.99")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("Bottled Water")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("$3.33")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            }))
                        .AddContents(new SeparatorComponent() { Margin = Spacing.Xxl })
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("ITEMS")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("3")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("TOTAL")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("$7.31")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("CASH")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("$8.0")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            }))
                        .AddContents(new BoxComponent(BoxLayout.Horizontal)
                            .AddContents(new TextComponent("CHANGE")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#555555",
                                Flex = 0
                            })
                            .AddContents(new TextComponent("$0.69")
                            {
                                Size = ComponentSize.Sm,
                                Color = "#111111",
                                Align = Align.End
                            })))
                    .AddBodyContents(new SeparatorComponent() { Margin = Spacing.Xl })
                    .AddBodyContents(new BoxComponent(BoxLayout.Horizontal) { Margin = Spacing.Md }
                        .AddContents(new TextComponent("PAYMENT ID")
                        {
                            Size = ComponentSize.Xs,
                            Color = "#aaaaaa",
                            Flex = 0
                        })
                        .AddContents(new TextComponent("#743289384279")
                        {
                            Size = ComponentSize.Xs,
                            Color = "#aaaaaa",
                            Align = Align.End
                        }));

        }

        private async Task ReplyFlexWithObjectInitializer(MessageEvent ev)
        {
            var flex = new FlexMessage("Restrant")
            {

                Contents = new BubbleContainer()
                {
                    Hero = new ImageComponent(url: "https://scdn.line-apps.com/n/channel_devcenter/img/fx/01_1_cafe.png")
                    {
                        Size = ComponentSize.Full,
                        AspectRatio = AspectRatio._20_13,
                        AspectMode = AspectMode.Cover,
                        Action = new UriTemplateAction(null, "http://linecorp.com/")
                    },
                    Body = new BoxComponent(layout: BoxLayout.Vertical)
                    {
                        Contents = new IFlexComponent[] {
                        new TextComponent("Broun Cafe")
                        {
                            Weight = Weight.Bold,
                            Size = ComponentSize.Xl
                        },
                        new BoxComponent(layout: BoxLayout.Baseline)
                        {
                            Margin = Spacing.Md,
                            Contents = new IFlexComponent[]
                            {
                                new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                                {
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                                {
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                                {
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gold_star_28.png")
                                {
                                    Size = ComponentSize.Sm
                                },
                                new IconComponent("https://scdn.line-apps.com/n/channel_devcenter/img/fx/review_gray_star_28.png")
                                {
                                    Size = ComponentSize.Sm
                                },
                                new TextComponent("4.0")
                                {
                                    Size = ComponentSize.Sm,
                                    Margin = Spacing.Md,
                                    Flex = 0,
                                    Color = "#999999"
                                }
                            }
                        },
                        new BoxComponent(BoxLayout.Vertical)
                        {
                            Margin = Spacing.Lg,
                            Spacing = Spacing.Sm,
                            Contents = new IFlexComponent[]
                            {
                                new BoxComponent(BoxLayout.Baseline)
                                {
                                    Spacing = Spacing.Sm,
                                    Contents = new IFlexComponent[]
                                    {
                                        new TextComponent("Place")
                                        {
                                            Size = ComponentSize.Sm,
                                            Color = "#aaaaaa",
                                            Flex = 1
                                        },
                                        new TextComponent("Miraina Tower, 4-1-6 Shinjuku, Tokyo")
                                        {
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
                                new TextComponent("Time")
                                {
                                    Size = ComponentSize.Sm,
                                    Color = "#aaaaaa",
                                    Flex = 1
                                },
                                new TextComponent("10:00 - 23:00")
                                {
                                    Size = ComponentSize.Sm,
                                    Wrap = true,
                                    Color = "#666666",
                                    Flex=5
                                }
                            }
                        }
                    }
                    },
                    Footer = new BoxComponent(BoxLayout.Vertical)
                    {
                        Spacing = Spacing.Sm,
                        Flex = 0,
                        Contents = new IFlexComponent[]
                        {
                            new ButtonComponent(new UriTemplateAction("Call", "https://linecorp.com"))
                            {
                                Style = ButtonStyle.Link,
                                Height = ButtonHeight.Sm
                            },
                            new ButtonComponent(new UriTemplateAction("WEBSITE", "https://linecorp.com"))
                            {
                                Style = ButtonStyle.Link,
                                Height = ButtonHeight.Sm
                            },
                            new SpacerComponent(ComponentSize.Sm)
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
                }
            };

            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, new[] { flex });
        }
    }
}
