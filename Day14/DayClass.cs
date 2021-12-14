using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    internal class Rule
    {
        public Rule(string elements, string middle)
        {
            Elements = elements;
            Middle = middle;
        }

        public string Elements { get; private set; }
        public string Middle { get; private set; }
        public override string ToString()
        {
            return string.Format("Between {0} insert {1}", Elements, Middle);
        }
    }
    internal class DayClass
    {
        string _polymer;
        List<Rule> _rules = new List<Rule>();
        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            // brute force - left in place, although algorithm used in part 2 is more efficient
            long rslt = 0;
            string polymer = _polymer;

            for (int i = 0; i < 10; i++)
            {
                StringBuilder sb = new StringBuilder();
                int index = 0;
                while (index < polymer.Length-1)
                {
                    string couplet = polymer.Substring(index, 2);
                    Rule rule = _rules.FirstOrDefault(r => r.Elements == couplet);
                    if (rule != null)
                    {
                        sb.Append(couplet[0]);
                        sb.Append(rule.Middle);
                    }
                    index++;
                }
                sb.Append(polymer[polymer.Length - 1]);
                polymer = sb.ToString();
            }

            Dictionary<char, long> counts = new Dictionary<char, long>();
            foreach (char c in polymer)
            {
                if (counts.ContainsKey(c))
                {
                    counts[c]++;
                }
                else
                {
                    counts[c] = 1;
                }
            }

            long mostCommon = counts.Max(c => c.Value);
            long leastCommon = counts.Min(c => c.Value);

            rslt = mostCommon - leastCommon;

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;
            string polymer = _polymer;

            Dictionary<string, long> dict1 = new Dictionary<string, long>();
            Dictionary<string, long> dict2 = new Dictionary<string, long>();

            // seed the first dictionary
            int index = 0;
            while (index < polymer.Length - 1)
            {
                string couplet = polymer.Substring(index++, 2);
                Rule pair = _rules.FirstOrDefault(r => r.Elements == couplet);
                if (pair != null)
                {
                    string key = couplet[0] + pair.Middle + couplet[1];
                    if (dict1.ContainsKey(key))
                        dict1[key]++;
                    else
                        dict1[key] = 1;

                    if (index >= polymer.Length-1)
                    {
                        key = polymer.Substring(polymer.Length - 1, 1);
                        if (dict1.ContainsKey(key))
                            dict1[key]++;
                        else
                            dict1[key] = 1;
                    }
                }
            }

            bool dict1IsSrc = true;

            for (int i = 1; i < 40; i++) // count from 1 as first step performed above
            {
                if (dict1IsSrc)
                {
                    Expand(dict1, dict2);
                }
                else
                {
                    Expand(dict2, dict1);
                }
                dict1IsSrc = !dict1IsSrc;
            }

            // the keys in the dictionary consist of 3 letters. Count only the first two.
            Dictionary<char, long> counts = new Dictionary<char, long>();
            foreach (string key in dict2.Keys)
            {
                long count = dict2[key];
                char charKey;

                if (key.Length > 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        charKey = key[i];
                        if (counts.ContainsKey(charKey))
                        {
                            counts[charKey] += count;
                        }
                        else
                        {
                            counts[charKey] = count;
                        }
                    }
                }
                else
                {
                    charKey = key[0];
                    if (counts.ContainsKey(charKey))
                    {
                        counts[charKey] += count;
                    }
                    else
                    {
                        counts[charKey] = count;
                    }
                }
            }

            long mostCommon = counts.Max(c => c.Value);
            long leastCommon = counts.Min(c => c.Value);

            rslt = mostCommon - leastCommon;

            Console.WriteLine("Part2: {0}", rslt);
        }

        private void Expand(Dictionary<string, long> src, Dictionary<string, long> dst)
        {
            List<string> dictKeys = src.Keys.ToList();
            foreach (string srcKey in dictKeys)
            {
                long srcValue = src[srcKey];
                if (srcKey.Length == 3 && srcValue > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        string ruleKey = srcKey.Substring(i, 2);
                        Rule rule = _rules.FirstOrDefault(r => r.Elements == ruleKey);
                        string dictKey = rule.Elements[0] + rule.Middle + rule.Elements[1];
                        if (dst.ContainsKey(dictKey))
                        {
                            dst[dictKey] += srcValue;
                        }
                        else
                        {
                            dst[dictKey] = srcValue;
                        }
                    }
                    src[srcKey] = 0;
                }
                else
                {
                    // single character that drags along at the end
                    if (srcKey.Length == 1)
                    {
                        dst[srcKey] = srcValue;
                    }
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
                _polymer = file.ReadLine();
                file.ReadLine(); // throw away blank line
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split(" -> ");
                    _rules.Add(new Rule(parts[0], parts[1]));
                }

                file.Close();
            }
        }

    }
}
