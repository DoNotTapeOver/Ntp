namespace Ntp
{
    internal enum Mode : byte
    {
        Reserved = 0b00000000,
        SymmetricActive = 0b00000001,
        SymmetricPassive = 0b00000010,
        Client = 0b00000011,
        Server = 0b00000100,
        Broadcast = 0b00000101,

        /// <summary>
        /// reserved for NTP control message
        /// </summary>
        ControlMessage = 0b00000110,

        /// <summary>
        /// reserved for private use
        /// </summary>
        Private = 0b00000111
    }
}
