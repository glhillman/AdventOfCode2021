using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    internal class DayClass
    {
        record Point(int x, int y);
        record Target (Point UpperLeft, Point LowerRight);
        Target _target = new Target(new Point(257, -57), new Point(286, -101));
        //Target _target = new Target(new Point(20, -5), new Point(30, -10)); // sample

        public DayClass()
        {
        }

        public void Part1And2()
        {

            long rslt = 0;
            int bestMaxY = int.MinValue;
            int count = 0;

            for (int x = 0; x <= _target.LowerRight.x; x++)
            {
                for (int y = 100; y >= _target.LowerRight.y; y--)
                {
                    bool targetHit;
                    int maxY;

                    (targetHit, maxY) = TryTrajectory(x, y, _target);
                    if (targetHit)
                    {
                        bestMaxY = Math.Max(bestMaxY, maxY);
                        count++;
                    }
                }
            }

            rslt = bestMaxY;

            Console.WriteLine("Part1: {0}", bestMaxY);
            Console.WriteLine("Part2: {0}", count);
        }

        private (bool, int) TryTrajectory(int xVelocity, int yVelocity, Target target)
        {
            int x = 0;
            int y = 0;
            int maxY = int.MinValue;
            bool targetHit = false;

            x += xVelocity;
            xVelocity = xVelocity == 0 ? 0 : xVelocity - 1;
            y += yVelocity;
            yVelocity--;
            maxY = Math.Max(maxY, y);

            while (x <= target.LowerRight.x && y >= target.LowerRight.y)
            {
                if (x >= target.UpperLeft.x && x <= target.LowerRight.x &&
                    y <= target.UpperLeft.y && y >= target.LowerRight.y)
                {
                    targetHit = true;
                    break;
                }

                x += xVelocity;
                xVelocity = xVelocity == 0 ? 0 : xVelocity - 1;
                y += yVelocity;
                yVelocity--;
                maxY = Math.Max(maxY, y);
            }

            return (targetHit, maxY);
        }
    }
}
