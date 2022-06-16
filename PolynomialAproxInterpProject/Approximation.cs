using MathNet.Numerics.LinearAlgebra;
using System;

namespace PolynomialAproxInterpProject
{
    public class Approximation
    {
        public static double[] CalculatePolynomialPoints(FunctionPoints points)
        {
            int numberOfPoints = points.ArrayX.Length;
            double[,] inputMatrix = new double[numberOfPoints, numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                int polynomialDegree = numberOfPoints - 1;

                for (int j = 0; j < numberOfPoints; j++)
                {
                    inputMatrix[i, j] = Math.Pow(points.ArrayX[i], polynomialDegree);
                    polynomialDegree--;
                }
            }

            var Amatrix = Matrix<double>.Build.DenseOfArray(inputMatrix);
            var BMatrix = Vector<double>.Build.Dense(points.ArrayY);

            var result = Amatrix.Solve(BMatrix).ToArray();

            return result;
        }
    }
}
