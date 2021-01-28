using System;
using System.Collections.Generic;
using KMeans;
using Maximin;
using Generator;
namespace TestApp
{
    class Program
    {
        public static void WriteAreas(List<IArea> areas)
        {
            int j = 0;
            foreach (var area in areas)
            {
                j++;
                Console.WriteLine($"Area {j}: {area.AreaPoints.Count} points. Kernel {area.Kernel.X},{area.Kernel.Y}");
            }
        }
        public static List<IArea> ConvertToIArea(List<Maximin.Area> areas) {
            List<IArea> result = new List<IArea>();
            areas.ForEach(area => result.Add(area));
            return result;
        } 
        public static List<IArea> ConvertToIArea(List<KMeans.Area> areas) {
            List<IArea> result = new List<IArea>();
            areas.ForEach(area => result.Add(area));
            return result;
        } 
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter count of points");
            //int pointCount = Console.Read();
            //Console.WriteLine("Enter count of class");
            //int classCount = Console.Read();

            int pointCount = 20000;
            int classCount = 8;
            List<IArea> areas;               
            //requared initialization
            Console.WriteLine("Initialize");
            areas = ConvertToIArea(Maximin.ClassesGenerator.Initialize(pointCount));
            WriteAreas(areas);
            //step-by-step generation
            int iterations = 0;
            Console.WriteLine("start step-by-step generation"); 
            while (!Maximin.ClassesGenerator.EndIteration())
            {
                areas = ConvertToIArea(Maximin.ClassesGenerator.Iteration2());
               
                iterations++;
                Console.WriteLine($"iteration {iterations}.");
                WriteAreas(areas);
            }
            Console.WriteLine($"iterate {iterations} times\n\n\n");
            //List<KMeans.Area> Kareas;
            //requared initialization
            Console.WriteLine("Initialize");
            areas = ConvertToIArea(KMeans.ClassesGenerator.Initialize(areas,Maximin.ClassesGenerator.GetPoints()));
            WriteAreas(areas);
            //step-by-step generation
            Console.WriteLine("start step-by-step generation");
            while (!KMeans.ClassesGenerator.EndIteration())
            {
                areas = ConvertToIArea(KMeans.ClassesGenerator.Iteration());
                iterations++;
                Console.WriteLine($"iteration {iterations}.");
                WriteAreas(areas);
            }
            Console.WriteLine($"iterate {iterations} times\n\n\n");
            //full generation
            //Console.WriteLine("start full generation");
            //areas = ClassesGenerator.Generate();
            //WriteAreas(areas);            
        }

    }
}
