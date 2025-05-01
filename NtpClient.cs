using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Ntp
{
    public class NtpClient
    {
        private const byte LeapIndicatorMask = 0b11000000;
        private const byte VersionNumberMask = 0b00111000;

        /// <summary>
        /// The NTP prime epoch is defined as 0h on January 1, 1900.
        /// This date marks the origin of the NTP timescale, and all timestamps in the NTP system are calculated from this point.
        /// </summary>
        public static readonly DateTime NtpPrimeEpoch = new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The port number associated with the NTP protocol.
        /// </summary>
        public const int NtpPort = 123;

        /// <summary>
        /// The NTP protocol version used by this client.
        /// </summary>
        public const VersionNumber NtpVersion = VersionNumber.NTPv4;

        private readonly EndPoint _NtpServerEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="NtpClient"/> class with the specified IP address.
        /// </summary>
        /// <param name="address">The IP address of the NTP server to be queried. </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="address"/> address is less than 0 or greater than <see cref="IPEndPoint.MaxPort"/>.</exception>
        public NtpClient(long address)
        {
            _NtpServerEndpoint = new IPEndPoint(address, NtpPort);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NtpClient"/> class with the specified IP address.
        /// </summary>
        /// <param name="address">An <see cref="IPAddress"/> containing the address of the NTP server to be queried.</param>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> is null.</exception>
        public NtpClient(IPAddress address)
        {
            _NtpServerEndpoint = new IPEndPoint(address, NtpPort);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NtpClient"/> class with the host name or string representation of an IP address.
        /// </summary>
        /// <param name="host">The host name or a string representation of the IP address of the NTP server to be queried.</param>
        /// <exception cref="ArgumentException">The <paramref name="host"/> parameter contains an empty string.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="host"/> parameter is null.</exception>
        public NtpClient(string host)
        {
            _NtpServerEndpoint = new DnsEndPoint(host, NtpPort);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes (in big-endian order) at a specified position in a byte array.
        /// </summary>
        /// <param name="bytes">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="bytes"/>.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="bytes"/> minus 3.</exception>
        private static uint ReadUInt32(byte[] bytes, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
            ArgumentOutOfRangeException.ThrowIfLessThan(startIndex, 0, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, bytes.Length - sizeof(uint), nameof(startIndex));

            // Check if system architecture is little endian.
            if (BitConverter.IsLittleEndian)
            {
                // Swap byte order to convert from big endian.
                return ((uint)bytes[startIndex] << 24) | ((uint)bytes[startIndex + 1] << 16) | ((uint)bytes[startIndex + 2] << 8) | (uint)bytes[startIndex + 3];
            }
            else
            {
                return ((uint)bytes[startIndex + 3] << 24) | ((uint)bytes[startIndex + 2] << 16) | ((uint)bytes[startIndex + 1] << 8) | (uint)bytes[startIndex];
            }
        }

        /// <summary>
        /// Converts a 32-bit NTP Short-Format timestamp at a specified position in a byte array to a TimeSpan.
        /// </summary>
        /// <param name="bytes">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="bytes"/>.</param>
        /// <returns>
        /// A TimeSpan formed by the four continuous bytes beginning at <paramref name="startIndex"/>.
        /// </returns>
        private static TimeSpan ReadTimeSpan(byte[] bytes, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
            ArgumentOutOfRangeException.ThrowIfLessThan(startIndex, 0, nameof(startIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, bytes.Length - sizeof(int), nameof(startIndex));

            // The NTP Short Format is a uint32_t (in big-endian format)
            // The first 16 bits contain seconds.
            // The remaining 16 bits contain fractions of a second. (stored as 1/(2^16) of a second)
            uint timestamp = ReadUInt32(bytes, startIndex);
            ushort timestampSeconds = (ushort)(timestamp >> 16);
            ushort timestampFractional = (ushort)(timestamp);

            double value = (double)timestampSeconds;

            // Divide timestamp fraction by 2^16.
            value += (double)timestampFractional / (double)65536;

            return TimeSpan.FromSeconds(value);
        }

        /// <summary>
        /// Converts a 64-bit NTP timestamp at a specified position in a byte array to a system DateTime.
        /// </summary>
        /// <param name="bytes">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="bytes"/>.</param>
        /// <returns>
        /// A DateTime formed by the eight continuous bytes beginning at <paramref name="startIndex"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="bytes"/> minus 3.</exception>
        private static DateTime ReadDateTime(byte[] bytes, int startIndex)
        {
            // The NTP timestamp is a 64 bit integer  (in big-endian format).
            // The first 32 bits contain seconds.
            // The remaining 32 bits contain fractions of a second. (stored as 1/(2^32) of a second)
            uint timestampSeconds = ReadUInt32(bytes, startIndex);
            uint timestampFractional = ReadUInt32(bytes, startIndex + 4);

            double value = (double)timestampSeconds;

            // Divide timestamp fraction by 2^32.
            value += (double)timestampFractional / (double)4294967296;

            return NtpPrimeEpoch.AddSeconds(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<NtpQuery> GetTimeAsync()
        {
            return await GetTimeAsync(CancellationToken.None);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<NtpQuery> GetTimeAsync(CancellationToken cancellationToken)
        {
            byte[] requestData = new byte[48];
            byte[] responseData = new byte[48];

            // Set ntp request message flags.
            requestData[0] = ((byte)NtpVersion << 3) | (byte)Mode.Client;

            Stopwatch roundtripTimer = new Stopwatch();
            DateTime originateTimestamp;

            using (Socket socket = new Socket(SocketType.Dgram, ProtocolType.Udp))
            {
                ReadOnlyMemory<byte> requestBuffer = new ReadOnlyMemory<byte>(requestData);
                Memory<byte> responseBuffer = new Memory<byte>(responseData);

                await socket.ConnectAsync(_NtpServerEndpoint, cancellationToken);

                originateTimestamp = DateTime.Now;

                roundtripTimer.Start();

                await socket.SendAsync(requestBuffer, SocketFlags.None, cancellationToken);
                await socket.ReceiveAsync(responseBuffer, SocketFlags.None, cancellationToken);

                roundtripTimer.Stop();
            }

            // Get the time the request was received by the server.
            DateTime serverReceiveTimestamp = ReadDateTime(responseData, 32);

            // Get the time the response was sent from the server.
            DateTime serverTransmitTimestamp = ReadDateTime(responseData, 40);

            // Calculate the amount of time it took the server to process the request.
            TimeSpan serverProcessingTime = serverTransmitTimestamp - serverReceiveTimestamp;

            // Estimate the time it took for the request to be transmitted to the server.
            TimeSpan delay = (roundtripTimer.Elapsed - serverProcessingTime) / 2;

            return new NtpQuery()
            {
                LeapIndicator = (LeapIndicator)((responseData[0] & LeapIndicatorMask) >> 6),
                Version = (VersionNumber)((responseData[0] & VersionNumberMask) >> 3),
                Stratum = (Stratum)responseData[1],
                PollInterval = TimeSpan.FromSeconds(Math.Pow(2, responseData[2])),
                Precision = Math.Pow(2, responseData[3]),
                RootDelay = ReadTimeSpan(responseData, 4),
                RootDispersion = ReadTimeSpan(responseData, 8),
                ReferenceIdentifier = ReadUInt32(responseData, 12),
                ReferenceTimestamp = ReadDateTime(responseData, 16),
                OriginateTimestamp = TimeZoneInfo.ConvertTimeToUtc(originateTimestamp, TimeZoneInfo.Local),
                ReceiveTimestamp = serverReceiveTimestamp,
                TransmitTimestamp = serverTransmitTimestamp,
                RoundTripDelay = roundtripTimer.Elapsed,
                Delay = delay
            };
        }
    }
}
