using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    internal class DayClass
    {
        List<string> _navLines = new List<string>();
        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            int errorScore = 0;

            foreach (string line in _navLines)
            {
                char illegalChar = ' ';
                ParseNavLine(line, ref illegalChar);
                switch (illegalChar)
                {
                    case ')': errorScore += 3; break;
                    case ']': errorScore += 57; break;
                    case '}': errorScore += 1197; break;
                    case '>': errorScore += 25137; break;
                }
            }

            Console.WriteLine("Part1: {0}", errorScore);
        }

        public void Part2()
        {
            Stack<char> tokens;
            List<long> scores = new List<long>();

            foreach (string line in _navLines)
            {
                char dummy = ' ';
                tokens = ParseNavLine(line, ref dummy);
                if (tokens.Count > 0)
                {
                    long score = 0;
                    while (tokens.Count > 0)
                    {
                        score *= 5;
                        switch (tokens.Pop())
                        {
                            case '(': score += 1; break;
                            case '[': score += 2; break;
                            case '{': score += 3; break;
                            case '<': score += 4; break;
                        }
                    }
                    scores.Add(score);
                }
            }
            
            scores.Sort();
            long middleScore = scores[scores.Count / 2];

            Console.WriteLine("Part2: {0}", middleScore);
        }

        private Stack<char> ParseNavLine(string line, ref char illegalChar)
        {
            Stack<char> tokens = new Stack<char>();
            illegalChar = ' ';

            foreach (char c in line)
            {
                bool pushed;

                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        tokens.Push(c);
                        pushed = true;
                        break;
                    default:
                        pushed = false;
                        break;
                }

                if (pushed == false)
                {
                    if (tokens.Count > 0)
                    {
                        switch (c)
                        {
                            case ')':
                                if (tokens.Peek() == '(')
                                    tokens.Pop();
                                else
                                    illegalChar = c;
                                break;
                            case ']':
                                if (tokens.Peek() == '[')
                                    tokens.Pop();
                                else
                                    illegalChar = c;
                                break;
                            case '}':
                                if (tokens.Peek() == '{')
                                    tokens.Pop();
                                else
                                    illegalChar = c;
                                break;
                            case '>':
                                if (tokens.Peek() == '<')
                                    tokens.Pop();
                                else
                                    illegalChar = c;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (illegalChar != ' ')
                {
                    tokens.Clear();
                }
            }

            return tokens;
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
                    _navLines.Add(line);
                }

                file.Close();
            }
        }
    }
}
