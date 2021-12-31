using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    internal class DeterministicDie
    {

        public DeterministicDie()
        {
            _next = 0;
            _rolls = 0;
        }

        private int _next;
        public int Next
        {
            get
            {
                _next++;
                if (_next > 100)
                {
                    _next = 1;
                }
                Rolls++;
                return _next;
            }
        }

        private int _rolls;
        public int Rolls
        {
            get { return _rolls; } 
            private set { _rolls = value; }
        }

        public int Next3
        {
            get
            {
                return Next + Next + Next;
            }
        }
    }

    internal class QuantumDie
    {
        int _nextIndex = 0;
        public QuantumDie()
        {
            RollValues = new List<int>();

            for (int x = 1; x <= 3; x++)
                for (int y = 1; y <= 3; y++)
                    for (int z = 1; z <= 3; z++)
                        RollValues.Add(x + y + z);
        }

        public List<int> RollValues { get; set; }

        public int Next // this is the sum of three rolls
        {
            get
            {
                if (_nextIndex >= RollValues.Count)
                {
                    _nextIndex = 0;
                }
                return RollValues[_nextIndex++];
            }
        }
    }
}
