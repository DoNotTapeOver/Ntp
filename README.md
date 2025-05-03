## NTPClient
Provides a simple way to query a NTP server for time related data.
### Example
```
    NtpClient ntpClient = new NtpClient("192.168.1.1");

    CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    NtpResponse ntpResponse = await ntpClient.GetTimeAsync(cts.Token);

    DateTime localTime = ntpResponse.TransmitTimestamp.ToLocalTime();
```
### References
- Simple Network Time Protocol (SNTP) Version 4 for IPv4, IPv6 and OSI
[link](https://www.rfc-editor.org/rfc/rfc2030) |  [archive.org](https://web.archive.org/web/20250331232623/https://www.rfc-editor.org/rfc/rfc2030)
- Network Time Protocol Version 4: Protocol and Algorithms Specification
[link](https://datatracker.ietf.org/doc/html/rfc5905) | [archive.org](https://web.archive.org/web/20250422194538/https://datatracker.ietf.org/doc/html/rfc5905)
- Understanding and using the Network Time Protocol
[link](https://www.eecis.udel.edu/~ntp/ntpfaq/NTP-s-algo.htm) | [archive.org](https://web.archive.org/web/20240514073047/https://www.eecis.udel.edu/~ntp/ntpfaq/NTP-s-algo.htm)
- Handcrafting NTP Requests
[link](https://jraviles.com/ntp/2020/10/10/handcrafting-ntp-requests.html) | [archive.org](https://web.archive.org/web/20250430231934/https://jraviles.com/ntp/2020/10/10/handcrafting-ntp-requests.html)
- A Very Short Introduction to NTP Timestamps
[link](https://tickelton.gitlab.io/articles/ntp-timestamps/) | [archive.org](https://web.archive.org/web/20240401020342/https://tickelton.gitlab.io/articles/ntp-timestamps/)
- Converting NTP Short Format to Seconds
[link](https://stackoverflow.com/questions/59771370/convert-ntp-short-format-to-seconds) | [archive.org](https://web.archive.org/web/20200208131045/https://stackoverflow.com/questions/59771370/convert-ntp-short-format-to-seconds)
