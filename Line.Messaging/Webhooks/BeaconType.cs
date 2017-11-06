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
        Banner
    }
}
