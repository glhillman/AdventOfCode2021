using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    internal class BitReader
    {
        BitArray _bitArray;
        int _index;

        public BitReader(string hexString)
        {
            _bitArray = new BitArray(hexString.Length * 4);
            int bitIndex = 0;
            for (int i = 0; i < hexString.Length; i++)
            {
                byte[] bytes = Convert.FromHexString("0" + hexString.Substring(i, 1));
                string bits = Convert.ToString(bytes[0], 2).PadLeft(4, '0');
                for (int j = 0; j < bits.Length; j++)
                {
                    _bitArray.Set(bitIndex++, bits[j] == '1' ? true : false);
                }
            }

            _index = 0;
        }

        public int ReadInt(int nBits)
        {
            int value = 0;

            for (int i = 0; i < nBits; i++)
            {
                value <<= 1;
                value |= _bitArray[_index++] ? 1 : 0;
            }

            return value;
        }

        public int Index
        {
            get { return _index; }
        }

        public void ResetIndex()
        {
            _index = 0;
        }
    }
}
