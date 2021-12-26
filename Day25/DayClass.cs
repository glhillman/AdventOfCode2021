using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    internal class DayClass
    {
        char[,] _grid;
        int _NRows;
        int _NColumns;

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {

            int moves;
            int steps = 0;

            do
            {
                moves = MoveEast();
                moves += MoveSouth();
                steps += moves > 0 ? 1 : 0;
            }
            while (moves > 0);
            Console.WriteLine("Part1: {0}", ++steps);
        }

        public void Part2()
        {

            long rslt = 0;

            Console.WriteLine("Part2: {0}", rslt);
        }

        private int MoveEast()
        {
            int moves = 0;

            for (int row = 0; row < _NRows; row++)
            {
                bool col0IsBlocked = false;
                for (int col = 0; col < _NColumns; col++)
                {
                    if (col == 0 && _grid[row, col] == '>')
                    {
                        col0IsBlocked = true;
                    }
                    int targetCol = (col + 1) % _NColumns;
                    if (!(targetCol == 0 && col0IsBlocked))
                    {
                        if (_grid[row, col] == '>' && _grid[row, targetCol] == '.')
                        {
                            _grid[row, targetCol] = '>';
                            _grid[row, col] = '.';
                            if (targetCol > 0)
                            {
                                col = targetCol; // keep from scooting this one further East
                            }
                            moves++;
                        }
                    }
                }
            }
            return moves;
        }

        private int MoveSouth()
        {
            int moves = 0;

            for (int col = 0; col < _NColumns; col++)
            {
                bool row0IsBlocked = false;
                for (int row = 0; row < _NRows; row++)
                {
                    if (row == 0 && _grid[row, col] == 'v')
                    {
                        row0IsBlocked = true;
                    }
                    int targetRow = (row + 1) % _NRows;
                    if (!(targetRow == 0 && row0IsBlocked))
                    {
                        if (_grid[row, col] == 'v' && _grid[targetRow, col] == '.')
                        {
                            _grid[targetRow, col] = 'v';
                            _grid[row, col] = '.';
                            if (targetRow > 0)
                            {
                                row = targetRow;
                            }
                            moves++;
                        }
                    }
                }
            }
            return moves;
        }


        private void DumpGrid(int step)
        {
            Console.WriteLine("Step: {0}", step);
            for (int row = 0; row < _NRows; row++)
            {
                Console.Write("Row: {0} ", row);
                for (int col = 0; col < _NColumns; col++)
                {
                    Console.Write(_grid[row, col]);
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
                List<string> lines = new List<string>();
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                _NRows = lines.Count;
                _NColumns = lines[0].Length;

                _grid = new char[_NRows, _NColumns];

                for (int row = 0; row < _NRows; row++)
                {
                    for (int col = 0; col < _NColumns; col++)
                    {
                        _grid[row, col] = lines[row][col];
                    }
                }
                file.Close();
            }
        }

    }
}
