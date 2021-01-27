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
        public double Distance(Point point1,Point point2) {

            return Math.Sqrt((point1.X - point2.X) ^ 2 + (point1.Y - point2.Y) ^ 2);
        }
        public void GetArea(Area currentArea) {
            currentArea.AreaPoints.Clear();
            foreach (var point in AllPoints)
            {
                bool inArea = true;
                foreach (var area in AllAreas)
                {
                    if (area == currentArea) {
                        continue;
                    }
                    if (Distance(currentArea.Kernel, point) > Distance(area.Kernel, point)) {
                        inArea = false;
                        break;
                    }

                }
                if (inArea)
                {
                    currentArea.AreaPoints.Add(point);
                }
            }
        }
        public double GetJ(Point newKernel,Area area) {
            double result = 0;
            foreach (var point in area.AreaPoints) {
                result += result + ((point.X - newKernel.X) ^ 2 + (point.Y - newKernel.Y) ^ 2);
            }
            return result;

        }
        public void Iteration() {
            foreach (var area in AllAreas) {
                Point min = area.Kernel;
                double minJ = GetJ(min, area);
                foreach (var point in area.AreaPoints) {
                    double tempJ = GetJ(point, area);
                    if (tempJ < minJ) {
                        minJ = tempJ;
                        min = point;
                    }
                }
                area.OldKernel = area.Kernel;
                area.Kernel = min;            
            }
        
        }
    }
    

    public class Point
    {
        public virtual bool Equals(Point point)
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

    public class Kmeans
    {
        
    }
}