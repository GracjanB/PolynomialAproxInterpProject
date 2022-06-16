using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PolynomialAproxInterpProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Polynomial _polynomial;
        private List<ResultApproximation> _result;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateAproximation_Click(object sender, RoutedEventArgs e)
        {
            List<double> arrayX = new();
            List<double> arrayY = new();

            for (int i = -10; i < 10; i++)
            {
                arrayX.Add(i);
                arrayY.Add(_polynomial.Evaluate(i));
            }

            var polynomialPoints = new FunctionPoints()
            {
                ArrayX = arrayX.ToArray(),
                ArrayY = arrayY.ToArray()
            };

            var points = Approximation.CalculatePolynomialPoints(polynomialPoints);
            List<ResultApproximation> result = new();

            for (int i = 0; i < polynomialPoints.ArrayX.Length; i++)
            {
                result.Add(new ResultApproximation
                {
                    X_Value = polynomialPoints.ArrayX[i],
                    Y_Value = polynomialPoints.ArrayY[i],
                    ApproximatedValue = Math.Round(points[i], 4),
                    Error = Math.Round((Math.Abs(polynomialPoints.ArrayY[i] - points[i]) / polynomialPoints.ArrayY[i]) * 100, 2)
                });
            }

            _result = result;
            ResultDataGrid.ItemsSource = result;
        }

        private void CalculateInterpolation_Click(object sender, RoutedEventArgs e)
        {
            List<double> arrayX = new();
            List<double> arrayY = new();

            for (int i = -10; i < 10; i++)
            {
                arrayX.Add(i);
                arrayY.Add(_polynomial.Evaluate(i));
            }

            var polynomialPoints = new FunctionPoints()
            {
                ArrayX = arrayX.ToArray(),
                ArrayY = arrayY.ToArray()
            };

            var degree = int.Parse(InterpolationDegreeTextBox.Text);
            var points = Interpolation.CalculatePolynomialCoefficients(polynomialPoints, degree);

            var coefficients = new List<double>();

            foreach (var point in points)
            {
                coefficients.Add(Math.Round(point, 4));
            }

            var approximatedPolynomial = new Polynomial(coefficients);

            List<ResultApproximation> result = new();

            for (int i = 0; i < polynomialPoints.ArrayX.Length; i++)
            {
                result.Add(new ResultApproximation
                {
                    X_Value = polynomialPoints.ArrayX[i],
                    Y_Value = polynomialPoints.ArrayY[i],
                    ApproximatedValue = approximatedPolynomial.Evaluate(polynomialPoints.ArrayX[i]),
                    Error = Math.Round((Math.Abs(polynomialPoints.ArrayY[i] - approximatedPolynomial.Evaluate(polynomialPoints.ArrayX[i])) / polynomialPoints.ArrayY[i]) * 100, 2) 
                });
            }

            _result = result;
            ResultDataGrid.ItemsSource = result;
        }

        private void GeneratePolynomial_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PolynomialCoeficientsTextBox.Text))
            {
                var coefficients = PolynomialCoeficientsTextBox.Text.Split(" ").ToList();
                bool validationResult = true;

                coefficients.ForEach(coefficient =>
                {
                    validationResult &= char.IsDigit(coefficient[0]);
                });

                if (validationResult)
                {
                    List<double> coefficients2 = new();

                    coefficients.ForEach(coefficient =>
                    {
                        var coefficientt = double.Parse(coefficient);
                        coefficients2.Add(coefficientt);
                    });

                    _polynomial = new Polynomial(coefficients2.ToArray());
                    PolynomialTextBox.Text = _polynomial.ToStringDescending();
                }
                else
                {
                    MessageBox.Show("Nieprawidłowy format wprowadzonych współczynników wielomianu");
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("X_value;Y_value;Y_value_approx;Error");

            _result.ForEach(item =>
            {
                stringBuilder.AppendLine($"{item.X_Value};{item.Y_Value};{item.ApproximatedValue};{item.Error}");
            });

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik CSV (*.csv)|*.csv";
            saveFileDialog.DefaultExt = "csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());

                MessageBox.Show("Zapisano.");
            }
        }
    }

    
}
