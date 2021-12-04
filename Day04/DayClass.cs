using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    internal class DayClass
    {
        List<int> _input = new List<int>();
        List<Card> _cards = new List<Card>();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            long rslt = 0;

            foreach (int value in _input)
            {
                foreach (Card card in _cards)
                {
                    if (card.IsBingo(value))
                    {
                        int unmarkedSum = card.SumUnmarked();
                        rslt = unmarkedSum * value;
                        break;
                    }
                }
                if (rslt != 0)
                {
                    break;
                }
            }
            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {

            long rslt = 0;
            int unmarkedSum = 0;
            int winningValue = 0;

            foreach (Card card in _cards)
            {
                card.ResetValues();
            }

            foreach (int value in _input)
            {
                foreach (Card card in _cards)
                {
                    if (card.IsActiveCard && card.IsBingo(value))
                    {
                        unmarkedSum = card.SumUnmarked();
                        winningValue = value;
                    }
                }
            }
            rslt = unmarkedSum * winningValue;
            Console.WriteLine("Part1: {0}", rslt);
        }

        private void LoadData()
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                string? line;
                bool processing = true;
                StreamReader file = new StreamReader(inputFile);
                line = file.ReadLine();
                string[] values = line.Split(',');
                foreach (string value in values)
                {
                    _input.Add(int.Parse(value));
                }
                file.ReadLine(); // throw away next blank line
                line = file.ReadLine();
                Card card = new Card(line);
                while (processing)
                { 
                    line = file.ReadLine();
                    if (line == null)
                    {
                        _cards.Add(card);
                        processing = false;
                    }
                    else if (line.Length == 0)
                    {
                        _cards.Add(card);
                        line = file.ReadLine();
                        card = new Card(line);
                    }
                    else 
                    {
                        card.AddRowValues(line);
                    }
                }

                file.Close();
            }
        }

    }
}
