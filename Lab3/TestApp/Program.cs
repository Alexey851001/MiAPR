using System;
using GaussianDistribution;

namespace TestApp
{
    class Program
    {
        public static void WriteResult(FaultResult faultResult) {
            Console.WriteLine($"falseAlarmError:{faultResult.falseAlarmError},\nmissingDetectingError {faultResult.missingDetectingError},\ntotalClassificationError {faultResult.totalClassificationError}");            
        } 
        static void Main(string[] args)
        {
            int pointsCount = 10000;
            Random rand = new Random();
            int[] pointsArray1=new int[pointsCount];
            int[] pointsArray2=new int[pointsCount];
            int range = 1000;
            int offset = 150;
            double PC1 = 0.3;
            double PC2 = 0.7;
            double mathExcept1 = 0;
            double sigma1 = 0;
            double mathExcept2 = 0;
            double sigma2 = 0;
            for (int i = 0; i < pointsCount; i++)
            {
                pointsArray1[i] = rand.Next(range) - offset;
                pointsArray2[i] = rand.Next(range) + offset;
            }
            FaultResult faultResult = GaussianGenerator.Generate(PC1, PC2, pointsCount, pointsArray1, pointsArray2, out mathExcept1, out sigma1, out mathExcept2, out sigma2);
            WriteResult(faultResult);
        }
    }
}
