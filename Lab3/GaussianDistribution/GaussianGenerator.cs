using System;

namespace GaussianDistribution
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y = 0)
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
    public class WrongProbabilityException:Exception {
        public WrongProbabilityException() {}    
    }
    public class FaultResult {

        public double falseAlarmError;
        public double missingDetectingError;
        public double borderX;
        public double totalClassificationError;
        public FaultResult(double FalseAlarmError,double MissingDetectingError, double BorderX = double.NaN) {
            falseAlarmError = FalseAlarmError;
            missingDetectingError = MissingDetectingError;
            totalClassificationError = missingDetectingError + falseAlarmError;
            borderX = BorderX;
        }
    }
    public static class GaussianGenerator
    {
        public static double GaussFunction(double x, double mathExcept, double sigma) {
            return Math.Exp(-0.5 * Math.Pow((x - mathExcept) / sigma, 2)) / (sigma * Math.Sqrt(2 * Math.PI)); ;
        }
        public static FaultResult Generate(double PC1, double PC2, int pointsCount, out double MathExcept1, out double Sigma1, out double MathExcept2, out double Sigma2, int range = 800, int offset = 150)
        {
            if ((PC1 > 1) || (PC2 > 1) || (PC1 + PC2 != 1))
            {
                throw new WrongProbabilityException();
            }
            double falseAlarmError = 0;
            double missingDetectingError = 0;
            int[] pointsArray1 = new int[pointsCount]; 
            int[] pointsArray2 = new int[pointsCount]; 
            double mathExcept1 = 0;
            double sigma1 = 0;
            double mathExcept2 = 0;
            double sigma2 = 0;
            Random rand = new Random();
            for (int i = 0; i < pointsCount; i++)
            {
                pointsArray1[i] = rand.Next(range) - offset;
                pointsArray2[i] = rand.Next(range) + offset;
            }
            for (int i = 0; i < pointsCount; i++)
            {
                mathExcept1 += pointsArray1[i];
                mathExcept2 += pointsArray2[i];
            }
            mathExcept1 /= pointsCount;
            mathExcept2 /= pointsCount;
            for (int i = 0; i < pointsCount; i++)
            {
                sigma1 += Math.Pow(pointsArray1[i] - mathExcept1, 2);
                sigma2 += Math.Pow(pointsArray2[i] - mathExcept2, 2);
            }
            sigma1 = Math.Sqrt(sigma1 / pointsCount);
            sigma2 = Math.Sqrt(sigma2 / pointsCount);
            double Eps = 0.001;
            double x = -offset;
            x = 0;
            double p1 = 1;
            double p2 = 0;
            if (PC2 != 0)
            {
                while (p2 < p1)
                {
                    p1 = PC1 * GaussFunction(x, mathExcept1, sigma1);
                    p2 = PC2 * GaussFunction(x, mathExcept2, sigma2); 
                    falseAlarmError += p2 * Eps;
                    x += Eps;
                }
            }
            double borderX = x;
            while (x < pointsCount+offset)
            {
                p1 = GaussFunction(x, mathExcept1, sigma1);
                p2 = GaussFunction(x, mathExcept2, sigma2);
                missingDetectingError += p1 * PC1 * Eps;
                x += Eps;
            }

            MathExcept1 = mathExcept1;
            Sigma1 = sigma1;
            MathExcept2 = mathExcept2;
            Sigma2 = sigma2;
            if (PC1 == 0.0)
            {
                return new FaultResult(1, 0);
            }
            else
            {
                if (PC2 == 0.0)
                {
                    return new FaultResult(0, 0);
                }
                else
                {
                    return new FaultResult(falseAlarmError /= PC1, missingDetectingError /= PC1, borderX);
                }
            }
        }
    }
}
