using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    record Position(int row, int col);
    record LegalMove(Position pos, int steps);
    record SnapShot(List<Position> positions, int cost);
    internal class DayClass
    {
        GridBase[,] _grid;
        int _holeBottomRow;
        GridBase _gridBaseSpace = new GridBase('.');
        GridBase _gridBaseWall = new GridBase('#');
        GridBase _gridBaseBlank = new GridBase(' ');

        List<Amphipod> _amphipods;
        public DayClass()
        {
        }

        public void Part1()
        {
            LoadData("input.txt");

            int minCost = int.MaxValue;

            ReCurse(0, ref minCost);

            Console.WriteLine("Part1: {0}", minCost);
        }

        public void Part2()
        {
            LoadData("input2.txt");

            int minCost = int.MaxValue;

            ReCurse(0, ref minCost);

            Console.WriteLine("Part2: {0}", minCost);
        }

        private long ReCurse(int cost, ref int minCost)
        {
            Stack<SnapShot> snapShots = new();

            foreach (Amphipod a in _amphipods)
            {
                List<LegalMove> legalMoves = a.FindLegalMoves();
                if (legalMoves.Count > 0)
                {
                    foreach (LegalMove legalMove in legalMoves)
                    {
                        snapShots.Push(GetSnapShot(cost));
                        cost += a.Move(legalMove);

                        // move every amphipod that can go home to their hole
                        bool moved;
                        do
                        {
                            moved = false;
                            foreach (Amphipod a2 in _amphipods)
                            {
                                LegalMove? homeMove = a2.FindHomeMove();
                                if (homeMove != null)
                                {
                                    cost += a2.Move(homeMove);
                                    moved = true;
                                }
                            }
                        } while (moved);

                        if (GridComplete())
                        {
                            // every amphipod is in their proper hole. Done with this path
                            minCost = Math.Min(cost, minCost);
                            cost = RestoreSnapShot(snapShots.Pop());
                            continue;
                        }
                        else
                        {
                            if (cost < minCost)
                            {
                                snapShots.Push(GetSnapShot(cost));
                                ReCurse(cost, ref minCost);
                                cost = RestoreSnapShot(snapShots.Pop());
                            }
                            else
                            {
                                continue; // already costs more than the current minimum. bail out
                            }

                        }
                        cost = RestoreSnapShot(snapShots.Pop());
                    }
                }
            }

            return cost;
        }

        private bool GridComplete()
        {
            return _amphipods.Count(a => a.IsHome) == _amphipods.Count();
        }

        private SnapShot GetSnapShot(int cost)
        {
            List<Position> positions = new();
            foreach (Amphipod a in _amphipods)
            {
                positions.Add(a.CurrentRowCol);
            }
            return new SnapShot(positions, cost);
        }

        private int RestoreSnapShot(SnapShot snapShot)
        {
            // blank out the Grid
            for (int col = 1; col <= 11; col++)
            {
                _grid[1, col] = _gridBaseSpace;
            }

            for (int row = 2; row < _holeBottomRow; row++)
            {
                _grid[row, 3] = _gridBaseSpace;
                _grid[row, 5] = _gridBaseSpace;
                _grid[row, 7] = _gridBaseSpace;
                _grid[row, 9] = _gridBaseSpace;
            }

            for (int i = 0; i < snapShot.positions.Count; i++)
            {
                _amphipods[i].CurrentRowCol = snapShot.positions[i];
                _grid[_amphipods[i].CurrentRowCol.row, _amphipods[i].CurrentRowCol.col] = _amphipods[i];
            }

            return snapShot.cost;
        }

        private void DumpGrid()
        {
            for (int row = 0; row < _grid.GetLength(0); row++)
            {
                for (int col = 0; col < _grid.GetLength(1); col++)
                {
                    Console.Write(_grid[row, col].Id);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void LoadData(string fileName)
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\" + fileName;

            if (File.Exists(inputFile))
            {
                string[] lines = File.ReadAllLines(inputFile);
                _grid = new GridBase[lines.Length, lines[0].Length];

                int maxLen = int.MinValue;

                for (int row = 0; row < lines.Length; row++)
                {
                    maxLen = Math.Max(maxLen, lines[row].Length);
                    for (int col = 0; col < lines[row].Length; col++)
                    {
                        switch(lines[row][col])
                        {
                            case '#':
                                _grid[row,col] = _gridBaseWall;
                                break;
                            case '.':
                                _grid[row,col] = _gridBaseSpace;
                                break;
                            case ' ':
                                _grid[row,col] = _gridBaseBlank;
                                break;
                            case 'A':
                                _grid[row,col] = new Amber(new Position(row, col));
                                break;
                            case 'B':
                                _grid[row,col] = new Bronze(new Position(row, col));
                                break;
                            case 'C':
                                _grid[row,col] = new Copper(new Position(row, col));
                                break;
                            case 'D':
                                _grid[row,col] = new Desert(new Position(row, col));
                                break;
                            default:
                                throw new ArgumentException("Unexpected input character");
                        }
                    }
                    if (lines[row].Length < maxLen)
                    {
                        int col = lines[row].Length;
                        while (col < maxLen)
                        {
                            _grid[row, col++] = _gridBaseBlank;
                        }
                    }
                }

                Amphipod.Grid = _grid;
                _holeBottomRow = _grid.GetLength(0) - 2;

                _amphipods = new List<Amphipod>();

                for (int row = 0; row < _grid.GetLength(0); row++)
                {
                    for (int col = 0; col < _grid.GetLength(1); col++)
                    {
                        if (_grid[row, col].IsAmphipod)
                        {
                            _amphipods.Add(_grid[row, col] as Amphipod);
                        }
                    }
                }

            }
        }

    }
}
