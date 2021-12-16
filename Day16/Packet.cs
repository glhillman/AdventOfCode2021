using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    internal class Packet
    {
        public Packet(int version, int type)
        {
            Version = version;
            Type = type;
            Literal = 0;
            SubPackets = new List<Packet>();
        }
        public int Version { get; set; }
        public int Type { get; set; }
        public long Literal { get; set; }
        public List<Packet> SubPackets { get; set; }
        public override string ToString()
        {
            return String.Format("Version: {0}, Type: {1}, Literal: {2}, NSubPackets: {3}", Version, Type, Literal, SubPackets.Count);
        }
    }
}
