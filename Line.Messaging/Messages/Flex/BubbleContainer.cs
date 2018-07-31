using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class BubbleContainer : IFlexContainer
    {
        public FlexContainerType Type => FlexContainerType.Bubble;
        public ComponentDirection Direction { get; set; }
        public BoxComponent Header { get; set; }
        public ImageComponent Hero { get; set; }
        public BoxComponent Body { get; set; }
        public BoxComponent Footer { get; set; }
        public BubbleStyles Styles { get; set; }
    }
}
