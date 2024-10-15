using System;

namespace Effanville.FinancialStructures.Stocks.Statistics.Implementation
{
    public class MovingAverageSettings : StockStatisticSettings
    {
        public int NumberAverageDays { get; }
        public int NumberAverageDaysTwo { get; }
        public bool Fraction { get; }
        public bool Difference => !Fraction;

        public MovingAverageSettings(bool fraction, int numberAverageDays, int numberAverageDaysTwo, string statType, StockDataStream priceType)
            : base(statType, Math.Max(numberAverageDays, numberAverageDaysTwo), priceType)
        {
            NumberAverageDays = numberAverageDays;
            NumberAverageDaysTwo = numberAverageDaysTwo;
            Fraction = fraction;
        }
    }
}