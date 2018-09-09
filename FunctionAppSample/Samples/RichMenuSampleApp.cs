using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionAppSample
{

    class RichMenuSampleApp : WebhookApplication
    {
        private LineMessagingClient MessagingClient { get; }

        private TraceWriter Log { get; }

        #region RichMenu definitions
        private static readonly ImagemapSize _richMenuSize = ImagemapSize.RichMenuShort;
        private static readonly int _buttonWidth = _richMenuSize.Width / 4;
        private static readonly int _buttonHeight = _richMenuSize.Height;

        private static readonly string MenuNameA = "RichMenuA";
        private static readonly string MenuNameB = "RichMenuB";
        private static readonly string MenuNameC = "RichMenuC";
        private static readonly string MenuNameD = "RichMenuD";

        private static readonly IList<ActionArea> _menuActionAreas = new[]
        {
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 0, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonA", MenuNameA, "Menu A")
            },
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 1, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonB", MenuNameB, "Menu B")
            },
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 2, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonC", MenuNameC, "Menu C")
            },
            new ActionArea()
            {
                Bounds = new ImagemapArea(_buttonWidth * 3, 0, _buttonWidth, _buttonHeight),
                Action = new PostbackTemplateAction("ButtonD", MenuNameD, "Menu D")
            },
        };

        private static readonly RichMenu RichMenuA = new RichMenu
        {
            Name = MenuNameA,
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu A",
            Areas = _menuActionAreas
        };
        private static readonly RichMenu RichMenuB = new RichMenu
        {
            Name = MenuNameB,
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu B",
            Areas = _menuActionAreas
        };
        private static readonly RichMenu RichMenuC = new RichMenu
        {
            Name = MenuNameC,
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu C",
            Areas = _menuActionAreas
        };
        private static readonly RichMenu RichMenuD = new RichMenu
        {
            Name = MenuNameD,
            Size = _richMenuSize,
            Selected = true,
            ChatBarText = "Menu D",
            Areas = _menuActionAreas
        };
        #endregion

        public RichMenuSampleApp(LineMessagingClient lineMessagingClient, TraceWriter log)
        {
            MessagingClient = lineMessagingClient;
            Log = log;
        }

        public async Task<IList<ResponseRichMenu>> CreateRichMenuAsync(bool isRefresh)
        {
            var menuList = await MessagingClient.GetRichMenuListAsync();
            if (isRefresh)
            {
                await DeleteRichMenusAsync(menuList);
            }
            var newMenuList = new List<ResponseRichMenu>();
            newMenuList.Add(await RegisterRichMenuAsync(RichMenuA));
            newMenuList.Add(await RegisterRichMenuAsync(RichMenuB));
            newMenuList.Add(await RegisterRichMenuAsync(RichMenuC));
            newMenuList.Add(await RegisterRichMenuAsync(RichMenuD));
            return newMenuList;

            async Task<ResponseRichMenu> RegisterRichMenuAsync(RichMenu newItem)
            {
                var item = menuList.FirstOrDefault(menu => menu.Name == newItem.Name);
                if (item == null)
                {
                    var id = await MessagingClient.CreateRichMenuAsync(newItem);
                    var image = CreateRichMenuImage(newItem);
                    await UploadRichMenuImageAsync(image, id);
                    item = newItem.ToResponseRichMenu(id);
                }
                return item;
            }
        }

        private async Task DeleteRichMenusAsync(IList<ResponseRichMenu> menuList)
        {
            foreach (var menu in menuList)
            {
                await MessagingClient.DeleteRichMenuAsync(menu.RichMenuId);
            }
            menuList.Clear();
        }

        private async Task UploadRichMenuImageAsync(System.Drawing.Image image, string richMenuId)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
                await MessagingClient.UploadRichMenuJpegImageAsync(stream, richMenuId);
            }
        }

        private System.Drawing.Image CreateRichMenuImage(RichMenu menu)
        {
            var bitmap = new Bitmap(menu.Size.Width, menu.Size.Height);
            var g = Graphics.FromImage(bitmap);

            var bkBrush = Brushes.White;

            if (menu.Name == RichMenuA.Name)
            {
                bkBrush = Brushes.Red;
            }
            else if (menu.Name == RichMenuB.Name)
            {
                bkBrush = Brushes.Blue;
            }
            else if (menu.Name == RichMenuC.Name)
            {
                bkBrush = Brushes.Green;
            }
            else if (menu.Name == RichMenuD.Name)
            {
                bkBrush = Brushes.Yellow;
            }

            g.FillRectangle(bkBrush, new Rectangle(0, 0, menu.Size.Width, menu.Size.Height));
            using (var pen = new Pen(Color.DarkGray, 10.0f))
            using (var font = new Font(FontFamily.GenericSansSerif, 80))
            {
                foreach (var area in menu.Areas)
                {
                    var action = (PostbackTemplateAction)area.Action;

                    g.DrawRectangle(pen, area.Bounds.X, area.Bounds.Y, area.Bounds.Width, area.Bounds.Height);
                    g.DrawString(action.Label, font, Brushes.Black,
                        new RectangleF(area.Bounds.X, area.Bounds.Y, area.Bounds.Width, area.Bounds.Height),
                        new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        });
                }
            }

            return bitmap;
        }

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            Log.WriteInfo($"SourceType:{ev.Source.Type}, SourceId:{ev.Source.Id}, MessageType:{ev.Message.Type}");

            var textMessage = ev.Message as TextEventMessage;
            bool isRefresh = (textMessage?.Text?.Trim()?.ToLower() == "refresh");

            var memuList = await CreateRichMenuAsync(isRefresh);
            var menuA = memuList.FirstOrDefault(m => m.Name == MenuNameA);
            if (menuA == null) { return; }

            await MessagingClient.LinkRichMenuToUserAsync(ev.Source.UserId, menuA.RichMenuId);
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, "Hello Rich Menu!");
        }

        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            var menuList = await MessagingClient.GetRichMenuListAsync();
            var nextMenu = menuList.FirstOrDefault(menu => menu.Name == ev.Postback.Data);
            if (nextMenu == null)
            {
                await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"Error!! {ev.Postback.Data} not found.");
            }
            await MessagingClient.LinkRichMenuToUserAsync(ev.Source.UserId, nextMenu.RichMenuId);
            await MessagingClient.ReplyMessageAsync(ev.ReplyToken, $"I changed a rich menu to {nextMenu.ChatBarText}");
        }
    }
}
