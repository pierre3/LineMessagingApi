using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging.Liff
{
    /// <summary>
    /// View object which contains the URL and view size of the LIFF app.
    /// </summary>
    public class View
    {
        /// <summary>
        /// Size of the LIFF app view. Specify one of the following values
        /// </summary>
        public ViewType Type { get; }

        /// <summary>
        /// URL of the LIFF app. Must start with HTTPS.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">
        /// Size of the LIFF app view. Specify one of the following values
        /// </param>
        /// <param name="url">
        /// URL of the LIFF app. Must start with HTTPS.
        /// </param>
        public View(ViewType type, string url)
        {
            Type = type;
            Url = url;
        }
    }
}
