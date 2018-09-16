using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class BubbleContainer : IFlexContainer
    {
        public FlexContainerType Type => FlexContainerType.Bubble;

        /// <summary>
        /// Text directionality and the order of components in horizontal boxes in the container. <para>
        /// Specify one of the following values:
        /// / ltr: Left to right
        /// / rtl: Right to left
        /// , The default value is ltr.</para>
        /// <para>(Optional)</para>
        /// </summary>
        public ComponentDirection Direction { get; set; }

        /// <summary>
        /// Header block. Specify a box component.
        /// <para>(Optional)</para>
        /// </summary>
        public BoxComponent Header { get; set; }

        /// <summary>
        /// Hero block. Specify an image component.
        /// <para>(Optional)</para>
        /// </summary>
        public ImageComponent Hero { get; set; }

        /// <summary>
        /// Body block. Specify a box component.
        /// <para>(Optional)</para>
        /// </summary>
        public BoxComponent Body { get; set; }

        /// <summary>
        /// Footer block. Specify a box component.
        /// <para>(Optional)</para>
        /// </summary>
        public BoxComponent Footer { get; set; }

        /// <summary>
        /// Style of each block. Specify a bubble style object. For more information, see Objects for the block style.
        /// <para>(Optional)</para>
        /// </summary>
        public BubbleStyles Styles { get; set; }
    }
}
