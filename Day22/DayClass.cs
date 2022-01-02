using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day22
{
    record CuboidRec(long x1, long x2, long y1, long y2, long z1, long z2, bool isOn);
    internal class DayClass
    {
        List<CuboidRec> _cuboidRecs = new();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            long rslt = ProcessCuboids2(50);

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            long rslt = ProcessCuboids2(int.MaxValue);

            Console.WriteLine("Part2: {0}", rslt);
        }

        private long ProcessCuboids2(int maxDimension)
        {
            List<Cuboid> cuboidsSrc = new();
            List<Cuboid> cuboidsDst = new();

            foreach (CuboidRec cuboidRec in _cuboidRecs)
            {
                if (RangeOk(cuboidRec, maxDimension))
                {
                    cuboidsSrc.Add(new Cuboid(cuboidRec));
                }
                else
                {
                    break;
                }
            }

            cuboidsDst.Add(cuboidsSrc[0]);

            for (int i = 1; i < cuboidsSrc.Count; i++)
            {
                for (int j = 0; j < cuboidsDst.Count; j++)
                {
                    if (!cuboidsDst[j].IsSplit && cuboidsSrc[i].Intersects(cuboidsDst[j]))
                    {
                        Cuboid intersection = cuboidsSrc[i].Intersection(cuboidsDst[j]);
                        List<Cuboid> subs = cuboidsDst[j].Split(intersection);
                        if (subs.Count > 0)
                        {
                            foreach (Cuboid sub in subs)
                            {
                                cuboidsDst.Add(sub);
                            }
                        }
                        else
                        {
                            if (intersection.AxesAreEqual(cuboidsDst[j]))
                            {
                                cuboidsDst[j].IsSplit = true; // causes it to be ignored.
                            }
                        }
                    }
                }
                if (!cuboidsSrc[i].IsSplit && cuboidsSrc[i].IsOn) // on
                {
                    cuboidsDst.Add(cuboidsSrc[i]);
                }

            }

            long rslt = cuboidsDst.Sum(c => (!c.IsSplit && c.IsOn) ? c.NCubes : 0);

            return rslt;
        }

        private bool RangeOk(CuboidRec cuboid, int maxDimension)
        {
            bool rangeOk = false;

            if (Math.Abs(cuboid.x1) <= maxDimension &&
                Math.Abs(cuboid.x2) <= maxDimension &&
                Math.Abs(cuboid.y1) <= maxDimension &&
                Math.Abs(cuboid.y2) <= maxDimension &&
                Math.Abs(cuboid.z1) <= maxDimension &&
                Math.Abs(cuboid.z2) <= maxDimension)
            {
                rangeOk = true;
            }

            return rangeOk;
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
                    // Process the string
                    //on x = -8..36, y = -13..38, z = -30..18
                    bool state = line.StartsWith("on");
                    MatchCollection matches = Regex.Matches(line, "-?[0-9]+");
                    int x1 = int.Parse(matches[0].Value);
                    int x2 = int.Parse(matches[1].Value);
                    int y1 = int.Parse(matches[2].Value);
                    int y2 = int.Parse(matches[3].Value);
                    int z1 = int.Parse(matches[4].Value);
                    int z2 = int.Parse(matches[5].Value);
                    _cuboidRecs.Add(new CuboidRec(x1, x2, y1, y2, z1, z2, state));
                }

                file.Close();
            }
        }

    }
}
