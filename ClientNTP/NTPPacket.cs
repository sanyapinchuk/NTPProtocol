using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientNTP
{
    public class NTPPacket
    {
        public byte First8bits; //First 2 - const, 6 last - PKT_ID
        public const byte Stratum = 0x3;
        public byte Poll; //8 bit request data
        public byte Precision; //8 bit response data
        public uint RootDelay; //32 bit response data
        public uint RefID; //32 bit response data
        public ulong Reference; //64 bit response data
        public ulong Originate; //64 bit request data
        public ulong Receive; //64 bit response data
        public ulong Transmit; //64 bit request data
        public NTPPacket()
        {
            First8bits = 0;
            Poll = 0;
            Precision = 0;
            RootDelay = 0;
            RefID = 0;
            Reference = 0;
            Originate = 0;
            Receive = 0;
            Transmit = 0;
        }
        public byte[] MakePacket(DateTime data)
        {
            byte[] packet = new byte[48];
            byte[] origByte = BitConverter.GetBytes(data.Ticks);
            packet[24] = origByte[0];
            packet[25] = origByte[1];
            packet[26] = origByte[2];
            packet[27] = origByte[3];
            packet[28] = origByte[4];
            packet[29] = origByte[5];
            packet[30] = origByte[6];
            packet[31] = origByte[7];

            Originate = BitConverter.ToUInt32(BitConverter.GetBytes(data.Ticks));
        }
    }
}
