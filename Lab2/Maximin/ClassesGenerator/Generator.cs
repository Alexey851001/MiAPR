using System;
using System.Linq;
using System.Collections.Generic;
using Generator;

namespace Maximin
{
    public class Area:IArea
    {
        public Point MaxKernel;
        public double maxDistance;
        
        public Area(Point kernel)
        {
            AreaPoints = new List<Point>();
            Kernel = kernel;
            maxDistance = double.MinValue;
            MaxKernel = new Point(-1, -1);
        }
        public Area(Area area)
        {
            AreaPoints = new List<Point>();
            AreaPoints.AddRange(area.AreaPoints);
            Kernel = area.Kernel;
            maxDistance = area.maxDistance;
            MaxKernel = area.MaxKernel;
        }
        public Area(IArea area)
        {
            AreaPoints = new List<Point>();
            AreaPoints.AddRange(area.AreaPoints);
            Kernel = area.Kernel;
            maxDistance = double.MinValue;
            MaxKernel = new Point(-1, -1);
        }
    }
    public static class ClassesGenerator
    {
        private static List<Point> AllPoints;
        private static List<Area> AllAreas;
        private static bool isEndIteration;
        
        public static List<Area> Initialize(int pointCount)
        {
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
        public static bool EndIteration() {
            return isEndIteration;
        }
        public static List<Area> Iteration()
        {
            isEndIteration = false;
            double maxDistance;
            foreach (var area in AllAreas)
            {
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
                if (tempDistance - maxDistance >= 0)
                {
                    maxArea = area;
                    maxDistance = tempDistance;
                }
            }
            if ((maxDistance - GetAverageDistance()/2) <= 0)
            {
                isEndIteration = true;
            }
            if (!isEndIteration)
            {
                AllAreas.Add(new Area(maxArea.MaxKernel));
            }
            GetAreas();
            return AllAreas;
        }
        public static List<Area> Iteration2() {
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
            return AllAreas;
        }
        public static List<Area> Iteration3()
        {
            isEndIteration = true;
            foreach (var _area in AllAreas)
            {
                //step4
                double maxDistance;
                foreach (var area in AllAreas)
                {
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
                //step5.1
                maxDistance = double.MinValue;
                Area maxArea = null;
                foreach (var area in AllAreas)
                {
                    double tempDistance = area.maxDistance;
                    if (tempDistance - maxDistance >= 0)
                    {
                        maxArea = area;
                        maxDistance = tempDistance;
                    }
                }
                Point Challenger = maxArea.MaxKernel;

                //step5.2
                if (AllAreas.All(firstArea =>
                {
                    return AllAreas.All(secondArea =>
                    {
                        if (firstArea == secondArea) return true;
                        double distance = Distance(firstArea.Kernel, secondArea.Kernel) / 2;
                        if ((maxDistance - distance) <= 0)
                        {
                            return false;
                        }
                        return true;
                    });
                }))
                {
                    isEndIteration = false;
                    AllAreas.Add(new Area(maxArea.MaxKernel));
                    break;
                }
                else
                {
                    maxArea.maxDistance = double.MinValue;
                    maxArea = null;
                }
            }
            GetAreas();
            return AllAreas;
        }
        private static double GetAverageDistance()
        {
            double count = 0;
            double sum = 0;
            for (var i = 0; i < AllAreas.Count - 1; i++)
            {
                for (var j = i + 1; j < AllAreas.Count; j++)
                {
                    sum += Distance(AllAreas[i].Kernel, AllAreas[j].Kernel);
                    count++;
                }
            }
            return (sum / count);
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
        public static List<Point> GetPoints()
        {
            return AllPoints;
        }
    }
}
