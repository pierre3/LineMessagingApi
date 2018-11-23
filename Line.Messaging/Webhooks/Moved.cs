using System.Collections.Generic;

namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Joined or left Members
    /// </summary>
    public class Moved
    {
        /// <summary>
        /// Joined or left Members
        /// </summary>
        public IList<WebhookEventSource> Members { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="members">
        /// Joined or left Members
        /// </param>
        public Moved(IList<WebhookEventSource> members)
        {
            Members = members;
        }
    }
}
