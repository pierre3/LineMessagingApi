using System.Collections.Generic;

namespace Line.Messaging
{
    /// <summary>
    /// Error returned from LINE Platform
    /// https://developers.line.me/en/docs/messaging-api/reference/#error-responses
    /// </summary>
    public class ErrorResponseMessage
    {
        /// <summary>
        /// Summary of the error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Details of the error
        /// </summary>
        public IList<ErrorDetails> Details { get; set; }

        public ErrorResponseMessage()
        { }

        public override string ToString()
        {
            return $"{Message},[{string.Join(",", Details)}]";
        }

        /// <summary>
        /// Details of the error
        /// </summary>
        public class ErrorDetails
        {
            /// <summary>
            /// Details of the error
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Location of where the error occurred
            /// </summary>
            public string Property { get; set; }

            public ErrorDetails()
            { }

            public override string ToString()
            {
                return $"{{{Message}, {Property}}}";
            }
        }
    }
}
