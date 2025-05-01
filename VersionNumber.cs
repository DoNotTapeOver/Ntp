namespace Ntp
{
    /// <summary>
    /// The NTP/SNTP version number.
    /// </summary>
    public enum VersionNumber : byte
    {
        NTPv1 = 1,

        /// <summary>
        ///  IPv4 support only.
        /// </summary>
        NTPv3 = 3, //0b00011000,

        /// <summary>
        /// IPv4, IPv6 and OSI support.
        /// </summary>
        NTPv4 = 4 //0b00100000
    }
}
