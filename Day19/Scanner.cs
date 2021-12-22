using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    internal class Scanner
    {
        public Scanner(int id)
        {
            ID = id;
            Beacons = new List<Vector>();
            RotatedBeacons = new List<List<Vector>>();
            DistanceVectors = new List<List<DistanceVector>>();
            Rotation = -1;
        }
        public int ID { get; private set; }
        public List<Vector> Beacons { get; set; }
        public List<List<Vector>> RotatedBeacons { get; set;}
        public List<List<DistanceVector>> DistanceVectors { get; set; }
        public int Rotation { get; set; }
        public Vector TranslationMap { get; set; }
        public bool RotationFound
        {
            get
            {
                return Rotation >= 0;
            }
        }
        public override string ToString()
        {
            return String.Format("ID: {0}, N Beacons: {1}, Rotation: {2}", ID, Beacons.Count, Rotation);
        }
    }
}
