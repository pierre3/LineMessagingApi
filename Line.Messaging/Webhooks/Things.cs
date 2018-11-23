namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// LINE Things
    /// </summary>
    public class Things
    {
        /// <summary>
        /// Device ID of the LINE Things-compatible device that was linked with LINE
        /// </summary>
        public string DeviceId { get; }

        /// <summary>
        /// Link or Unlink
        /// </summary>
        public ThingsType Type { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceId">Device ID of the LINE Things-compatible device that was linked with LINE</param>
        /// <param name="type">Link or Unlink</param>
        public Things(string deviceId, ThingsType type)
        {
            DeviceId = deviceId;
            Type = type;
        }

    }
}
