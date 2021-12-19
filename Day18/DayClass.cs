using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    internal class DayClass
    {
        List<string> _input = new List<string>();
        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            long rslt = 0;

            List<List<SFNode>> reduced = new List<List<SFNode>>();

            foreach (string s in _input)
            {
                List<SFNode> num = Parse(s);
                Reduce(num);
                reduced.Add(num);
            }

            List<SFNode> sum = reduced[0];
            for (int i = 1; i < reduced.Count; i++)
            {
                sum = AddNumbers(sum, reduced[i]);
                Reduce(sum);
            }

            int index = 0;
            rslt = Magnitude(sum, ref index);

            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            List<List<SFNode>> reduced = new List<List<SFNode>>();

            foreach (string s in _input)
            {
                List<SFNode> node = Parse(s);
                Reduce(node);
                reduced.Add(node);
            }
            
            long max = int.MinValue;
            foreach (List<SFNode> node1 in reduced)
            {
                foreach (List<SFNode> node2 in reduced)
                {
                    if (node1 != node2)
                    {
                        // I modify the nodes the during processing. Since I'm running the numbers over and over I have
                        //   to make clean copies of every list each time.
                        List<SFNode> op1 = CloneList(node1);
                        List<SFNode> op2 = CloneList(node2);
                        List<SFNode> sum = AddNumbers(op1, op2);
                        Reduce(sum);
                        int index = 0;
                        long thisMax = Magnitude(sum, ref index);
                        max = Math.Max(max, thisMax);
                    }
                }
            }

            Console.WriteLine("Part2: {0}", max);
        }

        private List<SFNode> CloneList(List<SFNode> nodes)
        {
            List<SFNode> clone = new List<SFNode>();
            foreach (SFNode node in nodes)
            {
                clone.Add(new SFNode(node));
            }

            return clone;
        }

        private void Reduce(List<SFNode> nodes)
        {
            while (Explode(nodes) || Split(nodes))
            {
                // loop will bail out when neither Explode nor Split do anything
            }
        }

        private bool Explode(List<SFNode> nodes)
        {
            bool exploded = false;
            int previousValueIndex = 0;

            for (int i = 0; i < nodes.Count && exploded == false; i++)
            {
                if (nodes[i].NodeType == NodeType.Value)
                {
                    if (nodes[i].Depth > 4 && nodes[i+1].NodeType == NodeType.Value) // explode this one
                    {
                        if (previousValueIndex > 0)
                        {
                            nodes[previousValueIndex].Value += nodes[i].Value; // add to previous value, if it exists
                        }
                        for (int j = i + 2; j < nodes.Count; j++)
                        {
                            if (nodes[j].NodeType == NodeType.Value)
                            {
                                nodes[j].Value += nodes[i + 1].Value;
                                break;
                            }
                        }
                        SFNode node = new SFNode(0, nodes[i].Depth - 1);
                        nodes.RemoveRange(i - 1, 4); // (xy)
                        nodes.Insert(i - 1, node);
                        exploded = true;
                    }
                    else
                    {
                        previousValueIndex = i; // track the most recent value node so we don't have to search backward
                    }
                }
            }

            return exploded;
        }

        private bool Split (List<SFNode> nodes)
        {
            bool split = false;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].NodeType == NodeType.Value && nodes[i].Value > 9)
                {
                    // needs to split
                    int x = nodes[i].Value / 2;
                    int y = nodes[i].Value % 2 == 1 ? x + 1 : x;  // odd numbers round up
                    int newDepth = nodes[i].Depth + 1;
                    SFNode nodex = new SFNode(x, nodes[i].Depth + 1);
                    SFNode nodey = new SFNode(y, nodes[i].Depth + 1);

                    // delete the value & replace it with the new nodes, surrounded by brackets
                    nodes.RemoveAt(i);
                    nodes.Insert(i, new SFNode(NodeType.LeftBracket));
                    nodes.Insert(i + 1, new SFNode(x, newDepth));
                    nodes.Insert(i + 2, new SFNode(y, newDepth));
                    nodes.Insert(i + 3, new SFNode(NodeType.RightBracket));
                    split = true;
                    break;
                }
            }

            return split;
        }

        private long Magnitude(List<SFNode> nodes, ref int index)
        {
            SFNode node = nodes[index++];
            if (node.NodeType != NodeType.Value) // brackets - recurse deeper
            {
                long x = Magnitude(nodes, ref index);
                long y = Magnitude(nodes, ref index);
                index++;
                return x * 3 + y * 2;
            }
            else
            {
                return node.Value;
            }
        }

        private List<SFNode> AddNumbers(List<SFNode> num1, List<SFNode> num2)
        {
            List<SFNode> sum = new List<SFNode>();
            sum.Add(new SFNode(NodeType.LeftBracket));
            foreach (SFNode node in num1)
            {
                if (node.NodeType == NodeType.Value)
                {
                    node.Depth++;
                }
                sum.Add(new SFNode(node));
            }
            foreach (SFNode node in num2)
            {
                if (node.NodeType == NodeType.Value)
                {
                    node.Depth++;
                }
                sum.Add(new SFNode(node));
            }
            sum.Add(new SFNode(NodeType.RightBracket));

            return sum;
        }

        private List<SFNode> Parse(string s)
        {
            List<SFNode> nodes = new List<SFNode>();

            int index = 0;
            int depth = 0;

            while (index < s.Length)
            {
                if (char.IsDigit(s[index]))
                {
                    nodes.Add(new SFNode(ParseNumber(s, ref index), depth));
                }
                else
                {
                    switch (s[index++])
                    {
                        case '[':
                            nodes.Add(new SFNode(NodeType.LeftBracket));
                            depth++;
                            break;
                        case ']':
                            nodes.Add(new SFNode(NodeType.RightBracket));
                            depth--;
                            break;
                        default:
                            break; // comma - don't need it
                    }
                }
            }

            return nodes;
        }
        private int ParseNumber(string s, ref int index)
        {
            StringBuilder sb = new StringBuilder();
            while (char.IsDigit(s[index]))
            {
                sb.Append(s[index++]);
            }
            return int.Parse(sb.ToString());
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
                    _input.Add(line);
                }

                file.Close();
            }
        }

    }
}
