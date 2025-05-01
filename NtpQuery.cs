using System.Text;

namespace Ntp
{
    public struct NtpQuery
    {
        /// <summary>
        /// Indicates whether an impending leap second is to be inserted or deleted in the last minute of the current day.
        /// </summary>
        public LeapIndicator LeapIndicator { get; internal set; }

        /// <summary>
        /// The NTP/SNTP version number.
        /// </summary>
        public VersionNumber Version { get; internal set; }

        /// <summary>
        /// The distance of the server from the reference clock.
        /// </summary>
        public Stratum Stratum { get; internal set; }

        /// <summary>
        /// Maximum interval between successive messages.
        /// </summary>
        public TimeSpan PollInterval { get; internal set; }

        /// <summary>
        /// System clock precision in seconds.
        /// </summary>
        public double Precision { get; internal set; }

        /// <summary>
        /// Total round-trip delay from the server to the reference clock.
        /// </summary>
        public TimeSpan RootDelay { get; internal set; }

        /// <summary>
        /// Maximum error in the reference clock.
        /// </summary>
        public TimeSpan RootDispersion { get; internal set; }

        /// <summary>
        /// Identifies the specific server or reference clock.
        /// </summary>
        public uint ReferenceIdentifier { get; internal set; }

        /// <summary>
        /// The time the servers clock was last set or corrected.
        /// </summary>
        public DateTime ReferenceTimestamp { get; internal set; }

        /// <summary>
        /// The client time when the request was sent.
        /// </summary>
        public DateTime OriginateTimestamp { get; internal set; }

        /// <summary>
        /// The server time when the request was received.
        /// </summary>
        public DateTime ReceiveTimestamp { get; internal set; }

        /// <summary>
        /// The server time when the response was sent.
        /// </summary>
        public DateTime TransmitTimestamp { get; internal set; }

        /// <summary>
        /// Total round-trip delay of the NTP query.
        /// </summary>
        public TimeSpan RoundTripDelay { get; internal set; }

        /// <summary>
        /// The one-way time it took for the request to travel from the server to the client.
        /// </summary>
        public TimeSpan Delay { get; internal set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Leap Indicator : {LeapIndicator}");
            stringBuilder.AppendLine($"Version Number : {Version}");
            stringBuilder.AppendLine($"Stratum : {Stratum}");
            stringBuilder.AppendLine($"Poll Interval: {PollInterval}");
            stringBuilder.AppendLine($"Precision : {Precision}");
            stringBuilder.AppendLine($"Root Delay : {RootDelay}");
            stringBuilder.AppendLine($"Root Dispersion : {RootDispersion}");
            stringBuilder.AppendLine($"Reference Identifier : 0x{ReferenceIdentifier.ToString("X")}");
            stringBuilder.AppendLine($"Reference Timestamp : {ReferenceTimestamp.ToString("M/dd/yyyy h:mm:ss.fffffff tt")} UTC");
            stringBuilder.AppendLine($"Originate Timestamp : {OriginateTimestamp.ToString("M/dd/yyyy h:mm:ss.fffffff tt")} UTC");
            stringBuilder.AppendLine($"Receive Timestamp : {ReceiveTimestamp.ToString("M/dd/yyyy h:mm:ss.fffffff tt")} UTC");
            stringBuilder.AppendLine($"Transmit Timestamp : {TransmitTimestamp.ToString("M/dd/yyyy h:mm:ss.fffffff tt")} UTC");
            stringBuilder.AppendLine($"Round Trip Delay : {RoundTripDelay}");
            stringBuilder.AppendLine($"Delay : {Delay}");

            return stringBuilder.ToString();
        }
    }
}
