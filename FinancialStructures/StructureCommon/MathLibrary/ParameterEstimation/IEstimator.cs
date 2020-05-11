namespace StructureCommon.MathLibrary.ParameterEstimation
{
    /// <summary>
    /// Container for any estimator of values.
    /// </summary>
    public interface IEstimator
    {
        int NumberOfParameters { get; }

        int NumberOfDataPoints { get; }

        double[,] FitData { get; }
        double[] FitValues { get; }

        /// <summary>
        /// Returns the values one has estimated.
        /// </summary>
        double[] Estimator { get; }

        /// <summary>
        /// The covariance matrix of the estimator.
        /// </summary>
        double[,] Uncertainty { get; }

        /// <summary>
        /// The quality of the fit given by the estimator.
        /// </summary>
        double GoodnessOfFit { get; }

        /// <summary>
        /// Evaluates the point using the estimator calculated.
        /// </summary>
        /// <param name="point">The value at which to evaluate at.</param>
        double Evaluate(double[] point);

        /// <summary>
        /// Calculates the values of the estimator from the data and expected values.
        /// </summary>
        /// <param name="data">The data values to use.</param>
        /// <param name="values">The expected outcomes for each row of the data matrix.</param>
        void GenerateEstimator(double[,] data, double[] values);
    }
}
