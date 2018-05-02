using System.Collections.Generic;
using System.Linq;

namespace Line.Messaging
{
    /// <summary>
    /// Rich menu response object.
    /// https://developers.line.me/en/docs/messaging-api/reference/#rich-menu-response-object
    /// </summary>
    public class ResponseRichMenu : RichMenu
    {
        /// <summary>
        /// Rich menu ID
        /// </summary>
        public string RichMenuId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="richMenuId">
        /// Rich menu ID
        /// </param>
        /// <param name="source">
        /// Rich menu object
        /// </param>
        public ResponseRichMenu(string richMenuId, RichMenu source)
        {
            RichMenuId = richMenuId;
            Size = source.Size;
            Selected = source.Selected;
            Name = source.Name;
            ChatBarText = source.ChatBarText;
            Areas = source.Areas;
        }

        internal static ResponseRichMenu CreateFrom(dynamic dynamicObject)
        {

            var areas = new List<ActionArea>();
            foreach (var area in dynamicObject?.areas ?? Enumerable.Empty<dynamic>())
            {
                areas.Add(ActionArea.CreateFrom(area));
            }

            var menu = new RichMenu()
            {
                Name = (string)dynamicObject?.name,
                Size = new ImagemapSize((int)(dynamicObject?.size?.width ?? 0), (int)(dynamicObject?.size?.height ?? 0)),
                Selected = (bool)(dynamicObject?.selected ?? false),
                ChatBarText = (string)dynamicObject?.chatBarText,
                Areas = areas
            };
            return new ResponseRichMenu((string)dynamicObject?.richMenuId, menu);
        }
    }
}
