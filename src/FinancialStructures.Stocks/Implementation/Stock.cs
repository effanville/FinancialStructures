﻿using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Stocks.Implementation
{
    /// <summary>
    /// Simulates a Stock.
    /// </summary>
    public partial class Stock : IStock
    {
        private List<StockDay> fValuations = new();
        internal int LastValueIndex = 0;

        /// <inheritdoc/>
        public NameData Name
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public List<StockDay> Valuations
        {
            get => fValuations;
            set
            {
                fValuations = value;
                fValuations.Sort();
            }
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Stock()
        {
        }

        /// <summary>
        /// Constructor setting basic name information
        /// </summary>
        public Stock(string ticker, string company, string name, string currency, string url)
        {
            Name = new NameData(company.Trim(), name.Trim(), currency, url.Trim());
            Name.Ticker = ticker;
            Valuations = new List<StockDay>();
        }

        public void AddValue(StockDay candle)
        {
            Valuations.Add(candle);
        }

        /// <inheritdoc/>
        public void AddValue(DateTime time, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            AddValue(new StockDay(time, open, high, low, close, volume));
        }

        /// <inheritdoc/>
        public void AddOrEditValue(DateTime time, decimal? newOpen = null, decimal? newHigh = null, decimal? newLow = null, decimal? newClose = null, decimal? newVolume = null)
        {
            var value = Valuations.FirstOrDefault(val => val.Start.Equals(time));
            if (value == null)
            {
                AddValue(time, newOpen ?? 0.0m, newHigh ?? 0.0m, newLow ?? 0.0m, newClose ?? 0.0m, newVolume ?? 0.0m);
                return;
            }

            if (newOpen.HasValue)
            {
                value.Open = newOpen.Value;
            }
            if (newHigh.HasValue)
            {
                value.High = newHigh.Value;
            }
            if (newLow.HasValue)
            {
                value.Low = newLow.Value;
            }
            if (newClose.HasValue)
            {
                value.Close = newClose.Value;
            }
            if (newVolume.HasValue)
            {
                value.Volume = newVolume.Value;
            }
        }

        /// <inheritdoc/>
        public void Sort()
        {
            Valuations.Sort();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Stock: {Name.Ticker}-{Name}-{Valuations.Count}";
        }

        /// <summary>
        /// Takes a copy of the stock with all data strictly before <paramref name="date"/>
        /// </summary>
        public Stock Copy(DateTime date)
        {
            var stock = new Stock(Name.Ticker, Name.Company, Name.Name, Name.Currency, Name.Url);
            foreach (var valuation in Valuations)
            {
                if (valuation.Start < date)
                {
                    stock.AddValue(valuation.Start, valuation.Open, valuation.High, valuation.Low, valuation.Close, valuation.Volume);
                }
            }
            return stock;
        }
    }
}