using FinancialStructures.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureCommon.FinanceFunctions
{
    /// <summary>
    /// Default financial based functions.
    /// </summary>
    public static class FinancialFunctions
    {
        /// <summary>
        /// Returns the compound annual rate from the firstValue to the lastValue in the time last-first
        /// </summary>
        public static double CAR(DateTime first, double firstValue, DateTime last, double lastValue)
        {
            if (firstValue == lastValue)
            {
                return 0.0;
            }
            if (firstValue == 0 || (last - first).Days == 0)
            {
                return double.NaN;
            }
            if (lastValue == 0)
            {
                return -1.0;
            }

            return Math.Pow(lastValue / firstValue, 365.0 / (last - first).Days) - 1;
        }

        /// <summary>
        /// Returns the compound annual rate from the <paramref name="firstValue"/> to the <paramref name="lastValue"/> in the time last-first
        /// </summary>
        public static double CAR(DailyValuation firstValue, DailyValuation lastValue)
        {
            if (firstValue == null || lastValue == null)
            {
                return double.NaN;
            }

            return CAR(firstValue.Day, firstValue.Value, lastValue.Day, lastValue.Value);
        }

        /// <summary>
        /// Function evaluation for the IRR method below. 
        /// This evaluates
        /// f(x) = <paramref name="latest.Value"/> -sum(<paramref name="investments"/>[i].Value x (1+<paramref name="expectedReturnRate"/>)^{<paramref name="latest.Date"/>-investments[i].Date})
        /// </summary>
        private static double Evaluator(List<DailyValuation> investments, DailyValuation latest, double expectedReturnRate)
        {
            double sum = 0;
            for (int i = 0; i < investments.Count(); ++i)
            {
                double modifier = Math.Pow(1.0 + expectedReturnRate, (latest.Day - investments[i].Day).Days / 365.0);
                sum += investments[i].Value * modifier;
            }

            return latest.Value - sum;
        }

        /// <summary>
        /// Returns the internal rate of return of the collection of <paramref name="investments"/>, having achieved the value <paramref name="latestValue"/>, via the bisection method.
        /// </summary>
        public static double IRR(List<DailyValuation> investments, DailyValuation latestValue)
        {
            if (investments == null || !investments.Any())
            {
                return double.NaN;
            }
            if (investments.Count == 1)
            {
                return CAR(investments[0], latestValue);
            }
            // More than one investment so have to perform bisection method.
            double lowestValue = -1;
            double highestValue = 2;

            // pre iteration checks (if function has same value at each side then return false
            double funcLow = Evaluator(investments, latestValue, lowestValue);
            double funcHigh = Evaluator(investments, latestValue, highestValue);

            if ((funcLow < 0 && funcHigh < 0) || (funcLow > 0 && funcHigh > 0))
            {
                return double.NaN;
            }

            int iterationNumber = 0;
            // stop after certain number of iterations (this here guarantees an error of 3x2^{-10}).
            while (iterationNumber < 10)
            {
                double middleValue = (lowestValue + highestValue) / 2.0;
                funcLow = Evaluator(investments, latestValue, lowestValue);
                funcHigh = Evaluator(investments, latestValue, highestValue);
                double funcMiddle = Evaluator(investments, latestValue, middleValue);

                if ((funcLow < 0 && funcMiddle < 0) || (funcLow > 0 && funcMiddle > 0))
                {
                    lowestValue = middleValue;
                }

                if ((funcHigh < 0 && funcMiddle < 0) || (funcHigh > 0 && funcMiddle > 0))
                {
                    highestValue = middleValue;
                }

                iterationNumber++;
            }

            return (lowestValue + highestValue) / 2.0;
        }

        /// <summary>
        /// Calculates Internal rate of return of a collection of investments over the last timelength number of months
        /// </summary>
        public static double IRRTime(DailyValuation startValue, List<DailyValuation> investments, DailyValuation latestValue)
        {
            if (latestValue == null || startValue == null || investments == null)
            {
                return double.NaN;
            }
            // easy case to get rid of
            if ((investments == null || investments.Count() == 0) && latestValue.Value == startValue.Value)
            {
                return 0.0;
            }
            // reduce number of investments to recent only
            var recentInvestments = new List<DailyValuation>
            {
                startValue
            };

            foreach (DailyValuation value in investments)
            {
                if (value == null)
                {
                    return double.NaN;
                }
                if (value.Day > startValue.Day)
                {
                    recentInvestments.Add(value);
                }
            }

            return IRR(recentInvestments, latestValue);
        }
    }
}
