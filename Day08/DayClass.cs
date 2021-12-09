using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    internal class DayClass
    {
        List<string> _rawData = new List<string>();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {

            long rslt = 0;
            foreach (string s in _rawData)
            {
                string[] parts = s.Split('|');
                string[] values = parts[1].Split(' ');
                
                foreach (string v in values)
                {
                    int length = v.Trim().Length;

                    if (length == 2 || length == 4 || length == 3 || length == 7)
                    {
                        rslt++;
                    }
                }
            }

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;
            foreach (string s in _rawData)
            {
                string[] parts = s.Split('|');
                Digit digit = RenderWiresAndSegments(parts[0]);
                digit.BuildMap();
                int num = digit.GetDisplayedNumber(parts[1]);
                rslt += num;
            }
            Console.WriteLine("Part2: {0}", rslt);
        }

        private Digit RenderWiresAndSegments(string allWires)
        {
            Digit digit = new Digit();

            List<string> wires = new List<string>();
            string[] parts = allWires.Split(' ');
            foreach (string w in parts)
            {
                if (w.Length > 0)
                {
                    wires.Add(w);
                }
            }

            // segments labeled as follows to avoid mapping confusion with letters in wires:
            /*
             * 11111
             * 2   3
             * 2   3
             * 44444
             * 5   6
             * 5   6
             * 77777
             */

            // isolate segment 1 by finding 2-char string & removing it from 3-char string
            string str36 = wires.Where(s => s.Length == 2).First();
            string str7 = wires.Where(s => s.Length == 3).First();
            foreach (char c in str36)
            {
                str7 = str7.Remove(str7.IndexOf(c), 1);
            }
            digit.SetSegment(1, str7);

            // isolate 4-char string & match all its chars to all the 5-char strings. All match = segment 4, single match = segment 2;
            string str4char = wires.Where(s => s.Length == 4).First();
            List<string> str5char = wires.Where(s => s.Length == 5).ToList();
            string all5 = string.Empty;
            foreach (string s in str5char)
            {
                all5 += s;
            }
            int nFound = 0;

            foreach (char c in str4char)
            {
                if (all5.Count(ch => ch == c) == 1)
                {
                    digit.SetSegment(2, c.ToString());
                    nFound++;
                }
                else if (all5.Count(ch => ch == c) == str5char.Count)
                {
                    digit.SetSegment(4, c.ToString());
                    nFound++;
                }
                if (nFound == 2)
                {
                    break;
                }
            }

            // in all the 5-char strings, the one that contains segment 2 has segment 6 (one of the 2-char strings), the other is segment 3
            foreach (string s in str5char)
            {
                if (s.Contains(digit[2]))
                {
                    // s also contains ONE of the chars in str36 - it is segment 6. The other is segment 3    
                    if (s.Contains(str36[0]))
                    {
                        digit[6] = str36[0].ToString();
                        digit[3] = str36[1].ToString();
                    }
                    else
                    {
                        digit[6] = str36[1].ToString();
                        digit[3] = str36[0].ToString();
                    }
                }
            }

            // in all the 5-char strings, the one that contains segments 1,3,4, & 6 is segment 7 on the remaining character
            foreach (string s in str5char)
            {
                if (s.Contains(digit[1]) && s.Contains(digit[3]) && s.Contains(digit[4]) && s.Contains(digit[6]))
                {
                    // s is the one we want
                    foreach (char c in s)
                    {
                        if (s.Contains(c) == false)
                        {
                            digit[7] = c.ToString();
                            digit.SetRemaining(5);
                        }
                    }
                }
            }

            // two segments remain undefined - 5 & 7
            // in all the 6-char strings, the one that contains 3 segments not already defined is segment 7
            List<string> str6char = wires.Where(s => s.Length == 6).ToList();
            for (char c = 'a'; c <= 'g'; c++)
            {
                int count = 0;

                foreach (string s in str6char)
                {
                    if (digit.Contains(c))
                    {
                        count = 0;
                        continue;
                    }
                    if (s.Contains(c))
                    {
                        count++;
                    }
                }
                if (count == str6char.Count)
                {
                    digit[7] = c.ToString();
                    digit.SetRemaining(5); // set the only remaining segment
                    break;
                }
            }

            return digit;
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
                    _rawData.Add(line);
                }

                file.Close();
            }
        }

    }
}
