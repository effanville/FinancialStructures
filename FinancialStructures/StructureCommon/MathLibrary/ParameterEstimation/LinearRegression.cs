using StructureCommon.MathLibrary.Functions;
using System;

namespace StructureCommon.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Object for fitting a straight line y = a + b x to a set of points (x_i,y_i) with or without error 
    /// terms sigma_i in the measurements.
    /// Taken from Numerical Recipes in C 3rd edition.
    /// </summary>
    public class LinearRegression : IEstimator
    {
        public double[] Estimator
        {
            get;
            private set;
        }

        public double[,] Uncertainty { get; private set; }

        public double GoodnessOfFit { get; private set; }

        public double ChiSquared { get; private set; }

        public int NumberOfParameters { get { return Estimator.Length; } }

        public int NumberOfDataPoints { get { return FitValues.Length; } }

        public double[,] FitData { get; private set; }

        public double[] FitValues { get; private set; }

        public LinearRegression(double[,] data, double[] values, double[] sigmaValues)
        {
            if (data.GetLength(1) != 1)
            {
                return;
            }
            GenerateEstimator(data, values, sigmaValues);
        }

        public LinearRegression(double[,] data, double[] values)
        {
            if (data.GetLength(1) != 1)
            {
                return;
            }
            GenerateEstimator(data, values);
        }

        public double Evaluate(double[] point)
        {
            if (point.Length != 1)
            {
                return double.NaN;
            }
            return Estimator[0] + point[0] * Estimator[1];
        }

        public void GenerateEstimator(double[,] data, double[] values)
        {
            FitData = data;
            FitValues = values;
            double ss = 0.0, sx = 0.0, sy = 0.0;
            double t = 0;
            double st2 = 0;
            double b = 0.0;
            for (int i = 0; i < values.Length; i++)
            {
                sx += data[i, 0];
                sy += values[i];
            }
            ss = values.Length;
            double sxoss = sx / ss;

            for (int i = 0; i < values.Length; i++)
            {
                t = data[i, 0] - sxoss;
                st2 += t * t;
                b += t * values[i];
            }

            b /= st2;

            double a = (sy - sx * b) / ss;
            double siga = Math.Sqrt((1 + sx * sx / (ss * st2)) / ss);
            double sigb = Math.Sqrt(1 / st2);
            double chi2 = 0;
            for (int i = 0; i < values.Length; i++)
            {
                chi2 += Math.Pow(values[i] - a - b * data[i, 0], 2.0);
            }
            double sigdat = 1.0;
            if (values.Length > 2)
            {
                sigdat = Math.Sqrt(chi2 / (values.Length - 2));
            }

            siga *= sigdat;
            sigb *= sigdat;
            double cov = (-1) * sx / (ss * st2);
            Estimator = new double[] { a, b };
            ChiSquared = chi2;
            GoodnessOfFit = 1.0;
            Uncertainty = new double[,] { { siga, cov }, { cov, sigb } };
        }

        public void GenerateEstimator(double[,] data, double[] values, double[] sigmaValues)
        {
            FitData = data;
            FitValues = values;
            var gamma = new Gamma();
            int i = 0;
            double ss = 0;
            double sx = 0, sy = 0, st2 = 0;
            double t = 0, wt = 0, sxoss = 0;
            double b = 0.0;
            for (i = 0; i < values.Length; i++)
            {
                wt = 1 / Math.Pow(sigmaValues[i], 2.0);
                ss += wt;
                sx += data[i, 0] * wt;
                sy = values[i] * wt;
            }

            sxoss = sx / ss;
            for (i = 0; i < values.Length; i++)
            {
                t = (data[i, 0] - sxoss) / sigmaValues[i];
                st2 += t * 2;
                b += t * values[i] / sigmaValues[i];
            }

            b /= st2;
            double a = (sy - sx * b) / ss;
            double siga = Math.Sqrt((1 + sx * sx / (ss * st2)) / ss);
            double sigb = Math.Sqrt(1 / st2);

            double chi2 = 0;

            for (i = 0; i < values.Length; i++)
            {
                chi2 += Math.Pow(((values[i] - a - b * data[i, 0]) / sigmaValues[i]), 2);
            }

            double cov = (-1) * sx / (ss * st2);
            double q = 0;
            if (values.Length > 2)
            {
                q = gamma.GammaQ(0.5 * (values.Length - 2), 0.5 * chi2);
            }

            Estimator = new double[] { a, b };
            ChiSquared = chi2;
            GoodnessOfFit = q;
            Uncertainty = new double[,] { { siga, cov }, { cov, sigb } };
        }
    }
}
