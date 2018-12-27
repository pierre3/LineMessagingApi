using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Line.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Line.MessagingTest
{
    [TestClass]
    public class FlexObjectSerializeTest
    {
        static FlexObjectSerializeTest()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented
                };

                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            };
        }

        [TestMethod]
        public void RestrantTest()
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
                        .AddContents(new ButtonComponent(new UriTemplateAction("Call", "https://linecorp.com", new AltUri("https://linecorp.com/en/"))) { Style = ButtonStyle.Link, Height = ButtonHeight.Sm })
                        .AddContents(new ButtonComponent(new UriTemplateAction("WEBSITE", "https://linecorp.com", new AltUri("https://linecorp.com/en/"))) { Style = ButtonStyle.Link, Height = ButtonHeight.Sm })
                        .AddContents(new SpacerComponent(ComponentSize.Sm))));

            var jsonA = JsonConvert.SerializeObject(flex);
            Console.WriteLine(jsonA);
            Console.WriteLine("-----------------------------------------------------");

            flex = new FlexMessage("Restrant")
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
                    }
                }
            };

            var jsonB = JsonConvert.SerializeObject(flex);
            Console.WriteLine(jsonB);            
        }
    }
}
