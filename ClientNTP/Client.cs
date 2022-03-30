using System;
using System.Net;
using System.Net.Sockets;
class Program
{

    public static void Main()
    {
        string address = "127.0.0.1";
        IPAddress ipAddress = IPAddress.Parse(address);

        while (true)
        {
            var time = GetNetworkTime(ipAddress);
            Console.WriteLine(time);
        }
        
    }
    public static DateTime GetNetworkTime(IPAddress ipAddress)
    {
        //default Windows time server
        const string ntpServer = "time.windows.com";

        // NTP message size - 16 bytes of the digest (RFC 2030)
        var ntpData = new byte[48];

        //Setting the Leap Indicator, Version Number and Mode values
        ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

        //var addresses = Dns.GetHostEntry(ntpServer).AddressList;

        

        //The UDP port number assigned to NTP is 123
        var ipEndPoint = new IPEndPoint(ipAddress, 123);
        //NTP uses UDP

      //  using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
      //  {
            UdpClient udpClient = new UdpClient();
            // socket.Connect(ipEndPoint);

            IPEndPoint remote = null;
            Console.WriteLine("CLIENT: send request: " + ntpData);
            udpClient.Send(ntpData, ntpData.Length, ipEndPoint);

            //UdpClient receiver = new UdpClient();

            byte[] receiveBytes = udpClient.Receive(
                       ref remote);

            long longVar = BitConverter.ToInt64(receiveBytes);
            DateTime dateTimeVar = DateTime.FromBinary(longVar);
        Console.WriteLine("CLIENT: now time: " + dateTimeVar.ToString());
        //return DateTime.ParseExact(receiveBytes,)
        //Console.WriteLine("CLIENT: now time: " + System.Text.Encoding.UTF8.GetString(receiveBytes));

        //Stops code hang if NTP is blocked
        // socket.ReceiveTimeout = 3000;

        /* socket.Send(ntpData);
         socket.Receive(ntpData);
         socket.Close();*/
        // }

        //Offset to get to the "Transmit Timestamp" field (time at which the reply 
        //departed the server for the client, in 64-bit timestamp format."
        const byte serverReplyTime = 40;

        //Get the seconds part
        ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

        //Get the seconds fraction
        ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

        //Convert From big-endian to little-endian
        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

        //**UTC** time
        var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

        return networkDateTime.ToLocalTime();
    }

    static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                       ((x & 0x0000ff00) << 8) +
                       ((x & 0x00ff0000) >> 8) +
                       ((x & 0xff000000) >> 24));
    }
}
