using MathNet.Numerics.LinearAlgebra;
using System;

namespace PolynomialAproxInterpProject
{
    public class Approximation
    {
        public static double[] CalculatePolynomialPoints(FunctionPoints points)
        {
            double[] x = points.ArrayX;
            double[] y = points.ArrayY;
            int numberOfPoints = x.Length;

            double[,] inputMatrix = new double[numberOfPoints, numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                int polynomialDegree = numberOfPoints - 1;

                for (int j = 0; j < numberOfPoints; j++)
                {
                    inputMatrix[i, j] = Math.Pow(x[i], polynomialDegree);
                    polynomialDegree--;
                }
            }

            var A = Matrix<double>.Build.DenseOfArray(inputMatrix);
            var b = Vector<double>.Build.Dense(y);

            var result = A.Solve(b).ToArray();

            return result;
        }
    }
}
