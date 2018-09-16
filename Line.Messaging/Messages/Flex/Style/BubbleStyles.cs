using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    /// <summary>
    /// Use the following two objects to define the style of blocks in a bubble.
    /// </summary>
    public class BubbleStyles
    {
        /// <summary>
        /// Style of the header block
        /// <para>(Optional)</para>
        /// </summary>
        public BlockStyle Header { get; set; }

        /// <summary>
        /// Style of the hero block
        /// <para>(Optional)</para>
        /// </summary>
        public BlockStyle Hero { get; set; }

        /// <summary>
        /// Style of the body block
        /// <para>(Optional)</para>
        /// </summary>
        public BlockStyle Body { get; set; }

        /// <summary>
        /// Style of the footer block
        /// <para>(Optional)</para>
        /// </summary>
        public BlockStyle Footer { get; set; }
    }
}
