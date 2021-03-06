﻿using System;
using System.Collections.Generic;
using System.Linq;
using Generator;


namespace KMeans
{

    public class Area:IArea
    {
        public Point OldKernel;
        
        public Area(Point kernel)
        {
            OldKernel = new Point(-1, -1);
            AreaPoints = new List<Point>();
            Kernel = kernel;
        }        
        public Area(Area area)
        {
            OldKernel = area.OldKernel;
            AreaPoints = new List<Point>();
            AreaPoints.AddRange(area.AreaPoints);
            Kernel = area.Kernel;
        }
        public Area(IArea area)
        {
            OldKernel = new Point(-1, -1);
            AreaPoints = new List<Point>();
            AreaPoints.AddRange(area.AreaPoints);
            Kernel = area.Kernel;
        }
    }
    public static class ClassesGenerator
    {
        private static List<Point> AllPoints;
        private static List<Area> AllAreas;
        public static List<Area> Initialize(List<IArea> areas, List<Point> points)
        {
            AllPoints = new List<Point>();
            AllAreas = new List<Area>();
            areas.ForEach(area => AllAreas.Add(new Area(area)));
            AllPoints = points;
            GetAreas();
            return AllAreas;
        }
        public static List<Area> Initialize(int pointCount, int classCount) {
            Random random = new Random();
            AllPoints = new List<Point>();
            for (int i = 0; i < pointCount; i++)
            {
                int x = random.Next() % 1000;
                int y = random.Next() % 1000;
                AllPoints.Add(new Point(x, y));
            }
            AllAreas = new List<Area>();
            AllAreas.Add(new Area(FindExtremePoint()));
            for (int i = 1; i < classCount; i++)
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
            GetAreas();
            return AllAreas;
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
            GetAreas();
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
        private static Point FindExtremePoint()
        {
            Point startPoint = new Point(0, 0);
            Point minKernel = AllPoints[0];
            double minDistance = double.MaxValue;
            foreach (var point in AllPoints)
            {
                double distance = Distance(startPoint, point);
                if (distance - minDistance <= 0)
                {
                    minDistance = distance;
                    minKernel = point;
                }
            }
            return minKernel;

        }
        public static  List<Point> GetPoints() {
            return AllPoints;
        }
    }
}