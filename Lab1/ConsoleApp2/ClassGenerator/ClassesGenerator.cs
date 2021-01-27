using System;
using System.Collections.Generic;
using System.Linq;


namespace KMeans
{

    public struct Point
    {
        public bool Equals(Point point)
        {
            return X == point.X && Y == point.Y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
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
    public static class ClassesGenerator
    {
        private static List<Point> AllPoints;
        private static List<Area> AllAreas;
        public static void Initialize(int pointCount, int classCount) {
            Random random = new Random();
            AllPoints = new List<Point>();
            for (int i = 0; i < pointCount; i++)
            {
                int x = random.Next() % 1000;
                int y = random.Next() % 1000;
                AllPoints.Add(new Point(x, y));
            }
            AllAreas = new List<Area>();
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
        public static List<Area> Generate()
        {            
            while (!EndIteration())
            {
                Iteration();
            }
            return AllAreas;
        }
        public static List<Area> Iteration()
        {
            GetAreas();
            foreach (var area in AllAreas)
            {
                Point min = area.Kernel;
                double minJ = GetJ(min, area);
                foreach (var point in area.AreaPoints)
                {
                    double tempJ = GetJ(point, area);
                    if (tempJ < minJ)
                    {
                        minJ = tempJ;
                        min = point;
                    }
                }
                area.OldKernel = area.Kernel;
                area.Kernel = min;

            }
            return AllAreas;
        }
        public static bool EndIteration()
        {
            return AllAreas.All(area => area.OldKernel == area.Kernel);
        }
        private static double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow((point1.X - point2.X), 2) + Math.Pow((point1.Y - point2.Y), 2));
        }
        private static double GetJ(Point newKernel, Area area)
        {
            double result = 0;
            foreach (var point in area.AreaPoints)
            {
                result += Math.Pow((point.X - newKernel.X), 2) + Math.Pow((point.Y - newKernel.Y), 2);
            }
            return result;

        }
        private static void GetAreas()
        {
            foreach (var area in AllAreas) { 
                area.AreaPoints.Clear();
            }
            foreach (var point in AllPoints)
            {
                Area minArea = null;
                double minDistance = double.MaxValue;
                foreach (var area in AllAreas)
                {
                    double distance = Distance(area.Kernel, point);
                    if (distance - minDistance <= 0) {
                        minDistance = distance;
                        minArea = area;
                    }
                }
                minArea.AreaPoints.Add(point);
            }
        }
    }
}