using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    internal class DayClass
    {
        List<Cave> _caves;
        public DayClass()
        {
            _caves = new List<Cave>();
            LoadData();
        }

        public void Part1()
        {

            Cave startCave = _caves.First(c => c.Name == "start");
            List<string> visited = new List<string>();
            Stack<string> currentPath = new Stack<string>();
            
            int nPaths = TraversePaths(startCave, visited, currentPath, false);

            Console.WriteLine("Part1: {0}", nPaths);
        }

        public void Part2()
        {
            Cave startCave = _caves.First(c => c.Name == "start");
            List<string> visited = new List<string>();
            Stack<string> currentPath = new Stack<string>();

            int nPaths = TraversePaths(startCave, visited, currentPath, true);

            Console.WriteLine("Part1: {0}", nPaths);
        }

         public int TraversePaths(Cave startCave, List<string> visited, Stack<string> currentPath, bool duplicateOK)
        {
            int count = 0;

            int visitedCount = visited.Count(s => s == startCave.Name);
            if (visitedCount == 2 || (visitedCount == 1 && duplicateOK == false))
            {
                return count;
            }

            if (visited.Contains(startCave.Name) == false || duplicateOK)
            {
                if (startCave.IsSmall)
                {
                    if (visited.Contains(startCave.Name))
                    {
                        duplicateOK = false;
                    }
                    visited.Add(startCave.Name);
                }
                currentPath.Push(startCave.Name);
                if (startCave.Name == "end")
                {
                    count = 1;
                    currentPath.Pop();
                }
                else
                {
                    foreach (Cave c in startCave.Caves)
                    {
                        count += TraversePaths(c, visited, currentPath, duplicateOK);
                    }
                    currentPath.Pop();
                    int index = visited.IndexOf(startCave.Name);
                    if (index >= 0)
                    {
                        visited.RemoveAt(visited.IndexOf(startCave.Name));
                        index = visited.IndexOf(startCave.Name);
                        if (index >= 0)
                        {
                            duplicateOK = true;
                        }
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
                    string[] parts = line.Split('-');
                    Cave leftCave;
                    Cave rightCave;
                    leftCave = _caves.FirstOrDefault(c => c.Name == parts[0]);
                    rightCave = _caves.FirstOrDefault(c => c.Name == parts[1]);
                    if (leftCave == null)
                    {
                        leftCave = new Cave(parts[0]);
                        _caves.Add(leftCave);
                    }
                    if (rightCave == null)
                    {
                        rightCave = new Cave(parts[1]);
                        _caves.Add(rightCave);
                    }
                    if (leftCave.Name != "end" && rightCave.Name != "start")
                    {
                        leftCave.AddConnectedCave(rightCave);
                    }
                    if (leftCave.Name != "start" && rightCave.Name != "end")
                    {
                        rightCave.AddConnectedCave(leftCave);
                    }
                }

                file.Close();
            }
        }

    }
}
