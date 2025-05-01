namespace Ntp
{
    /// <summary>
    /// Indicates whether an impending leap second is to be inserted or deleted in the last minute of the current day.
    /// </summary>
    public enum LeapIndicator : byte
    {
        /// <summary>
        /// No warning will be given.
        /// </summary>
        NoWarning = 0,

        /// <summary>
        /// Last minute has 61 seconds.
        /// </summary>
        LastMinuteLonger = 1,

        /// <summary>
        /// Last minute has 59 seconds.
        /// </summary>
        LastMinuteShorter = 2,

        /// <summary>
        /// Clock not synchronized.
        /// </summary>
        AlarmCondition = 3
    }
}
