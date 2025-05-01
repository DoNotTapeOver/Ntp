namespace Ntp
{
    /// <summary>
    /// Indicates the distance from the reference clock.
    /// <list type="bullet">
    /// <item>0 = Invalid</item>
    /// <item>1 = Primary Server</item>
    /// <item>2 - 15 = Secondary Server</item>
    /// <item>16 = Unsynchronized</item>
    /// </list>
    /// </summary>
    public enum Stratum
    {
        Invalid = 0,

        /// <summary>
        /// NTP server is connected directly to a reference clock.
        /// </summary>
        PrimaryServer = 1,
        SecondaryServer_Stratum2 = 2,
        SecondaryServer_Stratum3 = 3,
        SecondaryServer_Stratum4 = 4,
        SecondaryServer_Stratum5 = 5,
        SecondaryServer_Stratum6 = 6,
        SecondaryServer_Stratum7 = 7,
        SecondaryServer_Stratum8 = 8,
        SecondaryServer_Stratum9 = 9,
        SecondaryServer_Stratum10 = 10,
        SecondaryServer_Stratum11 = 11,
        SecondaryServer_Stratum12 = 12,
        SecondaryServer_Stratum13 = 13,
        SecondaryServer_Stratum14 = 14,
        SecondaryServer_Stratum15 = 15,
        Unsynchronized = 16
    }
}
