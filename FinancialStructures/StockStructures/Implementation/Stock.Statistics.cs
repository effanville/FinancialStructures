using System;
using System.Collections.Generic;
using Common.Structure.MathLibrary.Vectors;

namespace FinancialStructures.StockStructures.Implementation
{
    public partial class Stock
    {
        /// <inheritdoc/>
        public decimal MovingAverage(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return VectorStats.Mean(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        /// <inheritdoc/>
        public decimal Max(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return VectorStats.Max(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        /// <inheritdoc/>
        public decimal Min(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return VectorStats.Min(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
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

        private decimal DMPlus(DateTime date)
        {
            if (Value(date, StockDataStream.High) - Value(LastValueIndex - 1, StockDataStream.High) > Value(date, StockDataStream.Low) - Value(LastValueIndex - 1, StockDataStream.Low))
            {
                return Math.Max(Value(date, StockDataStream.High) - Value(LastValueIndex - 1, StockDataStream.High), 0.0m);
            }

            return 0.0m;
        }

        private decimal DMMinus(DateTime date)
        {
            if (Value(date, StockDataStream.High) - Value(LastValueIndex - 1, StockDataStream.High) <= Value(date, StockDataStream.Low) - Value(LastValueIndex - 1, StockDataStream.Low))
            {
                return Math.Max(Value(date, StockDataStream.Low) - Value(LastValueIndex - 1, StockDataStream.Low), 0.0m);
            }

            return 0.0m;
        }

        private decimal TR(DateTime date)
        {
            return Math.Max(Value(date, StockDataStream.High), Value(LastValueIndex - 1, StockDataStream.Close)) - Math.Min(Value(LastValueIndex, StockDataStream.Low), Value(LastValueIndex - 1, StockDataStream.Close));
        }

        private decimal DIPlus(DateTime date)
        {
            return DMPlus(date) / TR(date);
        }

        private decimal DIMinus(DateTime date)
        {
            return DMMinus(date) / TR(date);
        }

        /// <summary>
        /// Need to have a moving average of this.
        /// </summary>
        private decimal DX(DateTime day, int length = 14)
        {
            return (DIPlus(day) - DIMinus(day)) / (DIPlus(day) + DIMinus(day));
        }

        /// <inheritdoc/>
        public decimal ADX(DateTime day, int length = 14)
        {
            return 100 * DX(day, length);
        }
    }
}
