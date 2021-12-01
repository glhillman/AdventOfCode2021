using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    internal class DayClass
    {
        List<int> _depths = new List<int>();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {

            long rslt = 0;
            for (int i = 1; i < _depths.Count; i++)
            {
                rslt += (_depths[i] > _depths[i-1]) ? 1 : 0;
            }

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;
            int prevSum = _depths[0] + _depths[1] + _depths[2];
            for (int i = 3; i < _depths.Count; i++)
            {
                int sum = _depths[i] + _depths[i - 1] + _depths[i - 2];
                rslt += (sum > prevSum) ? 1 : 0;
                prevSum = sum;
            }
            Console.WriteLine("Part2: {0}", rslt);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    _depths.Add(int.Parse(line));
                }

                file.Close();
            }
        }

    }
}
