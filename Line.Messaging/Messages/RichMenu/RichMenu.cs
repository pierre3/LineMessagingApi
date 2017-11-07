using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Rich menu object
    /// https://developers.line.me/en/docs/messaging-api/reference/#rich-menu-object
    /// </summary>
    public class RichMenu
    {
        /// <summary>
        /// size object which contains the width and height of the rich menu displayed in the chat. Rich menu images must be one of the following sizes: 2500x1686, 2500x843.
        /// </summary>
        public ImagemapSize Size { get; set; }

        /// <summary>
        /// true to display the rich menu by default. Otherwise, false.
        /// </summary>
        public bool Selected { set; get; }

        /// <summary>
        /// Name of the rich menu. This value can be used to help manage your rich menus and is not displayed to users. Maximum of 300 characters.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Text displayed in the chat bar. Maximum of 14 characters.
        /// </summary>
        public string ChatBarText { set; get; }

        /// <summary>
        /// Array of area objects which define the coordinates and size of tappable areas. Maximum of 20 area objects.
        /// </summary>
        public IList<ActionArea> Areas { set; get; }

        public ResponseRichMenu ToResponseRichMenu(string richMenuId = "")
        {
            return new ResponseRichMenu(richMenuId, this);
        }
    }
}
