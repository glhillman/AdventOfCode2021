using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    internal class DayClass
    {
        List<string> _vectors = new List<string>();
        const int SIZE = 1000;
        int[,] _matrix;

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            InitMatrix();

            long rslt = 0;
            foreach (string vector in _vectors)
            {
                int x1;
                int y1;
                int x2;
                int y2;

                if (ParseVector(vector, out x1, out y1, out x2, out y2, true))
                {
                    MarkLine(x1, y1, x2, y2);
                }
            }

            rslt = CountOverlaps();

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            InitMatrix();

            long rslt = 0;
            foreach (string vector in _vectors)
            {
                int x1;
                int y1;
                int x2;
                int y2;

                ParseVector(vector, out x1, out y1, out x2, out y2, false);
                MarkLine(x1, y1, x2, y2);
            }

            rslt = CountOverlaps();

            Console.WriteLine("Part2: {0}", rslt);
        }

        private bool ParseVector(string vector, out int x1, out int y1, out int x2, out int y2, bool noDiags)
        {
            bool isValid = false;
            string[] vs = vector.Split("->");
            string[] xy1 = vs[0].Trim().Split(',');
            string[] xy2 = vs[1].Trim().Split(',');
            x1 = int.Parse(xy1[0]);
            y1 = int.Parse(xy1[1]);
            x2 = int.Parse(xy2[0]);
            y2 = int.Parse(xy2[1]);

            if (noDiags)
            {
                if (x1 == x2 || y1 == y2) // no diagonals
                {
                    isValid = true;
                }
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        private void InitMatrix()
        {
            _matrix = new int[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    _matrix[i, j] = 0;
                }
            }
        }

        private void MarkLine(int x1, int y1, int x2, int y2)
        {
            int xIncrement = 0;
            int yIncrement = 0;

            // Horizontal
            if (x1 == x2)
            {
                yIncrement = y1 <= y2 ? 1 : -1;
                while (y1 != y2)
                {
                    _matrix[x1, y1]++;
                    y1 += yIncrement;
                }
                _matrix[x1, y1]++;
            }
            else if (y1 == y2) // Vertical
            {
                xIncrement = x1 <= x2 ? 1 : -1;
                while (x1 != x2)
                {
                    _matrix[x1, y1]++;
                    x1 += xIncrement;
                }
                _matrix[x1, y2]++;
            }
            else // diagonal
            {
                xIncrement = x1 < x2 ? 1 : -1;
                yIncrement = y1 < y2 ? 1 : -1;

                while (x1 != x2 && y1 != y2)
                {
                    _matrix[x1, y1]++;
                    x1 += xIncrement;
                    y1 += yIncrement;
                }
                _matrix[x2, y2]++;
            }
        }

        private void DumpMatrix()
        {
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    Console.Write(_matrix[x, y] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private int CountOverlaps()
        {
            int count = 0;

            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    if (_matrix[x, y] > 1)
                    {
                        count++;
                    }
                }
            }

            return count;
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
                    _vectors.Add(line);
                }

                file.Close();
            }
        }

    }
}
