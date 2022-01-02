using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    internal record Axis
    {
        public Axis(long start, long end)
        {
            Start = start;
            End = end;
        }

        public Axis(Axis other)
        {
            Start = other.Start;
            End = other.End;
        }

        public bool Intersects(Axis other)
        {
            long start = Math.Max(this.Start, other.Start);
            long end = Math.Min(this.End, other.End);

            return start <= end;
        }

        public Axis Intersection(Axis other)
        {
            return new Axis(Math.Max(this.Start, other.Start), Math.Min(this.End, other.End));
        }

        public long Length
        {
            get
            {
                return this.End >= this.Start ? this.End - this.Start + 1 : 0;
            }
        }

        public long Start { get; set; }
        public long End { get; set; }

        public override string ToString()
        {
            return String.Format("{0}..{1}", Start, End);
        }
    }

    internal record Cuboid
    {
        static int _nextID = 1;

        public Cuboid(Axis xaxis, Axis yaxis, Axis zaxis, bool isOn )
        {
            ID = _nextID++; // ID is only to help with debugging
            XAxis = xaxis;
            YAxis = yaxis;
            ZAxis = zaxis;
            IsOn = isOn;
            IsSplit = false;
        }
        public Cuboid(long x1, long x2, long y1, long y2, long z1, long z2, bool isOn)
            :this(new Axis(x1, x2), new Axis(y1, y2), new Axis(z1, z2), isOn)
        { }

        public Cuboid(CuboidRec cuboidRec)
            :this(cuboidRec.x1, cuboidRec.x2, cuboidRec.y1, cuboidRec.y2, cuboidRec.z1, cuboidRec.z2, cuboidRec.isOn)
        { }

        public List<Cuboid> Split(Cuboid intersect)
        {
            // create a list of Cuboids that are carved off this to make room for intersect
            List<Cuboid> cuboids = new();
            // XMin
            if (XAxis.Start < intersect.XAxis.Start)
            {
                cuboids.Add(new Cuboid(new Axis(XAxis.Start, intersect.XAxis.Start - 1), new Axis(YAxis), new Axis(ZAxis), true));
                XAxis.Start = intersect.XAxis.Start;
            }
            // XMax
            if (XAxis.End > intersect.XAxis.End)
            {
                cuboids.Add(new Cuboid(new Axis(intersect.XAxis.End + 1, XAxis.End), new Axis(YAxis), new Axis(ZAxis), true));
                XAxis.End = intersect.XAxis.End;
            }
            // YMin
            if (YAxis.Start < intersect.YAxis.Start)
            {
                cuboids.Add(new Cuboid(new Axis(XAxis), new Axis(YAxis.Start, intersect.YAxis.Start - 1), new Axis(ZAxis), true));
                YAxis.Start = intersect.YAxis.Start;
            }
            // YMax
            if (YAxis.End > intersect.YAxis.End)
            {
                cuboids.Add(new Cuboid(new Axis(XAxis), new Axis(intersect.YAxis.End + 1, YAxis.End), new Axis(ZAxis), true));
                YAxis.End = intersect.YAxis.End;
            }
            // ZMin
            if (ZAxis.Start < intersect.ZAxis.Start)
            {
                cuboids.Add(new Cuboid(new Axis(XAxis), new Axis(YAxis), new Axis(ZAxis.Start, intersect.ZAxis.Start - 1), true));
                ZAxis.Start = intersect.ZAxis.Start;
            }
            // ZMax
            if (ZAxis.End > intersect.ZAxis.End)
            {
                cuboids.Add(new Cuboid(new Axis(XAxis), new Axis(YAxis), new Axis(intersect.ZAxis.End + 1, ZAxis.End), true));
                ZAxis.End = intersect.ZAxis.End;
            }

            IsSplit = cuboids.Count > 0;

            return cuboids;
        }

        public int ID { get; private set; }
        public Axis XAxis { get; set; }
        public Axis YAxis { get; set; }
        public Axis ZAxis { get; set; }
        public bool IsOn { get; set; }

        public bool AxesAreEqual(Cuboid other)
        {
            return XAxis.Equals(other.XAxis) && YAxis.Equals(other.YAxis) && ZAxis.Equals(other.ZAxis);
        }

        public bool Intersects(Cuboid other)
        {
            return this.XAxis.Intersects(other.XAxis) &&
                   this.YAxis.Intersects(other.YAxis) &&
                   this.ZAxis.Intersects(other.ZAxis);
        }

        public Cuboid Intersection(Cuboid other)
        {
            return new Cuboid(XAxis.Intersection(other.XAxis), YAxis.Intersection(other.YAxis), ZAxis.Intersection(other.ZAxis), IsOn);
        }
        
        public long NCubes
        {
            get
            {
                return XAxis.Length * YAxis.Length * ZAxis.Length;
            }
        }

        public bool IsSplit { get; set; }

        public override string ToString()
        {
            return String.Format("ID: {0}, XAxis: {1}, YAxis: {2}, ZAxis: {3}, NCubes: {4}, IsSplit: {5}, IsOn: {6}", 
                                  ID, XAxis, YAxis, ZAxis, NCubes, IsSplit.ToString(), IsOn.ToString());
        }
    }
}
