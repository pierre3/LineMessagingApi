using System.Collections.Generic;

namespace Line.Messaging
{
    public class RichMenu
    {
        public ImagemapSize Size { get; set; }
        public bool Selected { set; get; }
        public string Name { get; set; }
        public string ChatBarText { set; get; }
        public IList<ActionArea> Areas { set; get; }

        public ResponseRichMenu ToResponseRichMenu(string richMenuId = "")
        {
            return new ResponseRichMenu(richMenuId, this);
        }
    }

    public class ResponseRichMenu : RichMenu
    {
        public string RichMenuId { get; set; }

        public ResponseRichMenu(string richMenuId, RichMenu source)
        {
            RichMenuId = richMenuId;
            Size = source.Size;
            Selected = source.Selected;
            Name = source.Name;
            ChatBarText = source.ChatBarText;
            Areas = source.Areas;
        }

        public static ResponseRichMenu CreateFrom(dynamic dynamicObj)
        {
            var menu = new RichMenu()
            {
                Name = (string)dynamicObj?.name,
                Size = new ImagemapSize((int)(dynamicObj?.size?.width ?? 0), (int)(dynamicObj?.size?.height ?? 0)),
                Selected = (bool)(dynamicObj?.selected ?? false),
                ChatBarText = (string)dynamicObj?.chatBarText
            };
            return new ResponseRichMenu((string)dynamicObj?.richMenuId, menu);
        }

    }
}
