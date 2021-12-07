using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    internal class DayClass
    {
        List<int> _subPositions = new List<int>();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            int minFuel = int.MaxValue;
            int positionsMax = _subPositions.Max();

            for (int i = 0; i < positionsMax; i++)
            {
                int target = i;
                int fuel = 0;
                for (int j = 0; j < _subPositions.Count; j++)
                {
                    fuel += Math.Abs(target - _subPositions[j]);
                }
                minFuel = Math.Min(minFuel, fuel);
            }
            Console.WriteLine("Part1: {0}", minFuel);
        }

        public void Part2()
        {
            int minFuel = int.MaxValue;
            int positionsMax = _subPositions.Max();

            for (int i = 0; i < positionsMax; i++) // check every possibility from 0 to Max number in positions
            {
                int target = i;
                int fuel = 0;
                for (int j = 0; j < _subPositions.Count; j++)
                {
                    int delta = Math.Abs(target - _subPositions[j]);
                    fuel += (delta * (delta + 1)) / 2; // sum of digits formula
                }
                minFuel = Math.Min(minFuel, fuel);
            }
            Console.WriteLine("Part1: {0}", minFuel);
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
                    string[] parts = line.Split(',');
                    foreach (string part in parts)
                    {
                        _subPositions.Add(int.Parse(part));
                    }
                }

                file.Close();
            }
        }

    }
}
