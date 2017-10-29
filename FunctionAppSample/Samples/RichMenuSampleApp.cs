using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class RichMenuSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private TraceWriter Log { get; }

        private static readonly ImagemapSize _richMenuSize = ImagemapSize.RichMenuShort;
        private static readonly int _buttonWidth = _richMenuSize.Width / 4;
        private static readonly int _buttonHeight = _richMenuSize.Height;
        private static readonly IList<ActionArea> _menuActionAreas = new[]
        {
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 0, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonA", "MenuA", "Change To Menu A")
            },
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 1, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonB", "MenuB", "Change To Menu B")
            },
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 2, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonC", "MenuC", "Change To Menu C")
            },
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 3, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonD", "MenuD", "Change To Menu D")
            },
        };
        private List<ResponseRichMenu> _richMenus = new List<ResponseRichMenu>();

        private static readonly RichMenu RichMenuA = new RichMenu
        {
            Name = "RichMenuA",
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu A",
            Areas = _menuActionAreas
        };
        private static readonly RichMenu RichMenuB = new RichMenu
        {
            Name = "RichMenuB",
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu B",
            Areas = _menuActionAreas
        };
        private static readonly RichMenu RichMenuC = new RichMenu
        {
            Name = "RichMenuC",
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu C",
            Areas = _menuActionAreas
        };
        private static readonly RichMenu RichMenuD = new RichMenu
        {
            Name = "RichMenuD",
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu D",
            Areas = _menuActionAreas
        };

        public RichMenuSampleApp(LineMessagingClient lineMessagingClient, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            Log = log;
        }

        public async Task CreateRichMenuAsync()
        {
            _richMenus.Clear();

            var menuList = await MessagingClient.GetRichMenuList();

            await RegisterRichMenuAsync(RichMenuA);
            await RegisterRichMenuAsync(RichMenuB);
            await RegisterRichMenuAsync(RichMenuC);
            await RegisterRichMenuAsync(RichMenuD);
                        
            _richMenus.AddRange(menuList);

            async Task RegisterRichMenuAsync(RichMenu newItem)
            {
                if (!menuList.Any(menu => menu.Name == newItem.Name))
                {
                    var id = await MessagingClient.CreateRichMenuAsync(newItem);
                    _richMenus.Add(newItem.ToResponseRichMenu(id));
                }
            }
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    await MessagingClient.ReplyMessageAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text);
                    break;
            }
        }

    }
}
