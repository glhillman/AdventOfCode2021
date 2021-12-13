using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    internal class Cave
    {
        public Cave(string name)
        {
            Name = name;
            if (name == "start" || name == "end")
            {
                IsSmall = false;
            }
            else
            {
                IsSmall = name[0] > 'Z' ? true : false; // lower-case letters have higher ascii value than upper-case
            }
            Caves = new List<Cave>();
        }

        public string Name { get; private set; }
        public bool IsSmall { get; private set; }   

        public List<Cave> Caves { get; private set; }

        public void AddConnectedCave(Cave cave)
        {
            Caves.Add(cave);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string caves;
            if (Caves.Count > 0)
            {
                foreach (Cave cave in Caves)
                {
                    sb.Append(cave.Name + ",");
                }
                caves = sb.ToString();
            }
            else
            {
                caves = " ";
            }
            return String.Format("Name: {0}, {1}, n connected: {2} ({3})", Name, IsSmall ? "Small": "Large", Caves.Count, caves.Substring(0, caves.Length-1));
        }
    }
}
