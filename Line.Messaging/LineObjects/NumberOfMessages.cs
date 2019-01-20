namespace Line.Messaging
{
    public class NumberOfSentMessages
    {
        /// <summary>
        /// Status of the counting process. One of the following values is returned:<para>
        /// ready: You can get the number of messages.</para><para>
        /// unready: The message counting process for the date specified in date has not been completed yet.Retry your request later.Normally, the counting process is completed within the next day.</para><para>
        /// out_of_service: The date specified in date is earlier than March 31, 2018, when the operation of the counting system started.</para>
        /// </summary>
        public NumberOfSentMessagesStatus Status { get; set; }

        /// <summary>
        /// The number of messages sent with the Messaging API on the date specified in date. The response has this property only when the value of status is ready.
        /// </summary>
        public int Success { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NumberOfSentMessages()
        {}
    }
}
