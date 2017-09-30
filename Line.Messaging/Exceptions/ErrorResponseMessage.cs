using System;
using System.Collections.Generic;
using System.Text;

namespace Line.Messaging
{
    public class ErrorResponseMessage
    {
        public string Message { get; set; }
        public IList<ErrorDetails> Details { get; set; }
        public ErrorResponseMessage()
        { }

        public override string ToString()
        {
            return $"{Message},[{string.Join(",", Details)}]";
        }

        public class ErrorDetails
        {
            public string Message { get; set; }
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
