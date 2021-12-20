using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    internal class DayClass
    {
        char[] _enhancement;
        char[,] _image;

        public DayClass()
        {
        }

        public void Part1()
        {
            Console.WriteLine("Part1: {0}", Enhance(2, 5));
        }

        public void Part2()
        {
            Console.WriteLine("Part2: {0}", Enhance(50, 55));
        }

        private int Enhance(int steps, int padBy)
        {
            LoadData(padBy);

            int rslt = 0;
            char[,] image1 = new char[_image.GetLength(0), _image.GetLength(1)];
            char[,] image2 = new char[_image.GetLength(0), _image.GetLength(1)];

            Array.Copy(_image, image1, _image.Length);
            InitImage(image2, '.');

            bool srcIsImage1 = true;

            int minRow = padBy;
            int minCol = padBy;
            int maxRow = image1.GetLength(0) - padBy - 1;
            int maxCol = image1.GetLength(1) - padBy - 1;
            char infiniteChar = '.';

            for (int i = 0; i < steps; i++)
            {
                if (srcIsImage1)
                {
                    Process(image1, image2, minRow, minCol, maxRow, maxCol, infiniteChar);
                }
                else
                {
                    Process(image2, image1, minRow, minCol, maxRow, maxCol, infiniteChar);
                }
                minRow--;
                minCol--;
                maxRow++;
                maxCol++;
                srcIsImage1 = !srcIsImage1;
                infiniteChar = infiniteChar == '.' ? '#' : '.';
            }

            rslt = srcIsImage1 ? CountLit(image1) : CountLit(image2);

            return rslt;
        }

        private int CountLit(char[,] image)
        {
            int count = 0;

            for (int row = 0; row < image.GetLength(0); row++)
            {
                for (int col = 0; col < image.GetLength(1); col++)
                {
                    count += image[row, col] == '#' ? 1 : 0;
                }
            }

            return count;
        }

        private void Process(char[,] src, char[,] dst, int row1, int col1, int row2, int col2, char infiniteChar)
        {
            for (int row = row1-1; row <= row2+1; row++)
            {
                for (int col = col1-1; col <= col2+1; col++)
                {
                    int index = GetIndexFromGrid(src, row, col, row1, col1, row2, col2, infiniteChar);
                    dst[row, col] = _enhancement[index];
                }
            }
            InitImage(src, '.');
        }

        private int GetIndexFromGrid(char[,] image, int middleRow, int middleCol, int minRow, int minCol, int maxRow, int maxCol, char infiniteChar)
        {
            int index = 0;
            for (int row = middleRow - 1; row < middleRow + 2; row++)
            {
                for (int col = middleCol - 1; col < middleCol+ 2; col++)
                {
                    char c = (row < minRow || row > maxRow || col < minCol || col > maxCol) ? infiniteChar : image[row, col];
                    index <<= 1;
                    index += c == '#' ? 1 : 0;
                }
            }
            return index;
        }
        private void InitImage(char[,] image, char value)
        {
            for (int row = 0; row < image.GetLength(0); row++)
            {
                for (int col = 0; col < image.GetLength(1); col++)
                {
                    image[row, col] = value;
                }
            }
        }

        private void DumpImage(char[,] image)
        {
            for (int row = 0; row < image.GetLength(0); row++)
            {
                for (int col = 0; col < image.GetLength(1); col++)
                {
                    Console.Write(image[row, col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void LoadData(int padBy)
        {
            string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

            if (File.Exists(inputFile))
            {
                StreamReader file = new StreamReader(inputFile);
                string[] lines = File.ReadAllLines(inputFile);
                // first line is enhancement map
                _enhancement = new char[lines[0].Length];
                Array.Copy(lines[0].ToCharArray(), _enhancement, _enhancement.Length);
                int length = lines[2].Length;
                _image = new char[lines.Length - 2 + padBy * 2, length + padBy * 2];
                InitImage(_image, '.');
                
                for (int i = 2; i < lines.Count(); i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        _image[i - 2 + padBy, j + padBy] = lines[i][j];
                    }
                }

                file.Close();
            }
        }

    }
}
