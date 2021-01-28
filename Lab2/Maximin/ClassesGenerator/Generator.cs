using System;
using System.Collections.Generic;

namespace Maximin
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
    public class Area
    {
        public Point Kernel;
        public Point MaxKernel;
        public double maxDistance;
        public List<Point> AreaPoints;

        public Area(Point kernel)
        {
            AreaPoints = new List<Point>();
            Kernel = kernel;
            maxDistance = double.MinValue;
            MaxKernel = new Point(-1, -1);
        }
        public Area(Area area)
        {
            AreaPoints = area.AreaPoints;
            Kernel = area.Kernel;
            maxDistance = area.maxDistance;
            MaxKernel = area.MaxKernel;
        }
    }
    public static class ClassesGenerator
    {
        private static List<Point> AllPoints;
        private static List<Area> AllAreas;
        private static int ClassCount = 0;
        private static bool isEndIteration;
        
        public static List<Area> Initialize(int pointCount, int classCount)
        {
            ClassCount = classCount;
            Iterator = 2;
            isEndIteration = false;
            Random random = new Random();
            AllPoints = new List<Point>();
            for (int i = 0; i < pointCount; i++)
            {
                int x = random.Next() % 1000;
                int y = random.Next() % 1000;
                AllPoints.Add(new Point(x, y));
            }
            AllAreas = new List<Area>();
            AllAreas.Add(new Area(AllPoints[random.Next() % pointCount]));
            double maxDistance = double.MinValue;
            Point maxKernel = new Point(-1,-1);
            foreach (var point in AllPoints)
            {
               double distance = Distance(AllAreas[0].Kernel, point);
               if (distance - maxDistance >= 0)
               {
                   maxDistance = distance;
                   maxKernel = point;
               }
            }
            AllAreas.Add(new Area(maxKernel));
            GetAreas();
            return AllAreas;
        }
        public static List<Area> Generate() {
            while(!EndIteration()){
                Iteration();            
            }
            return AllAreas;
        }
        private static int Iterator;
        public static bool EndIteration() {
            return (Iterator >= ClassCount)||isEndIteration;
        }
        public static List<Area> Iteration() {
            double maxDistance;
            foreach (var area in AllAreas) {
                maxDistance = double.MinValue;
                Point maxKernel = new Point(-1, -1);
                foreach (var point in area.AreaPoints)
                {
                    double distance = Distance(area.Kernel, point);
                    if (distance - maxDistance >= 0)
                    {
                        maxDistance = distance;
                        maxKernel = point;
                    }
                }
                area.MaxKernel = maxKernel;
                area.maxDistance = maxDistance;
            }
            maxDistance = double.MinValue;
            Area maxArea = null;
            foreach (var area in AllAreas)
            {
                double tempDistance = area.maxDistance;
                if (tempDistance - maxDistance >= 0) {
                    maxArea = area;
                    maxDistance = tempDistance;
                }
            }
            Point Challenger = maxArea.MaxKernel;
            foreach (var firstArea in AllAreas) {
                foreach (var secondArea in AllAreas)
                {
                    if (firstArea == secondArea) continue;
                    double distance = Distance(firstArea.Kernel, secondArea.Kernel)/2;
                    if ((maxDistance - distance) <= 0)
                    {
                        isEndIteration = true;
                    }
                }
            }
            if (!isEndIteration)
            {
                AllAreas.Add(new Area(maxArea.MaxKernel));
            }
            GetAreas();
            Iterator++;
            return AllAreas;
        }
        private static double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow((point1.X - point2.X), 2) + Math.Pow((point1.Y - point2.Y), 2));
        }
        private static void GetAreas()
        {
            foreach (var area in AllAreas)
            {
                area.AreaPoints.Clear();
            }
            foreach (var point in AllPoints)
            {
                Area minArea = null;
                double minDistance = double.MaxValue;
                foreach (var area in AllAreas)
                {
                    double distance = Distance(area.Kernel, point);
                    if (distance - minDistance <= 0)
                    {
                        minDistance = distance;
                        minArea = area;
                    }
                }
                minArea.AreaPoints.Add(point);
            }
        }        
    }
}
