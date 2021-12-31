using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    internal class DayClass
    {
        // part2 structures
        record Players(int score1, int pos1, int score2, int pos2, bool player1);
        Dictionary<Players, (long, long)> _games = new(); // (long, long) is n wins for player1, n wins for player 2
        QuantumDie _quantumDie = new QuantumDie();

        List<int> _startPositions = new List<int>();

        public DayClass()
        {
            LoadData();
        }

        public void Part1()
        {
            DeterministicDie die = new DeterministicDie();
            Player player1 = new Player(1, _startPositions[0], 1000);
            Player player2 = new Player(2, _startPositions[1], 1000);

            while (player1.Move(die.Next3) == false && player2.Move(die.Next3) == false)
            { }

            long rslt = die.Rolls * (player1.IsWinner ? player2.CurrentScore : player1.CurrentScore);
            Console.WriteLine("Part1: {0}", rslt);
        }

        public void Part2()
        {
            Players players = new Players(0, _startPositions[0], 0, _startPositions[1], true);

            (long, long) results = RecurseGames(players);

            long rslt = Math.Max(results.Item1, results.Item2);

            Console.WriteLine("Part2: {0}", rslt);
        }

        private (long wins1, long wins2) RecurseGames(Players players)
        {
            
            if (players.score1 >= 21)
            {
                return (1, 0);
            }
            else if (players.score2 >= 21)
            {
                return (0, 1);
            }

            if (_games.ContainsKey(players) == false)
            {
                var (wins01, wins02) = (0L, 0L);
                for (int i = 0; i < 27; i++) // all possible quantum 3-roll results
                {
                    int steps = _quantumDie.Next;
                    if (players.player1)
                    {
                        int newPos = Move(players.pos1, steps);
                        var wins = RecurseGames(new Players(players.score1 + newPos, newPos, players.score2, players.pos2, !players.player1));
                        wins01 += wins.wins1;
                        wins02 += wins.wins2;
                    }
                    else
                    {
                        int newPos = Move(players.pos2, steps);
                        var wins = RecurseGames(new Players(players.score1, players.pos1, players.score2 + newPos, newPos, !players.player1));
                        wins01 += wins.wins1;
                        wins02 += wins.wins2;
                    }
                    _games[players] = (wins01, wins02);
                }
            }
            return _games[players];
        }

        private int Move(int curPos, int steps)
        {
            int newPosition = (curPos + steps) % 10;
            if (newPosition == 0)
            {
                newPosition = 10;
            }
            return newPosition;
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
                    string[] split = line.Split(' ');
                    _startPositions.Add(int.Parse(split[4]));
                }

                file.Close();
            }
        }
    }
}
