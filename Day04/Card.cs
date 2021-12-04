using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    internal class Card
    {
        List<int> _values;
        List<int> _backupValues;
        public Card(string rowValues)
        {
            IsActiveCard = true;

            _values = new List<int>();
            
            AddRowValues(rowValues);
        }

        public void AddRowValues(string rowValues)
        {
            string [] values = rowValues.Trim().Replace("  ", " ").Split(' ');
            for (int i = 0; i < values.Length; i++)
            {
                _values.Add(int.Parse(values[i]));
            }

            if (_values.Count == 25)
            {
                _backupValues = new List<int>(_values);
            }
        }

        public void ResetValues()
        {
            _values = new List<int>(_backupValues);
        }

        public bool IsBingo(int value)
        {
            bool isBingo = false;

            int offset = _values.IndexOf(value);
            if (offset >= 0)
            {
                _values[offset] = -1; // mark the position - don't need to preserve the value that was there
                if (IsRowAtOffsetBingo(offset) || IsColAtOffsetBingo(offset))
                {
                    isBingo = true;
                    IsActiveCard = false;
                }
            }

            return isBingo;
        }

        public bool IsActiveCard { get; set; }

        public int SumUnmarked()
        {
            int sum = 0;
            foreach (int value in _values)
            {
                if (value > 0)
                {
                    sum += value;
                }
            }

            return sum;
        }

        private bool IsRowAtOffsetBingo(int offset)
        {
            bool isBingo = true;
            int row = offset / 5;
            int startRow = row * 5;
            for (int i = startRow; i < startRow + 5; i++)
            {
                if (_values[i] != -1)
                {
                    isBingo = false;
                    break;
                }
            }
            return isBingo;
        }

        private bool IsColAtOffsetBingo(int offset)
        {
            bool isBingo = true;
            int col = offset % 5;
            for (int row = 0; row < 5; row++)
            {
                if (_values[row * 5 + col] != -1)
                {
                    isBingo = false;
                    break;
                }
            }
            return isBingo;
        }

        public void DumpCard()
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Console.Write(_values[row * 5 + col] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
