using System;
using System.Net;

namespace Line.Messaging
{
    public class LineResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
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
