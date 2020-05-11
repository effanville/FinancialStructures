using StructureCommon.MathLibrary.Matrices;

namespace StructureCommon.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Holds data on least squares estimator for a matrix of data inputs and corresponding y values.
    /// </summary>
    public class LSEstimator : IEstimator
    {
        public double[] Estimator { get; private set; }

        public double[,] Uncertainty { get; private set; }

        public double GoodnessOfFit { get; private set; }

        public int NumberOfParameters { get { return Estimator.Length; } }

        public int NumberOfDataPoints { get { return FitValues.Length; } }

        public double[,] FitData { get; private set; }

        public double[] FitValues { get; private set; }

        public LSEstimator(double[,] data, double[] values)
        {
            GenerateEstimator(data, values);
        }

        public double Evaluate(double[] point)
        {
            if (Estimator.Length != point.Length)
            {
                return double.NaN;
            }
            double value = 0.0;
            for (int index = 0; index < Estimator.Length; index++)
            {
                value += Estimator[index] * point[index];
            }

            return value;
        }

        public void GenerateEstimator(double[,] data, double[] values)
        {
            FitData = data;
            FitValues = values;
            var XTY = data.Transpose().PostMultiplyVector(values);
            Estimator = data.XTX().Inverse().PostMultiplyVector(XTY);
        }
    }
}
