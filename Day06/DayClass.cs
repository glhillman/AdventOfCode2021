using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day06
{
    internal class DayClass
    {
        List<Int16> _fish = new List<Int16>();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            Console.WriteLine("Part1: {0}", SpawnFish(80));
        }

        public void Part2()
        {
            Console.WriteLine("Part2: {0}", SpawnFish(256));
        }

        private long SpawnFish(int nDays)
        {
            long[] fishCount = new long[9];

            for (int i = 0; i < _fish.Count; i++)
            {
                fishCount[_fish[i]]++;
            }

            for (int day = 0; day < nDays; day++)
            {
                long newFish = fishCount[0];
                Array.Copy(fishCount, 1, fishCount, 0, 8);
                fishCount[8] = newFish;
                fishCount[6] += newFish;
            }

            return fishCount.Sum();
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
                        _fish.Add(Int16.Parse(part));
                    }
                }

                file.Close();
            }
        }

    }
}
