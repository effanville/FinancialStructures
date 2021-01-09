using System;
using System.Collections.Generic;

namespace FinancialStructures.StockStructures
{
    public partial class Stock
    {
        /// <summary>
        /// The earliest time held in the stock.
        /// </summary>
        public DateTime EarliestTime()
        {
            return Valuations[0].Time;
        }

        /// <summary>
        /// The latest time held in the Stock.
        /// </summary>
        public DateTime LastTime()
        {
            return Valuations[Valuations.Count - 1].Time;
        }

        /// <summary>
        /// Calculates the value of the stock at the time specified.
        /// </summary>
        public double Value(DateTime date, StockDataStream data = StockDataStream.Close)
        {
            return DayData(date).Value(data);
        }

        /// <summary>
        /// Calculates the value of the stock at the index in the list of values.
        /// </summary>
        public double Value(int index, StockDataStream data = StockDataStream.Close)
        {
            return Valuations[index].Value(data);
        }

        /// <summary>
        /// Returns a collection of values before and after the date specified.
        /// </summary>
        public List<double> Values(DateTime date, int numberValuesBefore, int numberValuesAfter = 0, StockDataStream data = StockDataStream.Close)
        {
            _ = DayData(date);
            List<double> desiredValues = new List<double>();
            for (int index = LastAccessedValuationIndex - numberValuesBefore + 1; index < LastAccessedValuationIndex + numberValuesAfter + 1; index++)
            {
                desiredValues.Add(Valuations[index].Value(data));
            }

            return desiredValues;
        }

        private StockDay DayData(DateTime date)
        {
            int numberValues = Valuations.Count;
            int dayIndex = 0;
            do
            {
                dayIndex++;
            } while (date > Valuations[dayIndex].Time && dayIndex < numberValues);

            LastAccessedValuationIndex = dayIndex - 1;
            return Valuations[dayIndex - 1];
        }
    }
}
