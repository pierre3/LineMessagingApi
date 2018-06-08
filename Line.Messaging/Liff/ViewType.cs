using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging.Liff
{
    /// <summary>
    /// Size of the LIFF app view. Specify one of the following values
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// 50% of the screen height of the device. This size can be specified only for the chat screen.
        /// </summary>
        Compact,

        /// <summary>
        /// 80% of the screen height of the device. This size can be specified only for the chat screen.
        /// </summary>
        Tall,

        /// <summary>
        /// 100% of the screen height of the device. This size can be specified for any screens in the LINE app. 
        /// </summary>
        Full
    }
}
