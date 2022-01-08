using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    internal class DayClass
    {
        Dictionary<string, int> _indices = new Dictionary<string, int>();
        long[] _vars = new long[4]; // w, x, y, z
        List<string> _commands = new List<string>();
        public DayClass()
        {
            _indices["w"] = 0;
            _indices["x"] = 1;
            _indices["y"] = 2;
            _indices["z"] = 3;
            LoadData();
        }

        public void Part1()
        {
            long biggest = CalculateNumber(_commands.ToArray(), true);

            // validate
            //NextInput = biggest;
            //long rslt = Run();
            
            Console.WriteLine("Part1: {0}", biggest);
        }

        public void Part2()
        {
            long smallest = CalculateNumber(_commands.ToArray(), false);

            // validate
            //NextInput = small;
            //rslt = Run();
            Console.WriteLine("Part1: {0}", smallest);
        }

        // unashamedly copied - I've read a number of explanations of the pairing in the input, but
        //   it does not click for me. I've finally given up and cribbed one of the solutions that
        //   seemed the most straight-forward. I'm anxious to talk to Jeff & Scott about this one.
        long CalculateNumber(string[] input, bool findBig = true)
        {
            Stack<(int sourceIndex, int offset)> inputStash = new();
            int[] finalDigits = new int[14];

            int targetIndex = 0;
            for (int block = 0; block < input.Length; block += 18)
            {
                int check = int.Parse(input[block + 5].Split(' ')[2]);
                int offset = int.Parse(input[block + 15].Split(' ')[2]);
                if (check > 0)
                {
                    inputStash.Push((targetIndex, offset));
                }
                else
                {
                    (int sourceIndex, int offset) rule = inputStash.Pop();
                    int totalOffset = rule.offset + check;
                    if (totalOffset > 0)
                    {
                        if (findBig)
                        {
                            finalDigits[rule.sourceIndex] = 9 - totalOffset;
                            finalDigits[targetIndex] = 9;
                        }
                        else
                        {
                            finalDigits[rule.sourceIndex] = 1;
                            finalDigits[targetIndex] = 1 + totalOffset;
                        }
                    }
                    else
                    {
                        if (findBig)
                        {
                            finalDigits[rule.sourceIndex] = 9;
                            finalDigits[targetIndex] = 9 + totalOffset;
                        }
                        else
                        {
                            finalDigits[rule.sourceIndex] = 1 - totalOffset;
                            finalDigits[targetIndex] = 1;
                        }
                    }

                }
                targetIndex++;
                //Console.WriteLine(string.Join("", finalDigits)); // see the code emerging
            }
            return long.Parse(string.Join("", finalDigits));
        }

        // OK, so the interpreter is mine. I wrote it before I realized that I couldn't brute-force the solution
        private long Run()
        {
            foreach (string cmd in _commands)
            {
                (string op, int op1Index, long op2) = ParseCommand(cmd);
                switch (op)
                {
                    case "inp":
                        _vars[op1Index] = NextInput;
                        Console.WriteLine();
                        break;
                    case "add":
                        _vars[op1Index] += op2;
                        break;
                    case "mul":
                        _vars[op1Index] *= op2;
                        break;
                    case "div":
                        _vars[op1Index] /= op2;
                        break;
                    case "mod":
                        _vars[op1Index] %= op2;
                        break;
                    case "eql":
                        _vars[op1Index] = _vars[op1Index] == op2 ? 1 : 0;
                        break;
                    default:
                        throw new ArgumentException("Unknown operator");
                }
                Console.WriteLine(cmd);
                Console.WriteLine("w: {0}, x: {1}, y: {2}, z: {3}", _vars[0], _vars[1], _vars[2], _vars[3]);
            }

            return _vars[3]; // z
        }
        
        private int _inputIndex = 0;
        private string _input;
        private long NextInput
        {
            get { return long.Parse(_input[_inputIndex++].ToString()); }
            set { _input = value.ToString(); _inputIndex = 0; }
        }

        private (string, int, long) ParseCommand(string s)
        {
            string[] split = s.Split(' ');
            long op2 = 0;
            if (split[0] != "inp")
            {
                if (long.TryParse(split[2], out op2) == false) // variable, not constant
                {
                    int opIndex = _indices[split[2]];
                    op2 = _vars[opIndex];
                }
            }
            return (split[0], _indices[split[1]], op2);
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
                    _commands.Add(line);
                }

                file.Close();
            }
        }

    }
}
