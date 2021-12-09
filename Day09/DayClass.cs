using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    internal class DayClass
    {
        char[,] _heightMap;
        List<Tuple<int, int>> _lowPoints;
        int _nRows = 0;
        int _nCols = 0;

        public DayClass()
        {
            LoadData();
            _lowPoints = new List<Tuple<int, int>>();
            //DumpMap();
        }

        public void Part1()
        {

            int riskSum = 0;
            for (int row = 1; row < _nRows-1; row++)
            {
                for (int col = 1; col < _nCols-1; col++)
                {
                    char anchor = _heightMap[row, col];
                    if (anchor < _heightMap[row-1, col] && anchor < _heightMap[row+1, col] &&
                        anchor < _heightMap[row, col-1] && anchor < _heightMap[row, col+1])
                    {
                        _lowPoints.Add(new Tuple<int, int>(row, col));
                        riskSum += int.Parse(anchor.ToString()) + 1;
                    }
                }
            }

            Console.WriteLine("Part1: {0}", riskSum);
        }

        public void Part2()
        {

            long rslt = 0;
            List<int> basinSize = new List<int>();

            foreach (Tuple<int, int> point in _lowPoints)
            {
                basinSize.Add(BasinSize(point.Item1, point.Item2));
            }

            basinSize.Sort();
            int index = basinSize.Count - 3;
            rslt = 1;
            for (int i = 0; i < 3; i++)
            {
                rslt *= basinSize[i + index];
            }

            Console.WriteLine("Part2: {0}", rslt);
        }

        private int BasinSize(int row, int col)
        {
            int count = 0;

            if (_heightMap[row,col] < '9')
            {
                count++;
                _heightMap[row, col] = 'B'; // for Basin. get it?
                count += BasinSize(row - 1, col);
                count += BasinSize(row + 1, col);
                count += BasinSize(row, col - 1);
                count += BasinSize(row, col + 1);
            }
            
            //DumpMap();

            return count;
        }

        private void DumpMap()
        {
            for (int y = 0; y < _nRows; y++)
            {
                for (int x = 0; x < _nCols; x++)
                {
                    Console.Write(_heightMap[y, x]);
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
                line = file.ReadToEnd();
                string[] lines = line.Split('\n');
                // we're going to pad the outside of the array with '@' sign, so need two extra rows & columns
                _nRows = lines.Count() + 2;
                _nCols = lines[0].Length + 1; // +1 because there is already an extraneous '\r' at the end of all but the last row
                _heightMap = new char[_nRows, _nCols];
                for (int x=0; x < _nCols; x++)
                {
                    _heightMap[0, x] = '@';
                    _heightMap[_nRows-1, x] = '@';
                }
                for (int y = 0; y < _nRows-2; y++)
                {
                    _heightMap[y+1, 0] = '@';
                    _heightMap[y+1, _nCols - 1] = '@';
                    for (int x = 0; x < _nCols-2; x++)
                    {
                        _heightMap[y+1, x+1] = lines[y][x];
                    }
                }
                file.Close();
            }
        }

    }
}
