using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    internal class DayClass
    {
        List<string> _diags = new List<string>();  
        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {

            long rslt;
            int epsilon = 0;
            int gamma = 0;
            int pos = 0;
            int oneCount = 0;
            int diagSize = _diags[0].Length;
            while (pos < diagSize)
            {
                epsilon *= 2;
                gamma *= 2;

                foreach (string s in _diags)
                {
                    if (s[pos] == '1')
                    {
                        oneCount++;
                    }
                }
                if (oneCount > (_diags.Count / 2))
                {
                    gamma |= 1;
                }
                else
                {
                    epsilon |= 1;
                }
                pos++;
                oneCount = 0;
            }

            rslt = gamma * epsilon;

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;

            int pos = 0;
            int diagSize = _diags[0].Length;
            List<string> oxyRating;
            List<string> c02Rating;

            oxyRating = SplitList(_diags, pos++, '1');
            while (pos < diagSize && oxyRating.Count > 1)
            {
                oxyRating = SplitList(oxyRating, pos++, '1');
            }

            pos = 0;
            c02Rating = SplitList(_diags, pos++, '0');
            while (pos < diagSize && c02Rating.Count > 1)
            {
                c02Rating = SplitList(c02Rating, pos++, '0');
            }

            int oxy = Convert.ToInt32(oxyRating[0], 2);
            int c02 = Convert.ToInt32(c02Rating[0], 2);

            rslt = oxy * c02;

            Console.WriteLine("Part2: {0}", rslt);
        }

        private List<string> SplitList(List<string> inList, int pos, char tieBreaker)
        {
            List<string> ones = new List<string>();
            List<string> zeros = new List<string>();

            foreach (string s in inList)
            {
                if (s[pos] == '1')
                {
                    ones.Add(s);
                }
                else
                {
                    zeros.Add(s);
                }
            }

            if (tieBreaker == '1') // oxygen rating
            {
                if (ones.Count >= zeros.Count)
                {
                    return ones;
                }
                else
                {
                    return zeros;
                }
            }
            else
            {
                if (zeros.Count <= ones.Count)
                {
                    return zeros;
                }
                else
                {
                    return ones;
                }
            }
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
                    _diags.Add(line);
                }

                file.Close();
            }
        }

    }
}
