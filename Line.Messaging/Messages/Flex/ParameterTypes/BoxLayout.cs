using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    /// <summary>
    /// The placement style of components in this box. Specify one of the following values
    /// </summary>
    public enum BoxLayout
    {
        /// <summary>
        /// Components are placed horizontally. The direction property of the bubble container specifies the order.
        /// </summary>
        Horizontal,
        /// <summary>
        /// Components are placed vertically from top to bottom.
        /// </summary>
        Vertical,
        /// <summary>
        /// Components are placed in the same way as horizontal is specified except the baselines of the components are aligned.
        /// </summary>
        Baseline,
    }
}
