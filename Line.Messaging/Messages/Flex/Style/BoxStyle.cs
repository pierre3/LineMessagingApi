using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    /// <summary>
    /// Block Style
    /// </summary>
    public class BlockStyle
    {
        /// <summary>
        /// Background color of the block. Use a hexadecimal color code.
        /// <para>(Optional)</para>
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// true to place a separator above the block.<para> 
        /// true will be ignored for the first block in a container because you cannot place a separator above the first block. 
        /// The default value is false. </para>
        /// <para>(Optional)</para>
        /// </summary>
        
        public bool Separator { get; set; }

        /// <summary>
        /// Color of the separator. Use a hexadecimal color code.
        /// <para>(Optional)</para>
        /// </summary>
        public string SeparatorColor { get; set; }
    }
}
