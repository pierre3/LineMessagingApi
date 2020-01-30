namespace Line.Messaging.Webhooks
{
    public enum BeaconType
    {
        /// <summary>
        /// Entered beacon's reception range
        /// </summary>
        Enter,
        /// <summary>
        /// Left beacon's reception range
        /// </summary>
        Leave,
        /// <summary>
        /// Tapped beacon banner 
        /// </summary>
        Banner,
        /// <summary>
        /// A user is in the beacon's reception range.
        /// This event is sent repeatedly at a minimum of 10 seconds.
        /// </summary>
        Stay
    }
}
