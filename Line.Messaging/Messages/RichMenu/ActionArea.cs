using System.Collections.Generic;

namespace Line.Messaging
{
    public class ActionArea
    {
        public ImagemapArea Bounds { get; set; }
        public ITemplateAction Action { get; set; }
    }
}
