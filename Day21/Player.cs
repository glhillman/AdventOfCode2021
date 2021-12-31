using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    internal class Player
    {
        public Player(int id, int start, long winningScore)
        {
            ID = id;
            Position = start;
            WinningScore = winningScore;
            CurrentScore = 0;
            NthMove = 0;
        }
        public int NthMove { get; set; }
        public int ID { get; private set; }
        public int Position { get; private set; }
        public bool Move(int steps)
        {
            Position = (Position + steps) % 10;
            if (Position == 0)
            {
                Position = 10;
            }
            CurrentScore += Position;
            return CurrentScore >= 1000 ? true : false;
        }

        public long WinningScore { get; private set;}   
        public long CurrentScore { get; private set; }
        public bool IsWinner { get { return CurrentScore >= WinningScore; } }
        public override string ToString()
        {
            return String.Format("ID: {0}, Score: {1}, Position: {2}", ID, CurrentScore, Position);
        }
    }
}
