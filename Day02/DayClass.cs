using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    internal class DayClass
    {
        List<Tuple<char, int>> _positions = new List<Tuple<char, int>>();
        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {

            long rslt = 0;
            int horizontal = 0;
            int depth = 0;

            foreach (Tuple<char, int> pair in _positions)
            {
                switch (pair.Item1)
                {
                    case 'u':
                        depth -= pair.Item2;
                        break;
                    case 'd':
                        depth += pair.Item2;
                        break;
                    case 'f':
                        horizontal += pair.Item2;
                        break;
                    default:
                        break;
                }
            }

            rslt = horizontal * depth;

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;
            int horizontal = 0;
            int depth = 0;
            int aim = 0;

            foreach (Tuple<char, int> pair in _positions)
            {
                switch (pair.Item1)
                {
                    case 'u':
                        aim -= pair.Item2;
                        break;
                    case 'd':
                        aim += pair.Item2;
                        break;
                    case 'f':
                        horizontal += pair.Item2;
                        depth += aim * pair.Item2;
                        break;
                    default:
                        break;
                }
            }

            rslt = horizontal * depth;

            Console.WriteLine("Part2: {0}", rslt);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string? line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length == 2)
                    {
                        _positions.Add(Tuple.Create(parts[0][0], int.Parse(parts[1])));
                    }
                }

                file.Close();
            }
        }

    }
}
