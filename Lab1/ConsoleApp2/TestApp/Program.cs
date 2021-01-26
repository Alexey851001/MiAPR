using System;
using System.Collections.Generic;
using KMeans;
namespace TestApp
{
    class Program
    {
        public static void WriteAreas(List<Area> areas)
        {
            int j = 0;
            foreach (var area in areas)
            {
                j++;
                Console.WriteLine($"Area {j}: {area.AreaPoints.Count} points. Kernel {area.Kernel.X},{area.Kernel.Y}");
            }
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter count of points");
            //int pointCount = Console.Read();
            //Console.WriteLine("Enter count of class");
            //int classCount = Console.Read();
            int pointCount = 20000;
            int classCount = 1;
            List<Area> areas;
            //requared initialization
            ClassesGenerator.Initialize(pointCount, classCount);

            //step-by-step generation
            int iterations = 0;
            Console.WriteLine("start step-by-step generation");
            while (!ClassesGenerator.EndIteration())
            {
                areas = ClassesGenerator.Iteration();
                iterations++;
                Console.WriteLine($"iteration {iterations}.");
                WriteAreas(areas);
                Console.WriteLine($"iterate {iterations} times");
            }

            //full generation
            Console.WriteLine("start full generation");
            areas = ClassesGenerator.Generate();
            WriteAreas(areas);            
        }
    }
}
