using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    internal class PacketReader
    {
        public PacketReader(BitReader bitReader)
        {
            Bitrdr = bitReader;
        }

        public BitReader Bitrdr { get; private set; }

        public Packet ReadPacket()
        {
            int version = Bitrdr.ReadInt(3);
            int type = Bitrdr.ReadInt(3);

            Packet packet = new Packet(version, type);

            if (type == 4) // literal
            {
                long literal = 0;
                int group = Bitrdr.ReadInt(1);
                while (group == 1)
                {
                    literal <<= 4;
                    literal += Bitrdr.ReadInt(4);
                    group = Bitrdr.ReadInt(1);
                }
                literal <<= 4;
                literal += Bitrdr.ReadInt(4);
                packet.Literal = literal;
            }
            else // operator packet
            {
                int lengthTypeID = Bitrdr.ReadInt(1);
                if (lengthTypeID == 0)
                {
                    int totalLength = Bitrdr.ReadInt(15);
                    int indexEnd = Bitrdr.Index + totalLength;
                    while (Bitrdr.Index < indexEnd)
                    {
                        packet.SubPackets.Add(ReadPacket());
                    }
                }
                else
                {
                    int nSubPackets = Bitrdr.ReadInt(11);
                    for (int i = 0; i < nSubPackets; i++)
                    {
                        packet.SubPackets.Add(ReadPacket());
                    }
                }
            }

            return packet;
        }

        public void ResetIndex()
        {
            Bitrdr.ResetIndex();
        }

    }
}
