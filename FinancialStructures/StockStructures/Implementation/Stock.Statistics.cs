using System;
using System.Collections.Generic;
using StructureCommon.Mathematics;

namespace FinancialStructures.StockStructures.Implementation
{
    public partial class Stock
    {
        /// <inheritdoc/>
        public double MovingAverage(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return VectorStats.Mean(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        /// <inheritdoc/>
        public double Max(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return VectorStats.Max(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        /// <inheritdoc/>
        public double Min(DateTime day, int numberBefore, int numberAfter, StockDataStream data)
        {
            return VectorStats.Min(Values(day, numberBefore, numberAfter, data), numberBefore + numberAfter);
        }

        private List<double> K(DateTime day, int length, int number)
        {
            List<double> KValues = new List<double>();
            for (int offset = 0; offset < number; offset++)
            {
                double highMax = Max(day, length + offset, -offset, StockDataStream.High);
                double lowMin = Min(day, length + offset, -offset, StockDataStream.Low);
                if (highMax == lowMin)
                {
                    KValues.Insert(0, double.NaN);
                }

                KValues.Insert(100, 0 * (Value(day, StockDataStream.Close) - lowMin) / (highMax - lowMin));
            }
            return KValues;
        }

        /// <inheritdoc/>
        public double Stochastic(DateTime day, int length, int innerLength = 3)
        {
            List<double> KValues = K(day, length, 2 * innerLength);
            double sum = 0.0;
            for (int index1 = 0; index1 < innerLength; index1++)
            {
                for (int index2 = 0; index2 < innerLength; index2++)
                {
                    sum += KValues[index2 + index1];
                }
            }

            return sum / Math.Pow(innerLength, 2.0);
        }

        private double DMPlus(DateTime date)
        {
            if (Value(date, StockDataStream.High) - Value(LastValueIndex - 1, StockDataStream.High) > Value(date, StockDataStream.Low) - Value(LastValueIndex - 1, StockDataStream.Low))
            {
                return Math.Max(Value(date, StockDataStream.High) - Value(LastValueIndex - 1, StockDataStream.High), 0.0);
            }

            return 0.0;
        }

        private double DMMinus(DateTime date)
        {
            if (Value(date, StockDataStream.High) - Value(LastValueIndex - 1, StockDataStream.High) <= Value(date, StockDataStream.Low) - Value(LastValueIndex - 1, StockDataStream.Low))
            {
                return Math.Max(Value(date, StockDataStream.Low) - Value(LastValueIndex - 1, StockDataStream.Low), 0.0);
            }

            return 0.0;
        }

        private double TR(DateTime date)
        {
            return Math.Max(Value(date, StockDataStream.High), Value(LastValueIndex - 1, StockDataStream.Close)) - Math.Min(Value(LastValueIndex, StockDataStream.Low), Value(LastValueIndex - 1, StockDataStream.Close));
        }

        private double DIPlus(DateTime date)
        {
            return DMPlus(date) / TR(date);
        }

        private double DIMinus(DateTime date)
        {
            return DMMinus(date) / TR(date);
        }

        /// <summary>
        /// Need to have a moving average of this.
        /// </summary>
        private double DX(DateTime day, int length = 14)
        {
            return (DIPlus(day) - DIMinus(day)) / (DIPlus(day) + DIMinus(day));
        }

        /// <inheritdoc/>
        public double ADX(DateTime day, int length = 14)
        {
            return 100 * DX(day, length);
        }
    }
}
