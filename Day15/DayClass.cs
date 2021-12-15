using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    internal class DayClass
    {
        int _NRows;
        int _NCols;
        int[,] _riskMap;
        record Point(int x, int y);


        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            long rslt = MinCost(_riskMap, _NRows-1, _NCols-1);

            rslt -= _riskMap[0, 0]; // don't count starting square

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            int[,] bigMap = ExpandMap(_riskMap, 5);

            long rslt = Disjktra(bigMap);

            Console.WriteLine("Part2: {0}", rslt);
        }

        /* Returns cost of minimum
            cost path from (0,0) to
            (m, n) in mat[R][C]
           Searches only right & down - no left or up
         */
        private int MinCost(int[,] cost, int m, int n)
        {
            int i, j;
            int[,] tc = new int[m + 1, n + 1];

            tc[0, 0] = cost[0, 0];

            /* Initialize first column of total cost(tc) array */
            for (i = 1; i <= m; i++)
            {
                tc[i, 0] = tc[i - 1, 0] + cost[i, 0];
            }

            /* Initialize first row of tc array */
            for (j = 1; j <= n; j++)
            {
                tc[0, j] = tc[0, j - 1] + cost[0, j];
            }

            /* Construct rest of the tc array */
            for (i = 1; i <= m; i++)
            {
                for (j = 1; j <= n; j++)
                {
                    tc[i, j] = Math.Min(tc[i - 1, j], tc[i, j - 1]) + cost[i, j];
                }
            }

            return tc[m, n];
        }

        public int Disjktra(int[,] grid)
        {
            // this algorithm uses the grid data in a dictionary rather than a grid.
            // load the dictionary from the grid
            Dictionary<Point, int> riskMap = new Dictionary<Point, int>();
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    riskMap[new Point(row, col)] = grid[row, col];
                }
            }

            // Disjktra algorithm

            Point topLeft = new Point(0, 0);
            Point bottomRight = new Point(grid.GetLength(0) - 1, grid.GetLength(1) - 1);

            // Visit points in order of cumulated risk
            PriorityQueue<Point, int> priorityQ = new PriorityQueue<Point, int>();
            Dictionary<Point, int> totalRiskMap = new Dictionary<Point, int>();

            totalRiskMap[topLeft] = 0;
            priorityQ.Enqueue(topLeft, 0);

            // Go until we find the bottom right corner
            while (true)
            {
                Point point = priorityQ.Dequeue();

                if (point == bottomRight)
                {
                    break;
                }

                foreach (Point p in Neighbours(point))
                {
                    if (riskMap.ContainsKey(p))
                    {
                        int totalRiskThroughP = totalRiskMap[point] + riskMap[p];
                        if (totalRiskThroughP < totalRiskMap.GetValueOrDefault(p, int.MaxValue))
                        {
                            totalRiskMap[p] = totalRiskThroughP;
                            priorityQ.Enqueue(p, totalRiskThroughP);
                        }
                    }
                }
            }

            // return bottom right corner's total risk:
            return totalRiskMap[bottomRight];
        }

        IEnumerable<Point> Neighbours(Point point) =>
            new[] 
        {
           point with {y = point.y + 1},
           point with {y = point.y - 1},
           point with {x = point.x + 1},
           point with {x = point.x - 1},
        };

        private int[,] ExpandMap(int[,] map, int factor)
        {
            // ug - this function is more complicated than it needed to be, but it works & I'm tired of this one
            int[,] bigMap = new int[_NRows * factor, _NCols * factor];

            for (int row = 0; row < _NRows; row++)
            {
                for (int col = 0; col < _NCols; col++)
                {
                    int value = map[row, col];
                    int offset = 0;
                    for (int i = 0; i < factor; i++)
                    {
                        int targetValue = (value + i) % 10;
                        if (targetValue == 0)
                        {
                            offset++;
                        }
                        targetValue += offset;
                        int newCol = col + i * _NCols;
                        bigMap[row, newCol] = targetValue;
                    }

                }
            }
            // the first _NRows of bigMap are created & correct
            // Now just fill in the remaining rows with incremented copies of previous rows in bigmap
            for (int i = 0; i < factor - 1; i++)
            {
                int srcAnchorRow = i * _NRows;
                for (int row = 0; row < _NRows; row++)
                {
                    int srcRow = row + srcAnchorRow;
                    int dstRow = srcRow + _NRows;

                    for (int col = 0; col < _NCols * factor; col++)
                    {
                        int value = bigMap[srcRow, col] + 1;
                        if (value == 10)
                        {
                            value = 1;
                        }
                        bigMap[dstRow, col] = value;
                    }
                }
            }
            return bigMap;
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                StreamReader file = new StreamReader(inputFile);
                string[] lines = file.ReadToEnd().Split('\n');
                _NRows = lines.Count();
                _NCols = lines[0].Length - 1;
                _riskMap = new int[_NRows, _NCols];

                for (int r = 0; r < _NRows; r++)
                {
                    for (int c = 0; c < _NCols; c++)
                    {
                        _riskMap[r, c] = lines[r][c] - '0';
                    }
                }

                file.Close();
            }
        }
    }
}
