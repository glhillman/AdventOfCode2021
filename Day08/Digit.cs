using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    internal class Digit
    {
        private string[] segments;
        Dictionary<string, int> keyValuePairs;
        public Digit()
        {
            segments = new string[8];
            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = String.Empty;
            }

            keyValuePairs = new Dictionary<string, int>();
        }

        public void SetSegment(int segNumber, string value)
        {
            segments[segNumber] = value;
        }

        public bool Contains(char c)
        {
            bool contains = false;

            for (int i = 1; contains == false && i < segments.Length; i++)
            {
                if (segments[i] != string.Empty && segments[i][0] == c)
                    contains = true;       
            }

            return contains;
        }

        public void SetRemaining(int segNumber)
        {
            for (char chr = 'a'; chr <= 'g'; chr++)
            {
                if (Contains(chr) == false)
                {
                    segments[segNumber] = chr.ToString();
                    break;
                }
            }
        }


        public void BuildMap()
        {
            StringBuilder sb = new StringBuilder();

            keyValuePairs[DigitString(new int[] { 1, 2, 3, 5, 6, 7 })] = 0;
            keyValuePairs[DigitString(new int[] { 3, 6 })] = 1;
            keyValuePairs[DigitString(new int[] { 1, 3, 4, 5, 7 })] = 2;
            keyValuePairs[DigitString(new int[] { 1, 3, 4, 6, 7 })] = 3;
            keyValuePairs[DigitString(new int[] { 2, 3, 4, 6, })] = 4;
            keyValuePairs[DigitString(new int[] { 1, 2, 4, 6, 7 })] = 5;
            keyValuePairs[DigitString(new int[] { 1, 2, 4, 5, 6, 7 })] = 6;
            keyValuePairs[DigitString(new int[] { 1, 3, 6})] = 7;
            keyValuePairs[DigitString(new int[] { 1, 2, 3, 4, 5, 6, 7 })] = 8;
            keyValuePairs[DigitString(new int[] { 1, 2, 3, 4, 6, 7 })] = 9;
        }

        public static string SortString(string input)
        {
            char[] characters = input.Trim().ToArray();
            Array.Sort(characters);
            return new string(characters);
        }

        public string DigitString(int[] segIndexes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (int index in segIndexes)
            {
                sb.Append(segments[index]);
            }

            return SortString(sb.ToString());
        }

        public int GetDisplayedNumber(string numCodes)
        {
            int value = 0;
            string[] split = numCodes.Split(' ');

            foreach (string s in split)
            {
                if (s.Length > 0)
                {
                    value *= 10;
                    value += keyValuePairs[SortString(s)];
                }
            }

            return value;
        }

        public string this[int index]
        {
            get { return segments[index]; }
            set { segments[index] = value; }
        }
    }
}
