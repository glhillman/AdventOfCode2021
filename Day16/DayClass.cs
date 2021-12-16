using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    internal class DayClass
    {
        PacketReader _packetReader;
        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            Packet packet = _packetReader.ReadPacket();
            long sum = SumPacketVersions(packet);
            Console.WriteLine("Part1: {0}", sum);
        }

        public void Part2()
        {
            _packetReader.ResetIndex();
            Packet packet = _packetReader.ReadPacket();
            List<long> values = new List<long>();
            long rslt = EvaluatePackets(packet, values);
            Console.WriteLine("Part2: {0}", rslt);
        }

        private int SumPacketVersions(Packet packet)
        {
            int sum = packet.Version;
            foreach (Packet subPacket in packet.SubPackets)
            {
                sum += SumPacketVersions(subPacket);
            }

            return sum;
        }

        private long EvaluatePackets(Packet packet, List<long> values)
        {
            switch (packet.Type)
            {
                case 0: // sum
                    values.Add(GetSubValues(packet).Sum());
                    break;
                case 1: // product
                    values.Add(GetSubValues(packet).Aggregate((prod, value) => prod * value));
                    break;
                case 2: // minimum
                    values.Add(GetSubValues(packet).Min());
                    break;
                case 3: // maximum
                    values.Add(GetSubValues(packet).Max());
                    break;
                case 4: // Literal
                    values.Add(packet.Literal);
                    break;
                case 5: // greater than
                    values.Add(ComparisonPackets(packet, '>'));
                    break;
                case 6: // less than
                    values.Add(ComparisonPackets(packet, '<'));
                    break;
                case 7: // Equal
                    values.Add(ComparisonPackets(packet, '='));
                    break;
                default:
                    Console.WriteLine("Invalid packet type {0}", packet.Type);
                    break;
            }

            return values.Count > 0 ? values[0] : 0;
        }

        private long ComparisonPackets(Packet p, char operation)
        {
            long rslt = 0;
            List<long> values = GetSubValues(p);
            switch (operation)
            {
                case '>': 
                    rslt = values[0] > values[1] ? 1 : 0;
                    break;
                case '<':
                    rslt = values[0] < values[1] ? 1 : 0;
                    break;
                case '=':
                    rslt = values[0] == values[1] ? 1 : 0;
                    break;
            }

            return rslt;
        }

        private List<long> GetSubValues(Packet p)
        {
            List<long> values = new List<long>();
            foreach (Packet pSub in p.SubPackets)
            {
                EvaluatePackets(pSub, values);
            }

            return values;
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";
            
            if (File.Exists(inputFile))
            {
                string? line;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                file.Close();

                BitReader bitReader = new BitReader(line);
                _packetReader = new PacketReader(bitReader);
            }
        }
    }
}
