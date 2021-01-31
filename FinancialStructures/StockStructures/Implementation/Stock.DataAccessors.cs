using System;
using System.Collections.Generic;

namespace FinancialStructures.StockStructures.Implementation
{
    public partial class Stock
    {
        /// <inheritdoc/>
        public DateTime EarliestTime()
        {
            return Valuations[0].Time;
        }

        /// <inheritdoc/>
        public DateTime LastTime()
        {
            return Valuations[Valuations.Count - 1].Time;
        }

        /// <inheritdoc/>
        public double Value(DateTime date, StockDataStream data = StockDataStream.Close)
        {
            return GetDataAndSetAccessor(date).Value(data);
        }

        /// <inheritdoc/>
        public List<double> Values(DateTime date, int numberValuesBefore, int numberValuesAfter = 0, StockDataStream data = StockDataStream.Close)
        {
            _ = GetDataAndSetAccessor(date);
            List<double> desiredValues = new List<double>();
            for (int valuationIndex = LastValueIndex - numberValuesBefore + 1; valuationIndex < LastValueIndex + numberValuesAfter + 1; valuationIndex++)
            {
                desiredValues.Add(Valuations[valuationIndex].Value(data));
            }

            return desiredValues;
        }

        /// <summary>
        /// Calculates the value of the stock at the index in the list of values.
        /// This does not set <see cref="LastValueIndex"/>.
        /// </summary>
        private double Value(int valuationIndex, StockDataStream data = StockDataStream.Close)
        {
            return Valuations[valuationIndex].Value(data);
        }

        /// <summary>
        /// This retrieves the data on the date <paramref name="date"/> as well
        /// as setting <see cref="LastValueIndex"/> to the index of this
        /// value.
        /// </summary>
        private StockDay GetDataAndSetAccessor(DateTime date)
        {
            int numberValues = Valuations.Count;
            int dayIndex = 0;
            do
            {
                dayIndex++;
            } while (date > Valuations[dayIndex].Time && dayIndex < numberValues);

            LastValueIndex = dayIndex - 1;
            return Valuations[dayIndex - 1];
        }
    }
}
