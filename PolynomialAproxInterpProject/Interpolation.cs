using MathNet.Numerics.LinearAlgebra;
using System;

namespace PolynomialAproxInterpProject
{
    public class Interpolation
    {
        public static double[] CalculatePolynomialCoefficients(FunctionPoints points, int polynomialDegree)
        {
            double[,] Xmatrix = new double[polynomialDegree + 1, polynomialDegree];
            double[] Avector = new double[polynomialDegree + 1];
            double[] y_array = new double[(2 * polynomialDegree) + 1];

            for (int j = 0; j != (2 * polynomialDegree); j++)
            {
                double sum = 0;

                for (int i = 0; i < points.ArrayX.Length; i++)
                {
                    sum += Math.Pow(points.ArrayX[i], j);
                }

                y_array[j] = sum;
            }

            for (int j = 0; j != polynomialDegree; j++)
            {
                for (int i = 0; i != polynomialDegree; i++)
                {
                    Xmatrix[i, j] = y_array[i + j];
                }

                double sum = 0;

                for (int i = 0; i < points.ArrayX.Length - 1; i++)
                {
                    sum += Math.Pow(points.ArrayX[i], j) * points.ArrayY[i];
                }

                Avector[j] = sum;
            }

            var x_matrix = Matrix<double>.Build.DenseOfArray(Xmatrix);
            var a_matrix = Vector<double>.Build.Dense(Avector);

            var result = x_matrix.Solve(a_matrix).ToArray();

            return result;
        }
    }
}
