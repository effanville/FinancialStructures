using System;
using Common.Structure.MathLibrary.Vectors;

namespace FinancialStructures.StockStructures.Statistics.Implementation
{
    internal class MovingAverage : IStockStatistic
    {
        /// <inheritdoc/>
        public StockDataStream DataType
        {
            get;
        }

        /// <inheritdoc/>
        public int BurnInTime
        {
            get;
        }

        /// <inheritdoc/>
        public StockStatisticType TypeOfStatistic
        {
            get;
        }

        public MovingAverage(int numberDays, StockDataStream dataStream, StockStatisticType typeOfStatistic)
        {
            BurnInTime = numberDays;
            TypeOfStatistic = typeOfStatistic;
            DataType = dataStream;
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            System.Collections.Generic.List<double> values = stock.Values(date, BurnInTime, 0, DataType);
            return VectorStats.Mean(values, BurnInTime);
        }
    }
}
