using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    internal class DayClass
    {
        const int NRows = 10;
        const int NCols = 10;
        int[,] _data = new int[NRows, NCols];

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {

            int nFlashed = 0;
            
            for (int i = 0; i < 100; i++)
            {
                int nFlashedInCycle = 0;
                Cycle(ref nFlashedInCycle);
                nFlashed += nFlashedInCycle;
            }

            Console.WriteLine("Part1: {0}", nFlashed);
        }

        public void Part2()
        {

            int nCycles = 100; // from step 1 - don't need to reload the data & start over
            int nFlashedInCycle;

            do
            {
                nFlashedInCycle = 0;
                Cycle(ref nFlashedInCycle);
                nCycles++;
            } while (nFlashedInCycle != 100);

            Console.WriteLine("Part2: {0}", nCycles);
        }

        private void Cycle(ref int nFlashed)
        {
            bool needFlash = false;

            for (int row = 0; row < NRows; row++)
            {
                for (int col = 0; col < NCols; col++)
                {
                    _data[row, col] += 1;
                    if (_data[row, col] >= 10)
                    {
                        needFlash = true;
                    }
                }
            }

            if (needFlash)
            {
                FlashAll(ref nFlashed);
            }
        }

        private void FlashAll(ref int nFlashed)
        {
            for (int row = 0; row < NRows; row++)
            {
                for (int col = 0; col < NCols; col++)
                {
                    if (_data[row,col] >= 10)
                    {
                        FlashAt(row, col, ref nFlashed);
                    }
                }
            }
        }

        private void FlashAt(int row, int col, ref int nFlashed)
        {
            nFlashed++;

            // first, mark the anchor point to 0
            _data[row, col] = 0;
            // now increment everything around anchor point. If a value is 0, is has flashed this cycle. Do not increment it.
            Increment(row, col - 1, ref nFlashed); // left
            Increment(row - 1, col - 1, ref nFlashed); // upper left
            Increment(row - 1, col, ref nFlashed); // above
            Increment(row - 1, col + 1, ref nFlashed); // above right
            Increment(row, col + 1, ref nFlashed); // right
            Increment(row + 1, col + 1, ref nFlashed); //right below
            Increment(row + 1, col, ref nFlashed); // below
            Increment(row + 1, col - 1, ref nFlashed); // below left
        }

        private void Increment(int row, int col, ref int nFlashed)
        {
            if (row >= 0 && row < NRows && col >= 0 && col < NCols)
            {
                if (_data[row, col] != 0)
                {
                    _data[row, col]++;
                    if (_data[row, col] >= 10)
                    {
                        FlashAt(row, col, ref nFlashed); // curse you recursion!
                    }
                }
            }
        }

        private void DumpData(int[,] data, string msg)
        {
            Console.WriteLine(msg);
            for (int row = 0; row < data.GetLength(0); row++)
            {
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    Console.Write(string.Format("{0,3}", data[row, col]));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string? line;
                StreamReader file = new StreamReader(inputFile);
                int row = 0;

                while ((line = file.ReadLine()) != null)
                {
                    int col = 0;
                    foreach (char c in line)
                    {
                        _data[row, col++] = int.Parse(c.ToString());
                    }
                    row++;
                }

                file.Close();
            }
        }

    }
}
