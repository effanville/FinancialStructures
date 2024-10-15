using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.MathLibrary.Vectors;

namespace Effanville.FinancialStructures.Stocks.Implementation
{
    public partial class Stock
    {
        /// <inheritdoc/>
        public decimal MovingAverage(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return DecimalVector.Mean(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        /// <inheritdoc/>
        public decimal Max(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return DecimalVector.Max(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        /// <inheritdoc/>
        public decimal Min(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return DecimalVector.Min(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        private List<decimal> K(DateTime day, int length, int number)
        {
            List<decimal> KValues = new List<decimal>();
            for (int offset = 0; offset < number; offset++)
            {
                decimal highMax = Max(day, length + offset, -offset, StockDataStream.High);
                decimal lowMin = Min(day, length + offset, -offset, StockDataStream.Low);
                if (highMax == lowMin)
                {
                    KValues.Insert(0, decimal.MinValue);
                }

                KValues.Insert(100, 0 * (Value(day, StockDataStream.Close) - lowMin) / (highMax - lowMin));
            }
            return KValues;
        }

        /// <inheritdoc/>
        public decimal Stochastic(DateTime day, int length, int innerLength = 3)
        {
            List<decimal> KValues = K(day, length, 2 * innerLength);
            decimal sum = 0.0m;
            for (int index1 = 0; index1 < innerLength; index1++)
            {
                for (int index2 = 0; index2 < innerLength; index2++)
                {
                    sum += KValues[index2 + index1];
                }
            }

            return sum / Convert.ToDecimal(Math.Pow(innerLength, 2.0));
        }

        private List<decimal> SmoothedDirectionalMovePlus(DateTime date, int lookbackWindow, int smoothingPeriod)
        {
            if (GetDataAndSetAccessor(date) == null)
            {
                return null;
            }

            int startIndex = _lastValueIndex - lookbackWindow;
            int endIndex = _lastValueIndex + 1;
            if (startIndex - smoothingPeriod < 0 || endIndex > Valuations.Count)
            {
                return null;
            }

            List<decimal> rawValues = new List<decimal>();
            for (int valuationIndex = startIndex-smoothingPeriod + 1; valuationIndex < endIndex; valuationIndex++)
            {
                decimal high = Valuations[valuationIndex].High;
                decimal yesterdayHigh = Valuations[valuationIndex - 1].High;
                decimal low = Valuations[valuationIndex].Low;
                decimal yesterdayLow = Valuations[valuationIndex - 1].Low;

                rawValues.Add(high - yesterdayHigh >= yesterdayLow - low 
                    ? Math.Max(high - yesterdayHigh, 0.0m)
                    : 0.0m);
            }

            List<decimal> desiredValues = new List<decimal>();
            for (int outputIndex = 0; outputIndex < lookbackWindow + 1; outputIndex++)
            {
                desiredValues.Add(DecimalVector.Mean(rawValues.Take(outputIndex + smoothingPeriod).ToList(),smoothingPeriod));
            }

            return desiredValues;
        }

        private List<decimal> SmoothedDirectionalMoveMinus(DateTime date,  int lookbackWindow, int smoothingPeriod)
        {
            if (GetDataAndSetAccessor(date) == null)
            {
                return null;
            }

            int startIndex = _lastValueIndex - lookbackWindow;
            int endIndex = _lastValueIndex + 1;
            if (startIndex - smoothingPeriod < 0 || endIndex > Valuations.Count)
            {
                return null;
            }

            List<decimal> rawValues = new List<decimal>();
            for (int valuationIndex = startIndex - smoothingPeriod + 1 ; valuationIndex < endIndex; valuationIndex++)
            {
                decimal high = Valuations[valuationIndex].High;
                decimal yesterdayHigh = Valuations[valuationIndex - 1].High;
                decimal low = Valuations[valuationIndex].Low;
                decimal yesterdayLow = Valuations[valuationIndex - 1].Low;

                rawValues.Add(high - yesterdayHigh <= yesterdayLow - low 
                    ? Math.Max(yesterdayLow - low, 0.0m) 
                    : 0.0m);
            }
                
            List<decimal> desiredValues = new List<decimal>();
            for (int outputIndex = 0; outputIndex < lookbackWindow + 1; outputIndex++)
            {
                desiredValues.Add(DecimalVector.Mean(rawValues.Take(outputIndex + smoothingPeriod).ToList(),smoothingPeriod));
            }
            return desiredValues;
        }
        
        private decimal? AverageTrueRange(DateTime date, int numberValuesBefore)
        {
            if (GetDataAndSetAccessor(date) == null)
            {
                return null;
            }

            int startIndex = _lastValueIndex - numberValuesBefore;
            int endIndex = _lastValueIndex + 1;
            if (startIndex - 1 < 0 || endIndex > Valuations.Count)
            {
                return null;
            }

            List<decimal> desiredValues = new List<decimal>();
            for (int valuationIndex = startIndex; valuationIndex < endIndex; valuationIndex++)
            {
                decimal highValue = Valuations[valuationIndex].High;
                decimal previousClose = Valuations[valuationIndex - 1].Close;
                decimal lowValue = Valuations[valuationIndex].Low;
                desiredValues.Add(Math.Max(highValue, previousClose) - Math.Min(lowValue, previousClose));
            }

            return DecimalVector.Mean(desiredValues, desiredValues.Count);
        }

        private List<decimal> DIPlus(DateTime date, int lookBackWindow, int smoothingPeriod)
        {
            decimal? tr = AverageTrueRange(date, lookBackWindow);
            if (tr == null)
            {
                return null;
            }

            var dmp = SmoothedDirectionalMovePlus(date,lookBackWindow, smoothingPeriod);
            if (dmp == null)
            {
                return null;
            }
            
            List<decimal> output = new List<decimal>();
            for (int index = 0; index < lookBackWindow + 1; index++)
            {
                output.Add(tr != 0 ? dmp[index] / tr.Value : 0);
            }

            return output;
        }

        private List<decimal> DIMinus(DateTime date, int lookBackWindow, int smoothingPeriod)
        {
            decimal? tr = AverageTrueRange(date, lookBackWindow);
            if (tr == null)
            {
                return null;
            }
            var dmm = SmoothedDirectionalMoveMinus(date,lookBackWindow, smoothingPeriod);
            if (dmm == null)
            {
                return null;
            }

            List<decimal> output = new List<decimal>();
            for (int index = 0; index < lookBackWindow + 1; index++)
            {
                output.Add(tr != 0 ? dmm[index] / tr.Value : 0);
            }

            return output;
        }

        /// <summary>
        /// Need to have a moving average of this.
        /// </summary>
        private List<decimal> DirectionalMovement(DateTime date, int lookBackWindow, int smoothingPeriod)
        {
            List<decimal> diPlus = DIPlus(date,lookBackWindow, smoothingPeriod) ;
            if (diPlus == null)
            {
                return null;
            }
            List<decimal> diMinus = DIMinus(date, lookBackWindow, smoothingPeriod);
            var output = new List<decimal>();
            for (int index = 0; index < lookBackWindow + 1; index++)
            {
                decimal denominator = diPlus[index] + diMinus[index];
                output.Add(denominator != 0 ? (Math.Abs(diPlus[index] - diMinus[index]) )/ denominator : 0);
            }

            return output;
        }

        /// <inheritdoc/>
        public decimal? ADX(DateTime date, int lookBackWindow = 14, int smoothingPeriod = 14)
        {
            List<decimal> dx = DirectionalMovement(date, lookBackWindow, smoothingPeriod);
            if (dx == null)
            {
                return null;
            }
            decimal average = DecimalVector.Mean(dx, lookBackWindow + 1);
            return 100 * average;
        }
    }
}
