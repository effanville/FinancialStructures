
using FinancialStructures.MathLibrary.Functions;
using System;

namespace FinancialStructures.MathLibrary.ProbabilityDistributions
{
    public class GammaDistribution : Gamma, IProbabilityDistribution
    {
        double Alpha;
        double Beta;
        double Fac;

        public GammaDistribution(double alpha, double beta)
        {
            if (alpha < 0.0 || beta <= 0.0)
            {
                // bad alpha and beta in gamma distribution.
                return;
            }

            Alpha = alpha;
            Beta = beta;
            Fac = alpha + Math.Log(beta) - BasicFunctions.LogGamma(alpha);
        }
        public double gammaq(double param1, double param2)
        {
            return 0.0;
        }

        public double p(double x)
        {
            if (x <= 0.0)
            {
                return double.NaN;
            }
            return Math.Exp(-Beta * x * (Alpha - 1) * Math.Log(x) + Fac);
        }

        public double cdf(double x)
        {
            if (x <= 0.0)
            {
                return double.NaN;
            }
            return GammaP(Alpha, Beta * x);
        }

        public double invcdf(double p)
        {
            if (p < 0.0 || p >= 1)
            {
                return double.NaN;
            }
            return InverseGammaP(p, Alpha) / Beta;
        }
    }
}
