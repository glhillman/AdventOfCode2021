using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    internal class GridBase
    {
        public GridBase(char id)
        {
            Id = id;
        }
        public char Id { get; set; }
        public virtual bool IsAmphipod { get { return false; } }

        public override string ToString()
        {
            return String.Format("ID: {0}", Id);
        }
    }

    internal class Amphipod : GridBase
    {
        public Amphipod(char id)
            : base(id)
        {
        }
        public override bool IsAmphipod { get { return true;} }
        public int TargetCol { get; protected set; }
        public int MoveCost { get; protected set; }
        public Position CurrentRowCol { get; set;}
        public static int HoleBottomRow { get; protected set;}
        
        private static GridBase[,]? _grid;
        public static GridBase[,]? Grid 
        { 
            get
            {
                return _grid;
            }
            set
            {
                _grid = value;
                HoleBottomRow = _grid.GetLength(0) - 2;
            }
        }

        private static bool[] _holes = {false, false, false, true, false, true, false, true, false, true, false, false, false};

        // See if this amphipod can move home right now.
        public LegalMove? FindHomeMove()
        {
            LegalMove? homeMove = null;

            int holeRow = AvailableHoleRow(); // returns the target row if we can go, 0 otherwise
            if (holeRow > 1)
            {
                homeMove = new LegalMove(new Position(holeRow, TargetCol), CalcSteps(CurrentRowCol, new Position(holeRow, TargetCol)));
            }
            return homeMove; // position & steps to go home, else null
        }

        public bool IsHome // if true, we are in our hole & don't need to move again
        {
            get
            {
                bool isHome = false;

                if (CurrentRowCol.col == TargetCol && CurrentRowCol.row >= 2)
                {
                    int row = CurrentRowCol.row + 1;
                    while (row <= HoleBottomRow && Grid[row, TargetCol].Id == Id)
                    {
                        row++;
                    }
                    isHome = row > HoleBottomRow;
                }

                return isHome;
            }
        }

        public int AvailableHoleRow()
        {
            // see if the hole where this amphipod hides is available & we have a clear path to it. Return row if available, 0 otherwise
            int holeRow = 0;
            if (IsHome == false)
            {
                int row = HoleBottomRow;
                while (Grid[row, TargetCol].Id == this.Id)
                {
                    row--;
                }
                if (row > 1 && Grid[row, TargetCol].Id == '.')
                {
                    holeRow = row;

                    // the hole is available. Now see if anything is in the way
                    row = CurrentRowCol.row;
                    int col = CurrentRowCol.col;

                    // in case we need to go up & out of another hole
                    while (row - 1 >= 1 && Grid[row - 1, col].Id == '.')
                    {
                        row--;
                    }

                    if (row == 1)
                    {
                        // we are now in the aisle. check for a clear path
                        if (TargetCol < CurrentRowCol.col)
                        {
                            while (col > TargetCol && Grid[1, col - 1].Id == '.')
                            {
                                col--;
                            }
                        }
                        else
                        {
                            while (col < TargetCol && Grid[1, col + 1].Id == '.')
                            {
                                col++;
                            }
                        }
                        if (col != TargetCol)
                        {
                            holeRow = 0; // couldn't get there
                        }
                    }
                    else
                    {
                        holeRow = 0;
                    }
                }
            }

            return holeRow;
        }


        public List<LegalMove> FindLegalMoves()
        {
            List<LegalMove> legalMoves = new();

            if (CurrentRowCol.row > 1 && IsHome == false)
            {
                int row = CurrentRowCol.row;
                int stepsToAisle = 0;
                int stepsHorizontal = 0;
                int col = CurrentRowCol.col;
                while (Grid[row - 1, col].Id == '.')
                {
                    row--;
                    stepsToAisle++;
                }
                if (row == 1)
                {
                    // we can get out!
                    // can we go left?
                    while (Grid[row, col - 1].Id == '.')
                    {
                        stepsHorizontal++;
                        col--;
                        if (_holes[col] == false)
                        {
                            legalMoves.Add(new LegalMove(new Position(row, col), stepsToAisle + stepsHorizontal));
                        }
                    }
                    // can we go right?
                    col = CurrentRowCol.col;
                    stepsHorizontal = 0;
                    while (Grid[row, col + 1].Id == '.')
                    {
                        stepsHorizontal++;
                        col++;
                        if (_holes[col] == false)
                        {
                            legalMoves.Add(new LegalMove(new Position(row, col), stepsToAisle + stepsHorizontal));
                        }
                    }
                }
            }

            return legalMoves;
        }

        public int CalcSteps(Position src , Position dst)
        {
            int steps = 0;

            steps += src.row - 1; // step up out of a hole if necessary
            steps += Math.Abs(src.col - dst.col);
            steps += dst.row - 1;

            return steps;
        }
        public int Move(LegalMove legalMove)
        {
            // move the '.' to the current position of this amphipod
            Grid[CurrentRowCol.row, CurrentRowCol.col] = Grid[legalMove.pos.row, legalMove.pos.col];
            // move this amphipod to the new position in the grid
            Grid[legalMove.pos.row, legalMove.pos.col] = this;
            // update this amphipod's current position in the grid
            this.CurrentRowCol = legalMove.pos;

            return legalMove.steps * MoveCost; // return the cost of the move
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, TargetCol: {1}, Pos: {2}", Id, TargetCol, CurrentRowCol.ToString());
        }
    }

    internal class Amber : Amphipod
    {
        public Amber(Position pos)
            : base ('A')
        {
            TargetCol = 3;
            MoveCost = 1;
            CurrentRowCol = pos;
        }
    }

    internal class Bronze : Amphipod
    {
        public Bronze(Position pos)
            : base('B')
        {
            TargetCol = 5;
            MoveCost = 10;
            CurrentRowCol= pos;
        }
    }
    internal class Copper : Amphipod
    {
        public Copper(Position pos)
            : base('C')
        {
            TargetCol = 7;
            MoveCost = 100;
            CurrentRowCol = pos;
        }
    }
    internal class Desert : Amphipod
    {
        public Desert(Position pos)
            : base('D')
        {
            TargetCol = 9;
            MoveCost = 1000;
            CurrentRowCol = pos;
        }
    }
}
