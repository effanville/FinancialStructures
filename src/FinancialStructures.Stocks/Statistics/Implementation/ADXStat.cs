using System;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    internal class AdxStat : IStockStatistic
    {
        public int SmoothingPeriod { get; }
        
        /// <inheritdoc/>
        public bool IsNormalised => true;
        
        /// <inheritdoc/>
        public int BurnInTime { get; }
        
        /// <inheritdoc/>
        public StockDataStream DataType => StockDataStream.None;

        public AdxStat(int numberDays)
        {
            BurnInTime = numberDays;
        }

        public AdxStat(StockStatisticSettings settings)
        {
            BurnInTime = settings.Lag;
            if (settings is AdxStatisticSettings adxStatisticSettings)
            {
                SmoothingPeriod = adxStatisticSettings.SmoothingPeriod;
            }
        }

        /// <inheritdoc/>
        public double Calculate(DateTime date, IStock stock)
        {
            decimal? adx = stock?.ADX(date, BurnInTime, SmoothingPeriod);
            if (!adx.HasValue)
            {
                return double.NaN;
            }
            return Convert.ToDouble(adx.Value);
        }
    }
}
