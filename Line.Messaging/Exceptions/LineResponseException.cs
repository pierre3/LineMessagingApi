using System;
using System.Net;

namespace Line.Messaging
{
    /// <summary>
    /// Capture Error from LINE platform
    /// </summary>
    public class LineResponseException : Exception
    {
        /// <summary>
        /// HTTP Status Code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Error returned from LINE Platform
        /// </summary>
        public ErrorResponseMessage ResponseMessage { get; set; }

        public LineResponseException()
        {}

        public LineResponseException(string message) : base(message)
        {}

        public LineResponseException(string message, Exception innerException) : base(message, innerException)
        {}

        public override string ToString()
        {
            return $"StatusCode={StatusCode}, {ResponseMessage}" + Environment.NewLine
                + base.ToString();
        }
    }
}
