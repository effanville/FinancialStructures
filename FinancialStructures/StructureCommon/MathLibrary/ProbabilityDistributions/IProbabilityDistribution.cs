namespace StructureCommon.MathLibrary.ProbabilityDistributions
{
    internal interface IProbabilityDistribution
    {
        /// <summary>
        /// Returns the probability density function at the point x.
        /// </summary>
        double p(double x);

        /// <summary>
        /// Returns the cumulative distribution function at the point x.
        /// </summary>
        double cdf(double x);

        /// <summary>
        /// Returns the inverse cumulative distribution function.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        double invcdf(double p);
    }
}
