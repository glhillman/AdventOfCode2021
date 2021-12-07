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
        public void Parts1And2()
        {
            int minFuelPart1 = int.MaxValue;
            int minFuelPart2 = int.MaxValue;
            int positionsMax = _subPositions.Max();

            for (int i = 0; i < positionsMax; i++) // check every possibility from 0 to Max number in positions
            {
                int target = i;
                int fuelPart1 = 0;
                int fuelPart2 = 0;
                for (int j = 0; j < _subPositions.Count; j++)
                {
                    int delta = Math.Abs(target - _subPositions[j]);
                    fuelPart1 += delta;
                    fuelPart2 += (delta * (delta + 1)) / 2; // sum of digits formula
                }
                minFuelPart1 = Math.Min(minFuelPart1, fuelPart1);
                minFuelPart2 = Math.Min(minFuelPart2, fuelPart2);
            }
            Console.WriteLine("Part1: {0}", minFuelPart1);
            Console.WriteLine("Part2: {0}", minFuelPart2);
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
