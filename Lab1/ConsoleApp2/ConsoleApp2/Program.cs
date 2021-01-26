using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        public static List<Point> AllPoints= new List<Point>();
        public static List<Area> AllAreas = new List<Area>();
        
        static void Main(string[] args)
        {
            Random random = new Random();
            Console.WriteLine("Enter count of points");
            int pointCount = Console.Read();
            for (int i = 0; i < pointCount; i++)
            {
                int x = random.Next() % 1000;
                int y = random.Next() % 1000;
                AllPoints.Add(new Point(x, y));
            }
            Console.WriteLine("Enter count of class");
            int classCount = Console.Read();
            for (int i = 0; i < classCount; i++)
            {
                bool isKernel = false;
                Point kernel;
                do
                {
                    int index = random.Next() % pointCount;
                    kernel = AllPoints[index];
                    foreach (var area in AllAreas)
                    {
                        if (kernel == area.Kernel)
                        {
                            isKernel = true;
                        }
                    }
                } while (isKernel);

                AllAreas.Add(new Area(kernel));
            }
            
        }
    }

    public class Point
    {
        public virtual bool Equals(Point point)
        {
            return X == point.X && Y == point.Y;
        }

        private int X {get; set; }
        private int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Area
    {
        public Point Kernel;
        public Point OldKernel;
        public List<Point> AreaPoints;

        public Area(Point kernel)
        {
            OldKernel = new Point(-1, -1);
            AreaPoints = new List<Point>();
            Kernel = kernel;
        }
    }
}