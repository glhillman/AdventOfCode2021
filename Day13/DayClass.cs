using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    internal class DayClass
    {
        List<Tuple<int, int>> _points;
        int _maxX = int.MinValue;
        int _maxY = int.MinValue;
        char[,] _paper;
        List<Tuple<char, int>> _folds;

        public DayClass()
        {
            _points = new List<Tuple<int,int>>();
            _folds = new List<Tuple<char, int>>();
            LoadData();
            _paper = new char[++_maxX, ++_maxY];

            for (int y = 0; y < _maxY; y++)
                for (int x = 0; x < _maxX; x++)
                    _paper[x, y] = '.';

            foreach (Tuple<int, int> point in _points)
            {
                _paper[point.Item1, point.Item2] = '#';
            }

            //DumpPaper();
        }

        public void Part1And2()
        {

            long part1Rslt = 0;
            bool part1 = true;

            foreach (Tuple<char, int> fold in _folds)
            {
                if (fold.Item1 == 'x')
                {
                    FoldLeft(fold.Item2);
                }
                else
                {
                    FoldUp(fold.Item2);
                }
                if (part1)
                {
                    part1Rslt = CountDots();
                    part1 = false;
                }
            }

            DumpPaper();

            Console.WriteLine("Part1: {0}", part1Rslt);
            // Part 2, when dumped, is HKUJGAJZ
        }

        private void FoldUp(int yAnchor)
        {
            int ySrc = yAnchor + 1;
            int yDst = yAnchor - 1;
            while (yDst >= 0)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    if (_paper[x, yDst] == '.')
                    {
                        if (ySrc < _maxY)
                        {
                            _paper[x, yDst] = _paper[x, ySrc];
                        }
                    }
                }
                yDst--;
                ySrc++;
            }
            _maxY = yAnchor;
        }

        private void FoldLeft(int xAnchor)
        {
            int xSrc = xAnchor + 1;
            int xDst = xAnchor - 1;
            while (xDst >= 0)
            {
                for (int y = 0; y < _maxY; y++)
                {
                    if (_paper[xDst, y] == '.')
                    {
                        if (xSrc < _maxX)
                        {
                            _paper[xDst, y] = _paper[xSrc, y];
                        }
                    }
                }
                xDst--;
                xSrc++;
            }
            _maxX = xAnchor;
        }

        private int CountDots()
        {
            int count = 0;
            for (int y = 0; y < _maxY; y++)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    count += _paper[x, y] == '#' ? 1 : 0;
                }
            }

            return count;
        }

        private void DumpPaper()
        {
            for (int y =0; y < _maxY; y++)
            {
                for (int x =0; x < _maxX; x++)
                {
                    char c = _paper[x, y];
                    Console.Write(c == '.' ? ' ' : c);
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
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Trim().Length == 0)
                    {
                        break;
                    }
                    string[] parts = line.Split(',');
                    Tuple<int,int> point = Tuple.Create(int.Parse(parts[0]), int.Parse(parts[1]));
                    _maxX = Math.Max(_maxX, point.Item1);
                    _maxY = Math.Max(_maxY, point.Item2);
                    _points.Add(point);
                }
                while ((line = file.ReadLine()) != null)
                {
                    // process fold instructions
                    string[] parts = line.Split(' ');
                    string[] foldParts = parts[2].Split('=');
                    
                    _folds.Add(Tuple.Create(foldParts[0][0], int.Parse(foldParts[1])));
                }

                file.Close();
            }
        }

    }
}
