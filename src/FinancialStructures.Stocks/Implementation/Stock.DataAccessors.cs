﻿using System;
using System.Collections.Generic;

namespace FinancialStructures.Stocks.Implementation
{
    public partial class Stock
    {
        /// <inheritdoc/>
        public DateTime EarliestTime()
        {
            if (Valuations.Count == 0)
            {
                return DateTime.MinValue;
            }
            return Valuations[0].Start;
        }

        /// <inheritdoc/>
        public DateTime LastTime()
        {
            if (Valuations.Count == 0)
            {
                return DateTime.MinValue;
            }
            return Valuations[Valuations.Count - 1].Start;
        }

        /// <summary>
        /// This retrieves the data on the date <paramref name="date"/> as well
        /// as setting <see cref="LastValueIndex"/> to the index of this
        /// value.
        /// </summary>
        public StockDay GetData(DateTime date)
        {
            var data = GetDataAndSetAccessor(date);
            if (data == null)
            {
                return null;
            }

            return new StockDay(data.Start, data.Open, data.High, data.Low, data.Close, data.Volume);
        }

        /// <inheritdoc/>
        public decimal Value(DateTime date, StockDataStream data = StockDataStream.Close)
        {
            return GetDataAndSetAccessor(date)?.Value(data) ?? decimal.MinValue;
        }

        /// <inheritdoc/>
        public List<decimal> Values(DateTime date, int numberValuesBefore, int numberValuesAfter = 0, StockDataStream data = StockDataStream.Close)
        {
            _ = GetDataAndSetAccessor(date);
            List<decimal> desiredValues = new List<decimal>();
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
        private decimal Value(int valuationIndex, StockDataStream data = StockDataStream.Close)
        {
            return Valuations[valuationIndex].Value(data);
        }

        /// <summary>
        /// This retrieves the data at the time specified in <paramref name="date"/> as well
        /// as setting <see cref="LastValueIndex"/> to the index of this
        /// value. If the time is within the duration of the Day here, then only the open
        /// is returned.
        /// </summary>
        private StockDay GetDataAndSetAccessor(DateTime date)
        {
            int numberValues = Valuations.Count;
            int dayIndex = 0;
            do
            {
                dayIndex++;
            }
            while (dayIndex < numberValues && date >= Valuations[dayIndex].Start);

            LastValueIndex = dayIndex - 1;
            var value = Valuations[dayIndex - 1];
            var endTime = value.End;
            if (date >= endTime)
            {
                return value;
            }
            if (date >= value.Start)
            {
                return value.CopyAsOpenOnly();
            }

            return null;
        }
    }
}