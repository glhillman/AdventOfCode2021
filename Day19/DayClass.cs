using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    record Vector(int x, int y, int z);
    record DistanceVector(Vector calculated, Vector beacon);

    internal class DayClass
    {
        //string allData;
        public DayClass()
        {
            AllScanners = new List<Scanner>();
            LoadData();
        }

        public void Part1()
        {
            long rslt = 0;

            // set up the base scanner (scanner 0)
            // it has only one Rotated Beacons List - same as it's Beacons list
            // it has one Calculated Vector
            Scanner baseScanner = AllScanners[0];
            baseScanner.RotatedBeacons.Add(baseScanner.Beacons);
            baseScanner.DistanceVectors.Add(CalculateDistance(baseScanner.Beacons));
            baseScanner.Rotation = 0; // first rotation results in no rotation
            baseScanner.TranslationMap = new Vector(0, 0, 0);

            
            // calculate all rotation vectors & their distance calculations for scanners 1..n
            for (int i = 1; i < AllScanners.Count; i++)
            {
                Scanner scanner = AllScanners[i];
                for (int rotation = 0; rotation < 24; rotation++)
                {
                    scanner.RotatedBeacons.Add(RotateVector(scanner.Beacons, rotation));
                    scanner.DistanceVectors.Add(CalculateDistance(RotateVector(scanner.Beacons, rotation)));
                }
            }


            bool allRotated = false;
            baseScanner = AllScanners[0];
            int allScannerIndex = 0;
            do
            {
                // now find matching distance vectors from other scanners
                for (int i = 1; i < AllScanners.Count; i++)
                {
                    Scanner scanner = AllScanners[i];
                    if (scanner.RotationFound == false)
                    {
                        for (int rotation = 0; rotation < 24; rotation++)
                        {
                            IEnumerable<DistanceVector> intersect = baseScanner.DistanceVectors[0].Intersect(scanner.DistanceVectors[rotation], new DistanceVectorComparer());
                            if (intersect.Count() >= 12)
                            {
                                scanner.Rotation = rotation; // matching rotation found!
                                DistanceVector firstIntersect = intersect.First();
                                DistanceVector aVector = baseScanner.DistanceVectors[0].First(dv => dv.calculated == firstIntersect.calculated);
                                DistanceVector bVector = scanner.DistanceVectors[rotation].First(dv => dv.calculated == firstIntersect.calculated);
                                Vector translationMap = SubtractVector(aVector.beacon, bVector.beacon);
                                // eliminate the other rotations & calculated vectors just to clean up future references 
                                if (rotation + 1 < scanner.DistanceVectors.Count)
                                {
                                    scanner.DistanceVectors.RemoveRange(rotation + 1, scanner.DistanceVectors.Count - (rotation + 1));
                                    scanner.RotatedBeacons.RemoveRange(rotation + 1, scanner.RotatedBeacons.Count - (rotation + 1));
                                }
                                if (rotation > 0)
                                {
                                    scanner.DistanceVectors.RemoveRange(0, rotation);
                                    scanner.RotatedBeacons.RemoveRange(0, rotation);
                                }
                                for (int j = 0; j < scanner.RotatedBeacons[0].Count; j++)
                                {
                                    scanner.RotatedBeacons[0][j] = AddVector(translationMap, scanner.RotatedBeacons[0][j]);
                                }
                                scanner.TranslationMap = translationMap;

                                // recalculate the DistanceVectors
                                scanner.DistanceVectors[0] = CalculateDistance(scanner.RotatedBeacons[0]);

                                break;
                            }
                        }
                    }
                }

                allRotated = AllScanners.Count(s => s.RotationFound) == AllScanners.Count;
                if (allRotated == false)
                {
                    // find next scanner that has rotation established
                    allScannerIndex++;
                    if (allScannerIndex >= AllScanners.Count)
                    {
                        allScannerIndex = 0;
                    }
                    while (AllScanners[++allScannerIndex].RotationFound == false)
                    {
                    }
                    baseScanner = AllScanners[allScannerIndex];
                }
            }
            while (!allRotated);


            // find the unique beacons
            HashSet<Vector> hash = new HashSet<Vector>();
            foreach (Scanner scanner in AllScanners)
            {
                foreach (Vector v in scanner.RotatedBeacons[0])
                {
                    hash.Add(v);
                }
            }

            rslt = hash.Count;

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            int maxDistance = int.MinValue;

            foreach (Scanner scanner1 in AllScanners)
            {
                foreach (Scanner scanner2 in AllScanners)
                {
                    if (scanner1 != scanner2)
                    {
                        // calc manhattan distance
                        Vector diff = SubtractVector(scanner1.TranslationMap, scanner2.TranslationMap);
                        int distance = Math.Abs(diff.x) + Math.Abs(diff.y) + Math.Abs(diff.z);
                        maxDistance = Math.Max(distance, maxDistance);
                    }
                }
            }

            Console.WriteLine("Part2: {0}", maxDistance);
        }

        private Vector SubtractVector(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        private Vector AddVector(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        private List<Vector> RotateVector(List<Vector> beacons, int rotation)
        {
            List<Vector> rotated = new List<Vector>();
            foreach (Vector beacon in beacons)
            {
                rotated.Add(Rotate(beacon, rotation)); ;
            }
            return rotated;
        }

        List<DistanceVector> CalculateDistance(List<Vector> Beacons)
        {
            List<DistanceVector> vectors = new List<DistanceVector>();

            foreach (Vector b1 in Beacons)
            {
                foreach (Vector b2 in Beacons)
                {
                    if (b1 != b2)
                    {
                        DistanceVector dv = new DistanceVector(SubtractVector(b1, b2), b1);
                        vectors.Add(dv);
                    }
                }
            }

            return vectors;
        }

#pragma warning disable 1717
        private Vector Rotate(Vector point, int rotation)
        {
            var (x, y, z) = (point.x, point.y, point.z);

            switch (rotation)
            {
                case 0:  (x, y, z) = (x, y, z);    break;
                case 1:  (x, y, z) = (-x, y, -z);  break;
                case 2:  (x, y, z) = (y, -x, z);   break;
                case 3:  (x, y, z) = (-y, x, z);   break;
                case 4:  (x, y, z) = (z, y, -x);   break;
                case 5:  (x, y, z) = (-z, y, x);   break;
                case 6:  (x, y, z) = (x, -z, y);   break;
                case 7:  (x, y, z) = (-x, z, y);   break;
                case 8:  (x, y, z) = (y, -z, -x);  break;
                case 9:  (x, y, z) = (-y, -z, x);  break;
                case 10: (x, y, z) = (z, x, y);    break;
                case 11: (x, y, z) = (-z, -x, y);  break;
                case 12: (x, y, z) = (x, -y, -z);  break;
                case 13: (x, y, z) = (-x, -y, z);  break;
                case 14: (x, y, z) = (y, x, -z);   break;
                case 15: (x, y, z) = (-y, -x, -z); break;
                case 16: (x, y, z) = (z, -y, x);   break;
                case 17: (x, y, z) = (-z, -y, -x); break;
                case 18: (x, y, z) = (x, z, -y);   break;
                case 19: (x, y, z) = (-x, -z, -y); break;
                case 20: (x, y, z) = (y, z, x);    break;
                case 21: (x, y, z) = (-y, z, -x);  break;
                case 22: (x, y, z) = (z, -x, -y);  break;
                case 23: (x, y, z) = (-z, x, -y);  break;
                default:
                    break;
            }
#pragma warning restore
            return new Vector(x, y, z);
        }

        private List<Scanner> AllScanners { get; set; }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string? line;
                StreamReader file = new StreamReader(inputFile);
                while ((line = file.ReadLine()) != null)
                {
                    string[] split = line.Split(' ');
                    Scanner scanner = new Scanner(int.Parse(split[2]));
                    //file.ReadLine(); // throwaway blank
                    while ((line = file.ReadLine()) != null && line.Length > 0)
                    {
                        split = line.Split(',');
                        scanner.Beacons.Add(new Vector(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
                    }
                    AllScanners.Add(scanner);
                }

                file.Close();
            }
        }

    }

    // Custom comparer for the DistanceVector record
    class DistanceVectorComparer : IEqualityComparer<DistanceVector>
    {
        public bool Equals(DistanceVector x, DistanceVector y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) 
                return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.calculated == y.calculated;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(DistanceVector distanceVector)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(distanceVector, null)) 
                return 0;

            //Get hash code for the calculated field
            return distanceVector.calculated.GetHashCode();
        }
    }
}
