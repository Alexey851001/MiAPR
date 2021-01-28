using System;
using System.Collections.Generic;

namespace Generator
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public bool Equals(Point point)
        {
            return X == point.X && Y == point.Y;
        }
        public static bool operator ==(Point c1, Point c2)
        {
            return c1.Equals(c2);
        }
        public static bool operator !=(Point c1, Point c2)
        {
            return !c1.Equals(c2);
        }
    }
    public class IArea {
        public Point Kernel;
        public List<Point> AreaPoints;
    }
}
