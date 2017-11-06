using System;

namespace Line.Messaging
{
    /// <summary>
    /// Capture Invalid Signature Exception.
    /// </summary>
    public class InvalidSignatureException : Exception
    {
        public InvalidSignatureException()
        {
        }

        public InvalidSignatureException(string message) : base(message)
        {
        }

        public InvalidSignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
